# ER-Modell – Dinosauria

## Modellierungsnotizen

Die ER-Darstellung verwendet eine tabellenorientierte Notation mit Kardinalitäten (Crow’s Foot), keine Chen-Notation.

### Enumerationen

Die folgenden Attribute der Entität `Dinosaurierart` werden als Enumerationen modelliert und deshalb nicht als separate Entitäten dargestellt:

| Attribut | Enumeration | Zulässige Werte |
|---|---|---|
| `Erdzeitalter` | `Erdzeitalter` | `Trias`, `Jura`, `Kreide` |
| `Ernaehrung` | `Ernaehrung` | `Pflanzenfresser`, `Fleischfresser`, `Allesfresser` |
| `Lebensraum` | `Lebensraum` | `Land`, `Wasser`, `Luft` |

### Zentrale Modellentscheidungen

- `SpielRessource` speichert den aktuellen Bestand (`Bestand`) und die insgesamt produzierte Menge (`GesamtProduziert`) getrennt.
- `SpielDinosaurier` enthält pro Spiel und Dinosaurierart genau einen Datensatz. Der zusammengesetzte Primärschlüssel besteht aus `SpielID` und `DinosaurierartID`. `Anzahl` speichert die gekaufte Anzahl; `IstFreigeschaltet` speichert den Freischaltungsstatus.
- `Spielkarte` enthält pro Spiel und Karte genau einen Datensatz. Der zusammengesetzte Primärschlüssel besteht aus `SpielID` und `KarteID`. `IstFreigeschaltet` und `IstGekauft` speichern den Zustand der Karte. Eine Karte kann pro Spiel nur einmal gekauft werden.
- `Aufgabenbedingung` ist die gemeinsame abstrakte Grundlage für konkrete Bedingungstypen. `RessourceProduzieren` ist ein konkreter Bedingungstyp.
- `Spielbedingung`, `Dinosaurierbedingung` und `Kartenbedingung` verbinden Bedingungen mit Spielzielen, Dinosaurierarten beziehungsweise Karten. Alle zugeordneten Bedingungen müssen erfüllt sein (UND-Verknüpfung).
- Ein Spiel wird mit Datensätzen für alle Dinosaurierarten und alle Karten initialisiert. Der Freischaltungsstatus wird je Spiel gespeichert.
- Die Abschlussdauer wird für abgeschlossene Spiele gespeichert. Die globale Rangliste verwendet pro Spieler die beste Abschlusszeit; unvollständige Spiele werden nicht gewertet.

## Entitäten und wichtige Attribute

- **Spieler**: `SpielerID` (PK), `Name`
- **Spiel**: `SpielID` (PK), `SpielerID` (FK), `Startzeitpunkt`, `Endzeitpunkt`, `AbschlussdauerSekunden`, `Status`
- **Spielziel**: `SpielzielID` (PK), `SpielID` (FK), `Bezeichnung`
- **Ressource**: `RessourceID` (PK), `Name`
- **SpielRessource**: `SpielID` (PK, FK), `RessourceID` (PK, FK), `Bestand`, `GesamtProduziert`
- **Dinosaurierart**: `DinosaurierartID` (PK), `Name`, `Beschreibung`, `Kaufkosten`, `KaufressourceID` (FK), `ProduktionProSekunde`, `ProduktionsressourceID` (FK), `Erdzeitalter`, `Ernaehrung`, `Lebensraum`
- **SpielDinosaurier**: `SpielID` (PK, FK), `DinosaurierartID` (PK, FK), `Anzahl`, `IstFreigeschaltet`
- **Karte**: `KarteID` (PK), `Name`, `Beschreibung`, `Kaufkosten`, `KaufressourceID` (FK), `Multiplikator`, `DinosaurierartID` (FK)
- **Spielkarte**: `SpielID` (PK, FK), `KarteID` (PK, FK), `IstFreigeschaltet`, `IstGekauft`
- **Aufgabenbedingung**: `AufgabenbedingungID` (PK), `Bezeichnung`
- **RessourceProduzieren**: `AufgabenbedingungID` (PK, FK), `RessourceID` (FK), `Zielmenge`
- **Spielbedingung**: `SpielzielID` (PK, FK), `AufgabenbedingungID` (PK, FK)
- **Dinosaurierbedingung**: `DinosaurierartID` (PK, FK), `AufgabenbedingungID` (PK, FK)
- **Kartenbedingung**: `KarteID` (PK, FK), `AufgabenbedingungID` (PK, FK)

## Hinweis

Die genaue Umsetzung der Enumerationen in der Datenbank hängt von der EF-Core-Konfiguration ab. Ohne besondere Konfiguration speichert EF Core Enum-Werte standardmäßig als Ganzzahlen (`int`).

## PlantUML-Quelltext

Der folgende Quelltext erzeugt das ER-Modell. Er kann in eine `.puml`-Datei kopiert oder direkt in PlantUML verwendet werden.

```plantuml
@startuml Dinosauria_ER_Modell
' ER-Modell in relationaler Notation mit Crow's-Foot-Kardinalitaeten
' Kein Chen-Modell: Entitaeten als Tabellen mit PK/FK-Attributen.

hide circle
skinparam linetype ortho
skinparam shadowing false
skinparam entity {
  BackgroundColor #F8F9FA
  BorderColor #495057
  FontColor #212529
}
skinparam note {
  BackgroundColor #FFF8DB
  BorderColor #B08900
}

entity "Spieler" as Spieler {
  * SpielerID : int <<PK>>
  --
  Name : string
  ErstelltAm : datetime
}

entity "Spiel" as Spiel {
  * SpielID : int <<PK>>
  --
  SpielerID : int <<FK>>
  SpielzielID : int <<FK>>
  Startzeitpunkt : datetime
  Endzeitpunkt : datetime?
  AbschlussdauerSekunden : int?
  Status : string
}

entity "Spielziel" as Spielziel {
  * SpielzielID : int <<PK>>
  --
  Name : string
  Beschreibung : string
}

entity "Ressource" as Ressource {
  * RessourceID : int <<PK>>
  --
  Name : string
  Einheit : string
}

entity "SpielRessource" as SpielRessource {
  * SpielID : int <<PK, FK>>
  * RessourceID : int <<PK, FK>>
  --
  Bestand : decimal
  GesamtProduziert : decimal
}

entity "Dinosaurierart" as Dinosaurierart {
  * DinosaurierartID : int <<PK>>
  --
  Name : string
  Beschreibung : string
  Kaufkosten : decimal
  KostenRessourceID : int <<FK>>
  ProduktionProSekunde : decimal
  ProduktionsRessourceID : int <<FK>>
  Erdzeitalter : string
  Ernaehrung : string
  Lebensraum : string
}

entity "SpielDinosaurier" as SpielDinosaurier {
  * SpielID : int <<PK, FK>>
  * DinosaurierartID : int <<PK, FK>>
  --
  Anzahl : int
  IstFreigeschaltet : bool
}

entity "Karte" as Karte {
  * KarteID : int <<PK>>
  --
  Name : string
  Beschreibung : string
  Kaufkosten : decimal
  KostenRessourceID : int <<FK>>
  DinosaurierartID : int <<FK>>
  Multiplikator : decimal
}

entity "Spielkarte" as Spielkarte {
  * SpielID : int <<PK, FK>>
  * KarteID : int <<PK, FK>>
  --
  IstFreigeschaltet : bool
  IstGekauft : bool
}

entity "Aufgabenbedingung" as Aufgabenbedingung {
  * AufgabenbedingungID : int <<PK>>
  --
  Typ : string
  Beschreibung : string
}

entity "RessourceProduzieren" as RessourceProduzieren {
  * AufgabenbedingungID : int <<PK, FK>>
  --
  RessourceID : int <<FK>>
  Zielmenge : decimal
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

' Spieler und Spielverlaeufe
Spieler ||--o{ Spiel : besitzt
Spielziel ||--o{ Spiel : wird_verwendet

' Ressourcenbestand und kumulierte Produktion je Spiel
Spiel ||--o{ SpielRessource : hat
Ressource ||--o{ SpielRessource : wird_verfolgt

' Dinosaurierkatalog und spielbezogener Zustand
Spiel ||--o{ SpielDinosaurier : enthaelt
Dinosaurierart ||--o{ SpielDinosaurier : Typ
Ressource ||--o{ Dinosaurierart : Kaufkosten
Ressource ||--o{ Dinosaurierart : Produktionsressource

' Kartenkatalog und spielbezogener Zustand
Spiel ||--o{ Spielkarte : enthaelt
Karte ||--o{ Spielkarte : Karte
Ressource ||--o{ Karte : Kaufkosten
Dinosaurierart ||--o{ Karte : wirkt_auf

' Bedingungstypen und Zuordnungen
Aufgabenbedingung ||--o| RessourceProduzieren : Spezialisierung
Ressource ||--o{ RessourceProduzieren : Zielressource
Spielziel ||--o{ Spielbedingung
Aufgabenbedingung ||--o{ Spielbedingung
Dinosaurierart ||--o{ Dinosaurierbedingung
Aufgabenbedingung ||--o{ Dinosaurierbedingung
Karte ||--o{ Kartenbedingung
Aufgabenbedingung ||--o{ Kartenbedingung

note right of Spiel
  Status z. B. "Laufend" oder "Abgeschlossen".
  AbschlussdauerSekunden wird beim Abschluss
  dauerhaft gespeichert und fuer Rankings genutzt.
end note

note right of SpielRessource
  Bestand sinkt bei Kaeufen.
  GesamtProduziert steigt durch Produktion
  und wird durch Kaeufe nicht reduziert.
end note

note bottom of SpielDinosaurier
  Beim Anlegen eines Spiels wird fuer jede
  Dinosaurierart ein Datensatz erzeugt.
  Anzahl = 0 ist zulaessig.
end note

note bottom of Spielkarte
  Beim Anlegen eines Spiels wird fuer jede
  Karte ein Datensatz erzeugt.
  Regel: IstGekauft = true => IstFreigeschaltet = true.
  Jede Karte ist pro Spiel nur einmal kaufbar.
end note

note bottom of Aufgabenbedingung
  Alle zugeordneten Bedingungen muessen erfuellt sein
  (UND-Verknuepfung, keine ODER-Logik).
end note

@enduml
```
