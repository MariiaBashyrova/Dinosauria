# Dinosauria

**Dinosauria** ist ein inkrementelles Aufbauspiel als WPF-Desktop-Anwendung. Spieler kaufen Dinosaurier, sammeln Fossilien durch automatische Ressourcenproduktion und schalten weitere Dinosaurier sowie Karten frei. Karten erhöhen die Produktion eines bestimmten Dinosauriertyps.

## Funktionen

- Spieler und mehrere unabhängige Spielrunden verwalten
- Spielstände speichern und fortsetzen
- Dinosaurier kaufen und automatische Fossilienproduktion
- Dinosaurier und Karten über Aufgabenbedingungen freischalten
- Karten kaufen und Produktionsmultiplikatoren anwenden
- Spielziele erfüllen und abgeschlossene Spiele in Ranglisten vergleichen

## Technologien

- C# / .NET 9
- WPF / XAML und MVVM
- Entity Framework Core 9
- SQL Server LocalDB
- xUnit

## Projektstruktur

| Projekt | Aufgabe |
|---|---|
| `Dinosauria.Domain` | Domänenmodelle und Repository-Schnittstellen |
| `Dinosauria.Application` | Anwendungslogik (`SpielService`) |
| `Dinosauria.Infrastructure` | Datenbankzugriff, Repositories und CSV-Import |
| `Dinosauria.ViewModels` | ViewModels und Präsentationslogik |
| `Dinosauria.WPF` | Grafische Oberfläche und Anwendungsstart |
| `Dinosauria.Tests` | Automatisierte Repository- und Datenbanktests |

## Start

Die Solution in Visual Studio öffnen und `Dinosauria.WPF` als Startprojekt auswählen. Für die Datenhaltung wird SQL Server LocalDB verwendet. Die initialen Spiel- und Konfigurationsdaten werden aus CSV-Dateien eingelesen.

## Dokumentation

Die ausführliche Projektdokumentation befindet sich in `Dinosauria_Dokumentation.md`.
