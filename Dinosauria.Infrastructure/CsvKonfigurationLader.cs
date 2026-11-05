using System.Globalization;
using Dinosauria.Domain.Models;
using Dinosauria.Infrastructure.Data;

public class CsvKonfigurationLader
{
    private readonly DinosauriaDbContext _context;
    private readonly string _pfad;

    public CsvKonfigurationLader(DinosauriaDbContext context, string pfad)
    {
        _context = context;
        _pfad = pfad;
    }

    private List<string[]> CsvLesen(string datei)
    {
        return File.ReadAllLines(Path.Combine(_pfad, datei))
            .Skip(1)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Split(';'))
            .ToList();
    }

    private Dictionary<string, string> KonfigurationsWerteLaden()
    {
        return CsvLesen("konfiguration.csv")
            .ToDictionary(x => x[0], x => x[1]);
    }
    public void Laden()
    {
        if (_context.Set<Ressource>().Any())
            return;

        var werte = KonfigurationsWerteLaden();

        RessourcenLaden();
        DinosaurierLaden(werte["Ressource"]);
        KartenLaden(werte["Ressource"]);
        SpielzielLaden();
        SystemKonfigurationLaden(werte);
        BedingungenLaden();

        _context.SaveChanges();
    }

    private void RessourcenLaden()
    {
        foreach (var zeile in CsvLesen("ressourcen.csv"))
        {
            _context.Set<Ressource>().Add(new Ressource
            {
                Name = zeile[1],
                Einheit = zeile[2]
            });
        }

        _context.SaveChanges();
    }

    private void DinosaurierLaden(string ressourcenName)
    {
        var fossilien = _context.Set<Ressource>().Single(x => x.Name == ressourcenName);

        foreach (var zeile in CsvLesen("dinosaurier.csv"))
        {
            _context.Set<Dinosaurierart>().Add(new Dinosaurierart
            {
                Name = zeile[1],
                Beschreibung = zeile[2],
                Erdzeitalter = Enum.Parse<Erdzeitalter>(zeile[3]),
                Ernaehrung = Enum.Parse<Ernaehrung>(zeile[4]),
                Lebensraum = Enum.Parse<Lebensraum>(zeile[5]),
                Kaufkosten = decimal.Parse(zeile[6], CultureInfo.InvariantCulture),
                ProduktionProSekunde = decimal.Parse(zeile[7], CultureInfo.InvariantCulture),
                KostenRessource = fossilien,
                Produktionsressource = fossilien,
                Bilddatei = zeile[8]
            });
        }

        _context.SaveChanges();
    }
    private void KartenLaden(string ressourcenName)
    {
        var dinosaurier = CsvLesen("dinosaurier.csv")
    .ToDictionary(
        zeile => zeile[0],
        zeile => _context.Set<Dinosaurierart>()
            .Single(d => d.Name == zeile[1]));

        var fossilien = _context.Set<Ressource>()
            .Single(x => x.Name == ressourcenName);

        foreach (var zeile in CsvLesen("karten.csv"))
        {
            var karte = new Karte
            {
                Name = zeile[1],
                Beschreibung = zeile[2],
                Dinosaurierart = dinosaurier[zeile[3]],
                Kaufkosten = decimal.Parse(zeile[4], CultureInfo.InvariantCulture),
                Multiplikator = decimal.Parse(zeile[5], CultureInfo.InvariantCulture),
                KostenRessource = fossilien,
                Bilddatei = zeile[6]
            };

            _context.Set<Karte>().Add(karte);
        }

        _context.SaveChanges();
    }
    private void SpielzielLaden()
    {
        foreach (var zeile in CsvLesen("spielziel.csv"))
        {
            _context.Set<Spielziel>().Add(new Spielziel
            {
                Name = zeile[1],
                Beschreibung = zeile[2]
            });
        }

        _context.SaveChanges();
    }

    private void SystemKonfigurationLaden(Dictionary<string, string> werte)
    {
        var spielziel = _context.Spielziele
            .Single(x => x.Name == werte["Spielziel"]);

        var ressource = _context.Ressourcen
            .Single(x => x.Name == werte["Ressource"]);

        var dinosaurier = _context.Dinosaurierarten
            .Single(x => x.Name == werte["ErsterDinosaurier"]);

        _context.SystemKonfigurationen.Add(new SystemKonfiguration
        {
            Spielziel = spielziel,
            Ressource = ressource,
            ErsterDinosaurier = dinosaurier,
            Startmenge = decimal.Parse(werte["Startmenge"], CultureInfo.InvariantCulture)
        });

        _context.SaveChanges();
    }
    private void BedingungenLaden()
    {
        var dinosaurier = CsvLesen("dinosaurier.csv")
      .ToDictionary(
          zeile => zeile[0],
          zeile => _context.Set<Dinosaurierart>()
              .Single(d => d.Name == zeile[1]));

        var ressourcen = CsvLesen("ressourcen.csv")
            .ToDictionary(
                zeile => zeile[0],
                zeile => _context.Set<Ressource>()
                    .Single(r => r.Name == zeile[1]));

        var spielziel = _context.SystemKonfigurationen
    .Select(x => x.Spielziel)
    .Single();

        var karten = CsvLesen("karten.csv")
            .ToDictionary(
                zeile => zeile[0],
                zeile => _context.Set<Karte>()
                    .Single(k => k.Name == zeile[1]));

        foreach (var zeile in CsvLesen("bedingungen.csv"))
        {
            Aufgabenbedingung bedingung;

            switch (zeile[2])
            {
                case "RessourceProduzieren":
                    bedingung = new RessourceProduzieren
                    {
                        Beschreibung = zeile[3],
                        Ressource = ressourcen[zeile[5]],
                        Zielmenge = decimal.Parse(zeile[7], CultureInfo.InvariantCulture)
                    };
                    break;

                case "DinosaurierKaufen":
                    bedingung = new DinosaurierKaufen
                    {
                        Beschreibung = zeile[3],
                        Dinosaurierart = dinosaurier[zeile[4]],
                        Anzahl = int.Parse(zeile[6])
                    };
                    break;

                default:
                    throw new InvalidOperationException(
                        $"Unbekannter Bedingungstyp: {zeile[2]}");
            }

            _context.Set<Aufgabenbedingung>().Add(bedingung);
            _context.SaveChanges();

            switch (zeile[0])
            {
                case "Spielziel":
                    _context.Set<Spielbedingung>().Add(new Spielbedingung
                    {
                        Spielziel = spielziel,
                        Aufgabenbedingung = bedingung
                    });
                    break;

                case "Dinosaurier":
                    _context.Set<Dinosaurierbedingung>().Add(new Dinosaurierbedingung
                    {
                        Dinosaurierart = dinosaurier[zeile[1]],
                        Aufgabenbedingung = bedingung
                    });
                    break;

                case "Karte":
                    _context.Set<Kartenbedingung>().Add(new Kartenbedingung
                    {
                        Karte = karten[zeile[1]],
                        Aufgabenbedingung = bedingung
                    });
                    break;
            }
        }

        _context.SaveChanges();
    }
}