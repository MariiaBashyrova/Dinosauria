# Dinosauria – UML-Klassendiagramm der Domänenlogik

## 1. Ziel und Umfang

Dieses Klassendiagramm beschreibt die Domänenlogik des Spiels Dinosauria. WPF-Oberflächen, EF-Core-Konfigurationen, Repository-Klassen und andere Infrastrukturdetails sind bewusst nicht enthalten.

Die Darstellung verwendet UML-Klassen mit Attributen, Methoden, Vererbung, Assoziationen und Kardinalitäten.

## 2. Bedingungen

`Aufgabenbedingung` ist die abstrakte Basisklasse für Bedingungen, die Spielziele und Freischaltungen beschreiben. Sie definiert die gemeinsame Eigenschaft `Beschreibung` und die abstrakte Methode `IstErfuellt(spiel : Spiel)`.

Die konkreten Bedingungstypen sind:

- **`RessourceProduzieren`**: erfüllt, wenn im Spiel mindestens die angegebene Gesamtmenge einer bestimmten Ressource produziert wurde. Dafür wird die kumulierte Produktion (`GesamtProduziert`) geprüft, nicht der aktuelle Bestand.
- **`DinosaurierKaufen`**: erfüllt, wenn im Spiel mindestens die angegebene Anzahl einer bestimmten Dinosaurierart gekauft wurde. Bei `Anzahl = 1` bedeutet das: mindestens ein Exemplar dieser Art muss gekauft worden sein. Die Dinosaurierart wird über die Assoziation `Dinosaurierart` referenziert.

Alle Bedingungen, die einem Ziel oder einem Freischaltungsobjekt zugeordnet sind, müssen erfüllt sein (UND-Verknüpfung).

## 3. Zentrale Modellierungsentscheidungen

- `Dinosaurierart`, `Karte` und `Ressource` sind Katalogklassen.
- `SpielRessource`, `SpielDinosaurier` und `Spielkarte` speichern den jeweiligen Zustand innerhalb einer konkreten Spielpartie.
- `SpielRessource` unterscheidet zwischen aktuellem `Bestand` und kumuliertem `GesamtProduziert`.
- `SpielDinosaurier` speichert die gekaufte Anzahl und den Freischaltungsstatus einer Dinosaurierart in einem Spiel.
- `Spielkarte` speichert Freischaltungs- und Kaufstatus. Eine Karte kann pro Spiel nur einmal gekauft werden.
- `Spiel` verwaltet den Spielablauf, die Produktion, die Prüfung von Freischaltungen und das Erreichen des Spielziels.
- Die Enum-Typen `Erdzeitalter`, `Ernaehrung`, `Lebensraum` und `Spielstatus` beschränken die jeweiligen Werte auf festgelegte Auswahlmöglichkeiten.

## 4. Identifikatoren

Die Katalog- und Hauptklassen besitzen eigene IDs. Für die spielbezogenen Zustandsklassen bleibt die ER-Modellierung mit zusammengesetzten Schlüsseln maßgeblich:

- `SpielRessource`: `SpielID` + `RessourceID`
- `SpielDinosaurier`: `SpielID` + `DinosaurierartID`
- `Spielkarte`: `SpielID` + `KarteID`

Diese zusammengesetzten Schlüssel werden nicht als zusätzliche fachliche Attribute aufgeführt, weil die Zugehörigkeit durch die Assoziationen dargestellt wird.

## 5. Vollständiger PlantUML-Quellcode

Der folgende Code kann als Datei mit der Endung `.puml` gespeichert und mit einer PlantUML-Erweiterung gerendert werden.

```plantuml


@startuml Dinosauria_Klassendiagramm
skinparam classAttributeIconSize 0
skinparam linetype ortho
hide empty members

enum Spielstatus {
  Neu
  Laufend
  Abgeschlossen
}
enum Erdzeitalter {
  Trias
  Jura
  Kreide
}
enum Ernaehrung {
  Pflanzenfresser
  Fleischfresser
  Allesfresser
}
enum Lebensraum {
  Land
  Wasser
  Luft
}
class Spielverwaltung {
  +SpielerListe : List<Spieler>
  --
  +ErstelleSpieler(name : string) : Spieler
  +FindeSpieler(spielerID : int) : Spieler?
  +HoleAlleSpieler() : List<Spieler>
  +LoescheSpieler(spielerID : int) : bool
  +BerechneRangliste() : List<Spieler>
}

class Spieler {
  +SpielerID : int
  +Name : string
  +ErstelltAm : DateTime
  --
  +ErstelleSpiel(spielziel : Spielziel) : Spiel
  +BerechneBesteAbschlusszeit() : TimeSpan?
  +HoleAbgeschlosseneSpiele() : List
}

class Spiel {
  +SpielID : int
  +Startzeitpunkt : DateTime
  +Endzeitpunkt : DateTime?
  +Abschlussdauer : TimeSpan?
  +Status : Spielstatus
  --
  +Starte() : void
  +Aktualisiere(deltaSekunden : decimal) : void
  +PruefeFreischaltungen() : void
  +PruefeSpielziel() : bool
  +Beende() : void
  +BerechneGesamtproduktionProSekunde() : decimal
}

class Spielziel {
  +SpielzielID : int
  +Name : string
  +Beschreibung : string
  --
  +IstErreicht(spiel : Spiel) : bool
}

class Ressource {
  +RessourceID : int
  +Name : string
  +Einheit : string
}

class SpielRessource {
  +Bestand : decimal
  +GesamtProduziert : decimal
  --
  +ErhoeheBestand(menge : decimal) : void
  +Verbrauche(menge : decimal) : bool
  +BucheProduktion(menge : decimal) : void
}

class Dinosaurierart {
  +DinosaurierartID : int
  +Name : string
  +Beschreibung : string
  +Kaufkosten : decimal
  +ProduktionProSekunde : decimal
  +Erdzeitalter : Erdzeitalter
  +Ernaehrung : Ernaehrung
  +Lebensraum : Lebensraum
  --
  +BerechneProduktion(anzahl : int, multiplikator : decimal) : decimal
}

class SpielDinosaurier {
  +Anzahl : int
  +IstFreigeschaltet : bool
  --
  +KannGekauftWerden() : bool
  +Kaufe() : bool
  +BerechneProduktionProSekunde() : decimal
}

class Karte {
  +KarteID : int
  +Name : string
  +Beschreibung : string
  +Kaufkosten : decimal
  +Multiplikator : decimal
  --
  +BerechneMultiplikator() : decimal
}

class Spielkarte {
  +IstFreigeschaltet : bool
  +IstGekauft : bool
  --
  +KannGekauftWerden() : bool
  +Kaufe() : bool
}

abstract class Aufgabenbedingung {
  +AufgabenbedingungID : int
  +Beschreibung : string
  --
  {abstract} +IstErfuellt(spiel : Spiel) : bool
}

class RessourceProduzieren {
  +Zielmenge : decimal
  --
  +IstErfuellt(spiel : Spiel) : bool
}

class DinosaurierKaufen {
  +Anzahl : int
  --
  +IstErfuellt(spiel : Spiel) : bool
}

Spieler "1" *-- "0..*" Spiel : Spielstaende
Spiel "1" --> "1" Spielziel : Ziel
Spiel "1" *-- "1..*" SpielRessource : Ressourcen
SpielRessource "*" --> "1" Ressource : Ressource
Spiel "1" *-- "1..*" SpielDinosaurier : Dinosaurier
SpielDinosaurier "*" --> "1" Dinosaurierart : Art
Spiel "1" *-- "1..*" Spielkarte : Karten
Spielkarte "*" --> "1" Karte : Karte
Dinosaurierart "*" --> "1" Ressource : Produktionsressource
Dinosaurierart "*" --> "1" Ressource : Kostenressource
Karte "*" --> "1" Ressource : Kostenressource
Karte "*" --> "1" Dinosaurierart : WirktAuf
Spielziel "1" *-- "1..*" Aufgabenbedingung : Zielbedingungen
Dinosaurierart "1" *-- "0..*" Aufgabenbedingung : Freischaltbedingungen
Karte "1" *-- "0..*" Aufgabenbedingung : Freischaltbedingungen
Aufgabenbedingung <|-- RessourceProduzieren
Aufgabenbedingung <|-- DinosaurierKaufen
DinosaurierKaufen --> "1" Dinosaurierart : Dinosaurierart
Spielziel ..> Spiel : prueft
Dinosaurierart --> Erdzeitalter
Dinosaurierart --> Ernaehrung
Dinosaurierart --> Lebensraum
Spiel --> Spielstatus
Spielverwaltung "1" *-- "0..*" Spieler : Spieler

@enduml
```
