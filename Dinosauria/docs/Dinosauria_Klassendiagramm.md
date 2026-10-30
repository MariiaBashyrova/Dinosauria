# Dinosauria – Klassendiagramm

## Zweck

Das Klassendiagramm beschreibt das fachliche Domänenmodell von **Dinosauria** für die Umsetzung in C# und Entity Framework Core. Es wird aus dem bestehenden ER-Modell abgeleitet; das ER-Modell selbst wird nicht verändert.

Das Diagramm zeigt Domänenklassen, Eigenschaften, Beziehungen und Navigationseigenschaften, zusammengesetzte Schlüssel sowie Enumerationen. Methoden, ViewModels, Services, Repositories und WPF-Views sind bewusst nicht enthalten.

## PlantUML

```plantuml
@startuml Dinosauria_Klassendiagramm
skinparam classAttributeIconSize 0
skinparam linetype ortho
skinparam shadowing false
hide empty methods


class Spieler {
  +SpielerID : int
  +Name : string
  +ErstelltAm : DateTime
  +Spiele : ICollection<Spiel>
  --
}

class Spiel {
  +SpielID : int
  +SpielerID : int
  +SpielzielID : int
  +Startzeitpunkt : DateTime
  +Endzeitpunkt : DateTime?
  +AbschlussdauerSekunden : int?
  +Status : string
  
  +Spieler : Spieler
  +Spielziel : Spielziel
  +SpielRessourcen : ICollection<SpielRessource>
  +SpielDinosaurier : ICollection<SpielDinosaurier>
  +Spielkarten : ICollection<Spielkarte>
  --
}

class Spielziel {
  +SpielzielID : int
  +Name : string
  +Beschreibung : string
  
  +Bedingungen : ICollection<Spielbedingung>
  --
}

class Ressource {
  +RessourceID : int
  +Name : string
  +Einheit : string
  
  --
}

class SpielRessource {
  +SpielID : int
  +RessourceID : int
  +Bestand : decimal
  +GesamtProduziert : decimal
  
  +Spiel : Spiel
  +Ressource : Ressource
  --
}

class Dinosaurierart {
  +DinosaurierartID : int
  +Name : string
  +Beschreibung : string
  +Kaufkosten : decimal
  +KostenRessourceID : int
  +ProduktionProSekunde : decimal
  +ProduktionsressourceID : int
  +Erdzeitalter : Erdzeitalter
  +Ernaehrung : Ernaehrung
  +Lebensraum : Lebensraum
  
  +KostenRessource : Ressource
  +Produktionsressource : Ressource
  
  +Karten : ICollection<Karte>
  +Dinosaurierbedingungen : ICollection<Dinosaurierbedingung>
  --
}

class SpielDinosaurier {
  +SpielID : int
  +DinosaurierartID : int
  +Anzahl : int
  +IstFreigeschaltet : bool
  
  +Spiel : Spiel
  +Dinosaurierart : Dinosaurierart
  --
}

class Karte {
  +KarteID : int
  +Name : string
  +Beschreibung : string
  +Kaufkosten : decimal
  +KostenRessourceID : int
  +Multiplikator : decimal
  +DinosaurierartID : int
  
  +KostenRessource : Ressource
  +Dinosaurierart : Dinosaurierart
  
  +Kartenbedingungen : ICollection<Kartenbedingung>
  --
}

class SpielKarte {
  +SpielID : int
  +KarteID : int
  +IstFreigeschaltet : bool
  +IstGekauft : bool
  
  +Spiel : Spiel
  +Karte : Karte
  --
}

abstract class Aufgabenbedingung {
  +AufgabenbedingungID : int
  +Beschreibung : string
  
  +Spielbedingungen : ICollection<Spielbedingung>
  +Dinosaurierbedingungen : ICollection<Dinosaurierbedingung>
  +Kartenbedingungen : ICollection<Kartenbedingung>
  --
}

class RessourceProduzieren {
 
  +RessourceID : int
  +Zielmenge : decimal
  
  +Ressource : Ressource
  --
}

class DinosaurierKaufen {
  
  +DinosaurierartID : int
  +Anzahl : int
  

  +Dinosaurierart : Dinosaurierart
  --
}

class Spielbedingung {
  +SpielzielID : int
  +AufgabenbedingungID : int
  
  +Spielziel : Spielziel
  +Aufgabenbedingung : Aufgabenbedingung
  --
}

class Dinosaurierbedingung {
  +DinosaurierartID : int
  +AufgabenbedingungID : int
  
  +Dinosaurierart : Dinosaurierart
  +Aufgabenbedingung : Aufgabenbedingung
  --
}

class Kartenbedingung {
  +KarteID : int
  +AufgabenbedingungID : int
  
  +Karte : Karte
  +Aufgabenbedingung : Aufgabenbedingung
  --
}

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

Spieler "1" -- "0..*" Spiel : besitzt
Spielziel "1" -- "0..*" Spiel : wird_verwendet
Spiel "1" *-- "0..*" SpielRessource : hat
Ressource "1" -- "0..*" SpielRessource : Ressourcentyp
Spiel "1" *-- "0..*" SpielDinosaurier : enthaelt
Dinosaurierart "1" -- "0..*" SpielDinosaurier : Typ
Ressource "1" -- "0..*" Dinosaurierart : Kaufressource
Ressource "1" -- "0..*" Dinosaurierart : Produktionsressource
Spiel "1" *-- "0..*" SpielKarte : enthaelt
Karte "1" -- "0..*" SpielKarte : Kartenstatus
Ressource "1" -- "0..*" Karte : Kaufressource
Dinosaurierart "1" -- "0..*" Karte : wirkt_auf
Aufgabenbedingung <|-- RessourceProduzieren
Aufgabenbedingung <|-- DinosaurierKaufen
Ressource "1" -- "0..*" RessourceProduzieren : Zielressource
Dinosaurierart "1" -- "0..*" DinosaurierKaufen : Zieldino
Spielziel "1" -- "0..*" Spielbedingung
Aufgabenbedingung "1" -- "0..*" Spielbedingung
Dinosaurierart "1" -- "0..*" Dinosaurierbedingung
Aufgabenbedingung "1" -- "0..*" Dinosaurierbedingung
Karte "1" -- "0..*" Kartenbedingung
Aufgabenbedingung "1" -- "0..*" Kartenbedingung
Dinosaurierart ..> Erdzeitalter
Dinosaurierart ..> Ernaehrung
Dinosaurierart ..> Lebensraum

note right of SpielRessource
  Bestand sinkt bei Kaeufen.
  GesamtProduziert steigt nur durch Produktion.
end note
note bottom of SpielDinosaurier
  Zusammengesetzter Schluessel: SpielID + DinosaurierartID.
  Ein Datensatz je Spiel und Dinosaurierart.
end note
note bottom of Spielkarte
  Zusammengesetzter Schluessel: SpielID + KarteID.
  Eine Karte kann pro Spiel nur einmal gekauft werden.
  IstGekauft setzt IstFreigeschaltet voraus.
end note
note bottom of Aufgabenbedingung
  Abstrakte Basisklasse fuer konkrete Bedingungstypen.
  Alle zugeordneten Bedingungen muessen erfuellt sein (UND).
end note
@enduml
```

## Hinweise zur Umsetzung

- `SpielRessource`, `SpielDinosaurier` und `Spielkarte` sind Zuordnungsklassen mit zusammengesetzten Schlüsseln.
- Die Beziehungen zu `Ressource` und `Dinosaurierart` sind teilweise mehrfach vorhanden. In C# müssen sie durch eindeutig benannte Navigationseigenschaften unterschieden werden.
- `Aufgabenbedingung` ist abstrakt. `RessourceProduzieren` bildet den konkreten Bedingungstyp ab, der im ER-Modell vorgesehen ist.
- Collections sind Navigationseigenschaften für EF Core. Sie ermöglichen den Zugriff auf verknüpfte Objekte.
- `DateTime`, `DateTime?`, `decimal`, `bool` und `ICollection<T>` sind C#-Typen zur Darstellung des Domänenmodells.
- Das Diagramm enthält bewusst keine Methoden. Die Spiellogik gehört später in passende Services beziehungsweise ViewModels.
