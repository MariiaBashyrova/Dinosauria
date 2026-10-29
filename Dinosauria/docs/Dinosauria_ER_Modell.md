# Dinosauria – ER-Modell

## 1. Zweck

Das ER-Modell beschreibt die relationalen Daten, die für die erste Version von **Dinosauria** benötigt werden. Es berücksichtigt Spieler, mehrere Spielstände pro Spieler, Dinosaurier und Karten, Ressourcen, Freischaltbedingungen, Spielziele sowie abgeschlossene Spiele und Ranglisten.

Die Darstellung verwendet PlantUML mit der Chen-ähnlichen Entity-Relationship-Notation. Primärschlüssel sind mit `PK`, Fremdschlüssel mit `FK` gekennzeichnet.

## 2. ER-Diagramm

```plantuml
@startuml Dinosauria_ER_Modell
title Dinosauria – Entity-Relationship-Modell
hide circle
skinparam linetype ortho
skinparam shadowing false
skinparam entity {
  BackgroundColor #F8F8F8
  BorderColor #555555
}

entity "Spieler" as Spieler {
  * SpielerID : int <<PK>>
  --
  Name : varchar
}

entity "Spiel" as Spiel {
  * SpielID : int <<PK>>
  --
  SpielerID : int <<FK>>
  Startzeit : datetime
  Endzeit : datetime [NULL]
  Status : varchar
  Gesamtproduktion : decimal
  AktuellerRessourcenbestand : decimal
}

entity "Spielziel" as Spielziel {
  * SpielzielID : int <<PK>>
  --
  SpielID : int <<FK>>
  Zielmenge : decimal
  RessourcenID : int <<FK>>
}

entity "Ressource" as Ressource {
  * RessourcenID : int <<PK>>
  --
  Name : varchar
  Beschreibung : varchar
}

entity "Dinosaurierart" as Dinosaurierart {
  * DinosaurierartID : int <<PK>>
  --
  Name : varchar
  Beschreibung : varchar
  Erdzeitalter : varchar
  Ernaehrung : varchar
  Lebensraum : varchar
  Kaufkosten : decimal
  KostenRessourcenID : int <<FK>>
  Basisproduktion : decimal
  ProduktionsRessourcenID : int <<FK>>
}

entity "SpielDinosaurier" as SpielDinosaurier {
  * SpielID : int <<PK, FK>>
  * DinosaurierartID : int <<PK, FK>>
  --
  Anzahl : int
}

entity "Karte" as Karte {
  * KarteID : int <<PK>>
  --
  Name : varchar
  Beschreibung : varchar
  Kaufkosten : decimal
  KostenRessourcenID : int <<FK>>
  Multiplikator : decimal
  DinosaurierartID : int <<FK>>
}

entity "Spielkarte" as Spielkarte {
  * SpielID : int <<PK, FK>>
  * KarteID : int <<PK, FK>>
  --
  Kaufzeitpunkt : datetime
}

entity "Aufgabenbedingung" as Aufgabenbedingung {
  * AufgabenbedingungID : int <<PK>>
  --
  Typ : varchar
  Zielwert : decimal
  RessourcenID : int <<FK, NULL>>
}

entity "Spielbedingung" as Spielbedingung {
  * SpielzielID : int <<PK, FK>>
  * AufgabenbedingungID : int <<PK, FK>>
}

entity "Dinosaurierbedingung" as Dinosaurierbedingung {
  * DinosaurierartID : int <<PK, FK>>
  * AufgabenbedingungID : int <<PK, FK>>
}

entity "Kartenbedingung" as Kartenbedingung {
  * KarteID : int <<PK, FK>>
  * AufgabenbedingungID : int <<PK, FK>>
}

Spieler ||--o{ Spiel : "besitzt"
Spiel ||--|| Spielziel : "hat"
Ressource ||--o{ Spielziel : "wird benoetigt fuer"

Spiel ||--o{ SpielDinosaurier : "enthaelt"
Dinosaurierart ||--o{ SpielDinosaurier : "wird gekauft als"

Spiel ||--o{ Spielkarte : "enthaelt"
Karte ||--o{ Spielkarte : "wird gekauft als"

Ressource ||--o{ Dinosaurierart : "Kaufkosten"
Ressource ||--o{ Dinosaurierart : "Produktionsressource"
Ressource ||--o{ Karte : "Kaufkosten"
Dinosaurierart ||--o{ Karte : "wird beeinflusst durch"
Ressource ||--o{ Aufgabenbedingung : "kann Bedingung betreffen"

Spielziel ||--o{ Spielbedingung
Aufgabenbedingung ||--o{ Spielbedingung

Dinosaurierart ||--o{ Dinosaurierbedingung
Aufgabenbedingung ||--o{ Dinosaurierbedingung

Karte ||--o{ Kartenbedingung
Aufgabenbedingung ||--o{ Kartenbedingung

note right of Aufgabenbedingung
  Typ beschreibt die konkrete Bedingungsart,
  z. B. RessourceProduzieren.
  Alle verknuepften Bedingungen muessen
  erfuellt sein (UND-Verknuepfung).
end note

note bottom of Spiel
  Endzeit ist bei laufenden Spielen NULL.
  Abgeschlossene Spiele werden fuer
  die persoenliche Historie verwendet.
end note

note bottom of Spielkarte
  Eine Karte kann pro Spiel nur einmal
  gekauft werden. Multiplikatoren wirken
  multiplikativ auf die Basisproduktion.
end note

@enduml
```

## 3. Wichtige Modellierungsentscheidungen

- **Spieler und Spiel:** Ein Spieler kann mehrere unabhängige Spiele speichern. Jedes Spiel gehört genau einem Spieler.
- **Spielziel:** Für Version 1 hat jedes Spiel genau ein Ziel. Das Ziel zählt die insgesamt produzierte Menge einer Ressource, nicht nur den aktuellen Bestand.
- **Ressourcen:** Das Modell unterstützt mehrere Ressourcen, obwohl Version 1 zunächst nur eine Ressource verwendet.
- **Dinosaurier:** `Dinosaurierart` beschreibt den gemeinsamen Katalog. `SpielDinosaurier` speichert den Besitz und die Anzahl pro Spiel.
- **Karten:** `Karte` beschreibt den Kartenkatalog. `Spielkarte` speichert gekaufte Karten pro Spiel. Der zusammengesetzte Schlüssel verhindert, dass dieselbe Karte innerhalb eines Spiels mehrfach gekauft wird.
- **Kartenwirkung:** Jede Karte beeinflusst in Version 1 eine Dinosaurierart. Mehrere Kartenmultiplikatoren werden miteinander multipliziert.
- **Aufgabenbedingungen:** `Aufgabenbedingung` ist eine gemeinsame abstrakte fachliche Idee. Die Zuordnungstabellen `Spielbedingung`, `Dinosaurierbedingung` und `Kartenbedingung` verbinden Bedingungen mit Spielzielen beziehungsweise Freischaltobjekten. Alle zugeordneten Bedingungen müssen erfüllt sein.
- **Rangliste:** Eine eigene Ranglisten-Tabelle ist nicht erforderlich. Sie kann aus abgeschlossenen Spielen abgeleitet werden: persönliche Bestzeit je Spieler und anschließend globale Sortierung nach der kürzesten Zeit.
- **Admin-Funktionen:** Sie sind nicht Teil von Version 1 und werden daher hier nicht modelliert.

## 4. Hinweise zur Umsetzung

Das Diagramm ist ein fachlicher Entwurf und noch kein endgültiges Datenbankschema. Vor der Implementierung sollten insbesondere folgende Punkte geprüft werden:

1. Ob `Status` als Text oder als festgelegter Wertebereich modelliert wird.
2. Ob `Gesamtproduktion` und `AktuellerRessourcenbestand` in Version 1 als getrennte Werte gespeichert oder teilweise aus Ereignissen berechnet werden.
3. Ob Bedingungen künftig unterschiedliche Daten benötigen. Falls ja, kann die gemeinsame Tabelle um typabhängige Detailtabellen ergänzt werden.
4. Ob ein Spielziel im späteren Projekt mehrere Ressourcen oder mehrere Zielkriterien unterstützen soll.

