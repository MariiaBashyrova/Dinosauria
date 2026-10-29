# Projektbeschreibung: Dinosauria

## 1. Projektziel

**Dinosauria** ist ein inkrementelles Aufbauspiel, in dem Spieler
Dinosaurier erwerben, Ressourcen produzieren und weitere Dinosaurier
sowie Karten freischalten. Ziel einer Spielrunde ist es, alle
vorgegebenen Spielbedingungen zu erfüllen und dadurch das Spielziel zu
erreichen.

Die benötigte Zeit bis zum Erreichen des Spielziels ist das Ergebnis der
Spielrunde. Nach erfolgreichem Abschluss wird die Runde beendet und das
Ergebnis dauerhaft gespeichert. Die Spieler können ihre eigenen
abgeschlossenen Spielrunden vergleichen; zusätzlich gibt es eine
Rangliste der Spieler auf Grundlage ihrer jeweils besten Zeit.

Das Projekt dient zugleich dazu, objektorientierte Programmierung und
die Entwicklung einer Desktop-Anwendung mit WPF praktisch umzusetzen.

## 2. Technologien

-   **C#** als Programmiersprache
-   **WPF** für die grafische Benutzeroberfläche
-   **Entity Framework Core (EF Core)** für den Datenzugriff und die
    Abbildung der Anwendungsobjekte auf die Datenbank
-   **SQL-Datenbank** zur dauerhaften Speicherung von Spielern,
    Spielrunden, Spielständen, Dinosauriern, Karten und Ergebnissen
-   **PlantUML** für UML- und Datenbankdiagramme in einer späteren
    Projektphase
-   **Markdown** für die Projektdokumentation

Die Oberfläche bleibt in der ersten Version bewusst einfach. Der
Schwerpunkt liegt auf der Spiellogik, der Datenhaltung und einer klaren
objektorientierten Struktur.

## 3. Spielablauf und fachliche Regeln

1.  Beim Start kann der Benutzer einen vorhandenen Spieler auswählen
    oder einen neuen Spieler ohne Passwort anlegen.
2.  Ein Spieler kann mehrere unabhängige Spielrunden anlegen und
    gespeicherte Runden fortsetzen.
3.  Zu Beginn einer Spielrunde wird ein Spielziel festgelegt.
4.  Der Spieler verwendet vorhandene Ressourcen, um Dinosaurier und
    Karten zu erwerben.
5.  Die erworbenen Dinosaurier produzieren automatisch Ressourcen,
    solange die Spielrunde läuft.
6.  Weitere Dinosaurier und Karten werden freigeschaltet, sobald ihre
    jeweiligen Freischaltbedingungen erfüllt sind.
7.  Sowohl ein Spielziel als auch die Freischaltung eines Dinosauriers
    oder einer Karte können von mehreren Bedingungen abhängen.
8.  **Alle** einer Zieldefinition zugeordneten Bedingungen müssen
    erfüllt sein. Es gilt eine UND-Verknüpfung; eine ODER-Verknüpfung
    ist für die erste Version nicht vorgesehen.
9.  Das erste Spielziel besteht darin, eine festgelegte Gesamtmenge
    einer Ressource zu produzieren. Es geht ausdrücklich nicht darum,
    diese Menge gleichzeitig im Ressourcenbestand zu besitzen.
10. Ausgaben für Dinosaurier und Karten verringern den aktuellen
    Ressourcenbestand, aber nicht die bereits insgesamt produzierte
    Ressourcenmenge.
11. Karten sind kaufbare Spielelemente. Eine Karte wird nach Erfüllung
    ihrer Freischaltbedingungen verfügbar und kann Ressourcen kosten.
    Ihre Wirkung besteht darin, die Produktionsrate eines bestimmten
    Generators bzw. Dinosauriertyps um einen festgelegten Faktor zu
    erhöhen.
12. Sobald alle Bedingungen des Spielziels erfüllt sind, wird die
    Spielrunde beendet. Die bis dahin benötigte Spielzeit ist das
    Ergebnis.
13. Abgeschlossene Ergebnisse werden gespeichert und können später in
    Ranglisten angezeigt.

## 4. Speichern und Ranglisten

Der Spielstand wird in einer SQL-Datenbank gespeichert. Der Zugriff
erfolgt über Entity Framework Core.

Eine Spielrunde kann gespeichert und später fortgesetzt werden.
Abgeschlossene Spielrunden werden als endgültige Ergebnisse aufbewahrt.
Das Ergebnis enthält insbesondere die benötigte Spielzeit.

Es gibt zwei Ansichten für Ergebnisse:

-   **Spielerrangliste:** Jeder Spieler wird anhand seiner besten
    (kürzesten) Zeit über alle abgeschlossenen Spielrunden eingeordnet.
-   **Eigene Spielrunden:** Ein Spieler kann seine abgeschlossenen
    Runden sehen, sortiert nach der benötigten Zeit.

Unvollständige Spielrunden werden nicht in die Ranglisten aufgenommen.
Die Ranglisten können aus den gespeicherten Ergebnissen ermittelt
werden; eine separate Ranglisten-Tabelle ist dafür zunächst nicht
erforderlich.

Eine Offline-Ressourcenproduktion während das Spiel geschlossen ist,
gehört nicht zum Umfang der ersten Version.

## 5. Spielziele und Bedingungen

Spielziele und Freischaltbedingungen verwenden dasselbe abstrakte
fachliche Konzept: **Aufgabenbedingung**. Konkrete Bedingungstypen
können davon abgeleitet und im Laufe der Weiterentwicklung ergänzt
werden.

Die Zuordnung erfolgt über unterschiedliche Beziehungen:

-   **Spielbedingung:** Verknüpft eine Spielrunde mit den Bedingungen,
    die für das Erreichen ihres Spielziels erfüllt sein müssen.
-   **Dinosaurierbedingung:** Verknüpft einen Dinosaurier mit den
    Bedingungen, die für seine Freischaltung erfüllt sein müssen.
-   **Kartenbedingung:** Verknüpft eine Karte mit den Bedingungen, die
    für ihre Freischaltung erfüllt sein müssen.

Für alle drei Beziehungen gilt: Alle zugeordneten Bedingungen müssen
erfüllt sein. In der ersten Version wird als konkreter Bedingungstyp
mindestens die Produktion einer festgelegten Ressourcenmenge benötigt.
Weitere Typen, etwa der Besitz einer bestimmten Anzahl von Dinosauriern,
können später ergänzt werden.

## 6. Zentrale Entitäten und ihre Beziehungen

### Spieler

Repräsentiert einen Benutzer des Spiels. Ein Spieler kann mehrere
Spielrunden erstellen, speichern und fortsetzen. Seine abgeschlossenen
Runden bilden die Grundlage für seine persönliche Ergebnisübersicht und
für seinen Eintrag in der Spielerrangliste.

### Spiel

Repräsentiert eine einzelne, unabhängige Spielrunde. Sie verwaltet den
laufenden Spielzustand, verwendet ein Spielziel und steht mit den in
dieser Runde vorhandenen Ressourcen sowie den erworbenen Dinosauriern
und Karten in Beziehung. Nach Erfüllung des Spielziels wird die Runde
abgeschlossen und ihre benötigte Spielzeit als Ergebnis festgehalten.

### Aufgabenbedingung (abstrakt)

Beschreibt eine überprüfbare Voraussetzung. Von dieser abstrakten
Entität werden konkrete Bedingungstypen abgeleitet. Eine
Aufgabenbedingung kann über Zuordnungstabellen bei Spielzielen, bei der
Freischaltung von Dinosauriern oder bei der Freischaltung von Karten
eingesetzt werden.

### Spielbedingung

Verknüpft eine Spielrunde mit den Aufgabenbedingungen ihres Spielziels.
Das Spielziel ist erreicht, wenn sämtliche zugeordneten Bedingungen
erfüllt sind.

### Dinosaurierbedingung

Verknüpft einen Dinosaurier mit seinen Freischaltbedingungen. Ein
Dinosaurier wird verfügbar, wenn sämtliche ihm zugeordneten Bedingungen
erfüllt sind.

### Karte

Repräsentiert ein kaufbares Spielelement, das nach Erfüllung seiner
Freischaltbedingungen verfügbar wird. Der Erwerb kostet Ressourcen. Eine
Karte beeinflusst die Produktionsrate eines festgelegten Generators bzw.
Dinosauriertyps, indem sie diese um einen bestimmten Faktor erhöht. Die
genaue Regel, ob Karten einmalig oder mehrfach gekauft werden können und
ob ihre Wirkung stapelbar ist, wird in der Detailplanung festgelegt.

### Kartenbedingung

Verknüpft eine Karte mit den Aufgabenbedingungen, die vor ihrem
Freischalten erfüllt sein müssen. Alle zugeordneten Bedingungen müssen
erfüllt sein.

### Dinosaurier

Repräsentiert einen Dinosauriertyp im gemeinsamen Katalog des Spiels.
Seine fachlichen Eigenschaften bestimmen unter anderem die
Anschaffungskosten, die produzierte Ressource und die
Basis-Produktionsrate. Der Katalog gilt grundsätzlich für alle Spieler
und Spielrunden.

### Gekaufter Dinosaurier / Spiel-Dinosaurier

Repräsentiert die Verwendung eines Dinosauriertyps innerhalb einer
bestimmten Spielrunde und hält fest, wie viele Exemplare dieses Typs der
Spieler in dieser Runde besitzt. Die Anzahl beeinflusst die
Ressourcenproduktion dieser Runde.

### Spielkarte / Gekaufte Karte

Repräsentiert den Erwerb einer Karte innerhalb einer bestimmten
Spielrunde. Sie verbindet die Karte aus dem gemeinsamen Kartenkatalog
mit der jeweiligen Spielrunde und hält den Besitz sowie gegebenenfalls
die Anzahl fest. Ihre Wirkung wird bei der Berechnung der
Produktionsrate des betroffenen Dinosauriertyps berücksichtigt.

### Ressource

Repräsentiert einen Typ von Spielressource. In der ersten Version wird
nur eine Ressource verwendet. Das Modell soll eine spätere Erweiterung
um zusätzliche Ressourcentypen ermöglichen.

### Spielressource / Ressourcenbestand

Verknüpft eine Spielrunde mit einer Ressource und hält den aktuellen
Bestand sowie die insgesamt produzierte Menge fest. Der Bestand wird
durch Produktion erhöht und durch Käufe von Dinosauriern oder Karten
verringert. Die Gesamtproduktionsmenge steigt nur durch Produktion und
wird durch Ausgaben nicht reduziert.

### Ergebnis einer Spielrunde

Beschreibt das endgültige Resultat einer abgeschlossenen Spielrunde,
insbesondere die bis zum Erreichen des Spielziels benötigte Zeit. Die
gespeicherten Ergebnisse dienen der persönlichen Übersicht und der
Spielerrangliste. Ob das Ergebnis als eigenständige Entität oder als
Teil der abgeschlossenen Spielrunde modelliert wird, wird bei der
detaillierten Datenmodellierung entschieden.

## 7. Benutzer-Storys

-   **Als Spieler** möchte ich einen neuen Spieler anlegen oder einen
    bestehenden Spieler auswählen, damit ich ohne Passwort mit dem
    Spielen beginnen kann.
-   **Als Spieler** möchte ich mehrere unabhängige Spielrunden haben,
    damit ich neue Durchläufe starten kann, ohne frühere Spielstände zu
    verlieren.
-   **Als Spieler** möchte ich Dinosaurier kaufen, damit diese
    Ressourcen produzieren und mir weitere Käufe ermöglichen.
-   **Als Spieler** möchte ich neue Dinosaurier freischalten, indem ich
    alle dafür erforderlichen Bedingungen erfülle.
-   **Als Spieler** möchte ich Karten freischalten und kaufen, damit ich
    die Produktionsrate bestimmter Dinosauriertypen erhöhen kann.
-   **Als Spieler** möchte ich meinen Spielstand speichern und später
    fortsetzen können.
-   **Als Spieler** möchte ich nach Abschluss einer Spielrunde meine
    benötigte Zeit sehen.
-   **Als Spieler** möchte ich meine abgeschlossenen Spielrunden
    miteinander vergleichen können.
-   **Als Spieler** möchte ich die Rangliste der Spieler sehen, die nach
    der jeweils besten abgeschlossenen Spielzeit sortiert ist.

## 8. Umfang und geplante Erweiterungen

### Bestandteil der ersten Version

-   Spieler ohne Passwort anlegen und auswählen
-   Mehrere Spielrunden pro Spieler
-   Dinosaurier kaufen und Ressourcen automatisch produzieren
-   Eine Ressource
-   Freischaltung von Dinosauriern und Karten über mehrere Bedingungen
-   Karten, die die Produktionsrate eines bestimmten Dinosauriertyps
    durch einen Faktor erhöhen
-   Ein Spielzieltyp: eine festgelegte Ressourcenmenge produzieren
-   Speichern und Fortsetzen laufender Spielrunden
-   Abschlusszeit speichern und Ranglisten anzeigen
-   Einfache WPF-Oberfläche

### Nicht Bestandteil der ersten Version

-   Administrationsoberfläche
-   Verwaltung von Benutzern durch einen Administrator
-   Mehrere Ressourcentypen
-   Offline-Produktion
-   Weitere Spielziel- und Bedingungstypen, sofern sie nicht für die
    erste spielbare Version benötigt werden

Die Architektur soll Erweiterungen ermöglichen, ohne diese Funktionen
bereits in der ersten Version umzusetzen. Insbesondere ist eine spätere
Administratorrolle vorgesehen, mit der Spielregeln, Bedingungen und
Ressourcentypen konfiguriert werden können.

## 9. Offene Punkte für die Detailplanung

-   Konkrete Dinosaurier, Karten und ihre Spielwerte
-   Exakte Freischaltbedingungen und deren Parameter
-   Ob Karten nur einmal oder mehrfach gekauft werden können
-   Ob sich die Effekte mehrerer Karten addieren oder multiplizieren
-   Wie sich eine Karte auf die Produktionsrate auswirkt, wenn der
    betreffende Dinosauriertyp noch nicht gekauft wurde
-   Anzahl und Gestaltung der Spielziele für die erste Version
-   Technische Details der Speicherung und Wiederaufnahme einer
    laufenden Spielrunde
-   Umgang mit Änderungen am Dinosaurier- oder Kartenkatalog, wenn
    bereits gespeicherte Spielrunden existieren
-   Endgültige Auswahl der SQL-Datenbank und Konfiguration von EF Core
