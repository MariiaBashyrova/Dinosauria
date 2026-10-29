# Dinosauria
## Wireframe-Entwurf der Benutzeroberfläche

**Entwurf für die erste Version – 29.10.2026**

## 1. Ziel des Wireframes

Der Wireframe stellt die geplante Anordnung der wichtigsten Elemente der Benutzeroberfläche des Spiels Dinosauria dar. Er dient als Grundlage für die spätere Umsetzung mit WPF und XAML. Der Fokus liegt auf einer einfachen, übersichtlichen Oberfläche ohne aufwendige Gestaltung.

## 2. Hauptmenü

Im Hauptmenü kann ein Spieler ausgewählt oder neu angelegt werden. Außerdem werden die globale Rangliste und die gespeicherten Spiele des ausgewählten Spielers angezeigt.

![Wireframe des Hauptmenüs](a_clean_ui_mockup_wireframe_style_screenshot_of.png)

*Abbildung 1: Wireframe des Hauptmenüs*

### 2.1 Spieler auswählen und verwalten

- Eine Liste zeigt alle vorhandenen Spieler.
- Über **„Neuer Spieler“** kann ein neuer Spieler angelegt werden.
- **„Umbenennen“** ändert den Namen des ausgewählten Spielers.
- **„Spieler löschen“** entfernt den ausgewählten Spieler nach einer Bestätigung einschließlich seiner gespeicherten Spiele.

### 2.2 Rangliste

Die globale Rangliste zeigt pro Spieler die beste Abschlusszeit und die Anzahl der abgeschlossenen Spiele. Maßgeblich ist die kürzeste Zeit, in der der Spieler das Spielziel erreicht hat. Spieler ohne abgeschlossene Spiele werden nicht in die Rangliste aufgenommen.

### 2.3 Spiele des ausgewählten Spielers

Die Tabelle zeigt die gespeicherten Spiele des oben ausgewählten Spielers. Angezeigt werden Spielnummer, Startdatum, Spielzeit, Gesamtproduktion der Fossilien, freigeschaltete Dinosaurier und Status. Laufende Spiele können fortgesetzt werden; abgeschlossene Spiele dienen der Ergebnisansicht.

Die Schaltflächen im unteren Bereich ermöglichen das Starten eines neuen Spiels, das Fortsetzen des ausgewählten laufenden Spiels, das Löschen eines ausgewählten Spielstands und das Beenden der Anwendung.

## 3. Spielübersicht

Die Spielübersicht besteht aus einer Kopfzeile, dem Spielziel, dem Spielfeld, der Dinosaurier-Tabelle und einer Statuszeile. Eine linke Seitenleiste ist nicht vorgesehen; weitere Funktionen sind über das Hauptmenü erreichbar.

![Wireframe der Spielübersicht](wide_clean_ui_mockup_in_flat_minimal_style_for_a.png)

*Abbildung 2: Wireframe der Spielübersicht*

### 3.1 Kopfzeile und Spielziel

- Die Kopfzeile zeigt den Spielnamen, den Spielernamen, den aktuellen Fossilienbestand und die Produktion pro Sekunde.
- Das Spielziel und der Fortschritt werden im oberen Bereich angezeigt.
- Die Schaltfläche **„Hauptmenü“** führt zurück zum Hauptmenü.

### 3.2 Spielfeld

- Das Spielfeld ist eine gemeinsame, bei Bedarf scrollbar ausgeführte Fläche.
- Dinosaurier werden als Kreise und Karten als Rechtecke dargestellt. Es werden zunächst nur einfache geometrische Formen, Text und gerade Verbindungslinien verwendet.
- Jede Karte ist genau einer Dinosaurierart zugeordnet und wird durch eine direkte Linie mit dem zugehörigen Dinosaurier-Kreis verbunden. Karten sind keine Zwischenknoten und werden nicht miteinander verbunden.
- Gekaufte Dinosaurier und Karten werden grün dargestellt.
- Freigeschaltete, aber noch nicht gekaufte Elemente werden blau und optisch zurückhaltend dargestellt.
- Gesperrte Elemente werden grau und ohne Namen oder weitere Informationen angezeigt.

### 3.3 Dinosaurier-Tabelle

Unter dem Spielfeld befindet sich eine Tabelle. Sie zeigt nur gekaufte Dinosaurierarten sowie freigeschaltete Arten, die gekauft werden können.

Spalten: **Name**, **Anzahl**, **aktuelle Produktion pro Sekunde**, **Produktionszuwachs beim Kauf eines weiteren Dinosauriers**, **Kosten des nächsten Exemplars** und die Schaltfläche **„Kaufen“**.

Bei einer noch nicht gekauften Art beträgt die aktuelle Produktion null. Der angezeigte Zuwachs berücksichtigt die aktiven Kartenmultiplikatoren. Der Preis jedes weiteren Exemplars steigt um 15 % gegenüber dem Preis des vorherigen Exemplars.

### 3.4 Statuszeile

Am unteren Rand werden die bisherige Spielzeit und die insgesamt produzierten Fossilien angezeigt. Die Gesamtproduktion ist ein kumulierter Wert und wird durch Käufe nicht reduziert.

## 4. Einheitliche Gestaltungsregeln

- Einfache Standard-Steuerelemente ohne dekorative Rahmen oder aufwendige Effekte.
- Keine Bilder im ersten Wireframe; Dinosaurier und Karten werden durch Kreise und Rechtecke repräsentiert.
- Klare Beschriftungen und geradlinige Verbindungen auf dem Spielfeld.
- Die Oberfläche wird mit WPF/XAML umgesetzt.

## 5. Hinweise zur Umsetzung

Die Abbildungen sind schematische Entwürfe. Beispielnamen, Zahlenwerte und Spielstände dienen nur der Veranschaulichung und sind keine verbindlichen Spieldaten. Die endgültigen Abstände und Größen werden bei der Umsetzung in WPF angepasst.

**Hinweis zu den Abbildungen:** Die Bilddateien müssen zusammen mit dieser Markdown-Datei im selben Ordner abgelegt werden, damit die eingebetteten Abbildungen angezeigt werden.
