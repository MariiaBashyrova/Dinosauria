using Dinosauria.Domain.Models;
using Dinosauria.Domain.Repositories;

namespace Dinosauria.Application.Services;

public class SpielService(
    ISpielRepository spielRepository,
    ISpielerRepository spielerRepository,
    IRessourceRepository ressourceRepository,
    IDinosaurierartRepository dinosaurierRepository,
    IKarteRepository karteRepository,
    ISpielzielRepository spielzielRepository)
{
    public Spieler SpielerHinzufuegen(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Der Spielername darf nicht leer sein.");

        var spieler = new Spieler
        {
            Name = name.Trim(),
            ErstelltAm = DateTime.Now
        };

        spielerRepository.Hinzufuegen(spieler);

        return spieler;
    }

    public IReadOnlyList<Spiel> RanglisteErstellen()
    {
        return spielRepository.AlleLaden()
            .Where(s => s.Status == Spielstatus.Abgeschlossen)
            .OrderBy(s => s.Abschlussdauer)
            .ThenBy(s => s.Endzeitpunkt)
            .ToList();
    }
    public Spiel NeuesSpielErstellen(int spielerID)
    {
        var spieler = spielerRepository.Finden(spielerID)
            ?? throw new InvalidOperationException("Spieler nicht gefunden.");

        var ressourcen = ressourceRepository.AlleLaden();
        var dinosaurier = dinosaurierRepository.AlleLaden();
        var karten = karteRepository.AlleLaden();
        var spielziel = spielzielRepository.AlleLaden()
            .Single(x => x.SpielzielID == 17);

        var fossilien = ressourcen.Single(x => x.Name == "Fossilien");

        var spiel = new Spiel
        {
            Spieler = spieler,
            Startzeitpunkt = DateTime.Now,
            Status = Spielstatus.Laufend,
            Abschlussdauer = TimeSpan.Zero,
            Spielziel = spielziel
        };

        spiel.SpielRessourcen.Add(new SpielRessource
        {
            Ressource = fossilien,
            Bestand = 10,
            GesamtProduziert = 0
        });

        foreach (var d in dinosaurier)
        {
            spiel.SpielDinosaurier.Add(new SpielDinosaurier
            {
                Dinosaurierart = d,
                Anzahl = 0,
                IstFreigeschaltet = d.Name == "Plateosaurus"
            });
        }

        foreach (var k in karten)
        {
            spiel.SpielKarten.Add(new SpielKarte
            {
                Karte = k,
                IstFreigeschaltet = false,
                IstGekauft = false
            });
        }

        spielRepository.Hinzufuegen(spiel);

        return spiel;
    }

    public Spiel SpielLaden(int spielID)
    {
        return spielRepository.Finden(spielID)
            ?? throw new InvalidOperationException("Spiel nicht gefunden.");
    }
    public bool DinosaurierKaufen(int spielID, int dinosaurierartID)
    {
        var spiel = spielRepository.Finden(spielID)
            ?? throw new InvalidOperationException("Spiel nicht gefunden.");
        if (spiel.Status != Spielstatus.Laufend)
            return false;
        var dinosaurier = dinosaurierRepository.Finden(dinosaurierartID)
            ?? throw new InvalidOperationException("Dinosaurier nicht gefunden.");

        var spielDinosaurier = spiel.SpielDinosaurier
            .SingleOrDefault(x => x.DinosaurierartID == dinosaurierartID);

        if (spielDinosaurier == null || !spielDinosaurier.IstFreigeschaltet)
            return false;

        var spielRessource = spiel.SpielRessourcen
            .Single(x => x.RessourceID == dinosaurier.KostenRessourceID);
        decimal kosten = KostenBerechnen(dinosaurier, spielDinosaurier);

        if (spielRessource.Bestand < kosten)
            return false;

        spielRessource.Bestand -= kosten;
        spielDinosaurier.Anzahl++;

        spielRepository.Aktualisieren(spiel);
        FreischaltungenPruefen(spielID);
        SpielstatusPruefen(spielID);
        return true;
    }

    

    public void RessourcenProduzieren(int spielID, double vergangeneSekunden)
    {
        
        var spiel = spielRepository.Finden(spielID)
            ?? throw new InvalidOperationException("Spiel nicht gefunden.");
        if (spiel.Status != Spielstatus.Laufend)
            return;
        foreach (var spielDinosaurier in spiel.SpielDinosaurier
            .Where(x => x.Anzahl > 0))
        {
            

            var produktion = DinosaurierProduktionBerechnen(spiel, spielDinosaurier); 
            var menge = (decimal)(produktion * (decimal)vergangeneSekunden);

            var ressource = spiel.SpielRessourcen
                .Single(x => x.RessourceID == spielDinosaurier.Dinosaurierart.ProduktionsressourceID);

            ressource.Bestand += menge;
            ressource.GesamtProduziert += menge;
        }
        spielRepository.Aktualisieren(spiel);
        FreischaltungenPruefen(spielID);
        SpielstatusPruefen(spielID);
    }

    public bool KarteKaufen(int spielID, int karteID)
    {
        var spiel = spielRepository.Finden(spielID)
            ?? throw new InvalidOperationException("Spiel nicht gefunden.");
        if (spiel.Status != Spielstatus.Laufend)
            return false;
        var karte = karteRepository.Finden(karteID)
            ?? throw new InvalidOperationException("Karte nicht gefunden.");

        var spielKarte = spiel.SpielKarten
            .SingleOrDefault(x => x.KarteID == karteID);

        if (spielKarte == null || !spielKarte.IstFreigeschaltet || spielKarte.IstGekauft)
            return false;

        var spielRessource = spiel.SpielRessourcen
            .Single(x => x.RessourceID == karte.KostenRessourceID);

        if (spielRessource.Bestand < karte.Kaufkosten)
            return false;

        spielRessource.Bestand -= karte.Kaufkosten;
        spielKarte.IstGekauft = true;

        spielRepository.Aktualisieren(spiel);

        return true;
    }

    private bool BedingungErfuellt(Aufgabenbedingung bedingung, Spiel spiel)
    {
        return bedingung switch
        {
            DinosaurierKaufen d =>
                spiel.SpielDinosaurier
                    .Any(x => x.DinosaurierartID == d.DinosaurierartID
                           && x.Anzahl >= d.Anzahl),

            RessourceProduzieren r =>
                spiel.SpielRessourcen
                    .Any(x => x.RessourceID == r.RessourceID
                           && x.GesamtProduziert >= r.Zielmenge),

            _ => false
        };
    }

    public void FreischaltungenPruefen(int spielID)
    {
        var spiel = spielRepository.Finden(spielID)
            ?? throw new InvalidOperationException("Spiel nicht gefunden.");

        var dinosaurier = dinosaurierRepository.AlleLaden();
        var karten = karteRepository.AlleLaden();

        bool geaendert = false;

        foreach (var spielDinosaurier in spiel.SpielDinosaurier)
        {
            if (spielDinosaurier.IstFreigeschaltet)
                continue;

            var art = dinosaurier.Single(d =>
                d.DinosaurierartID == spielDinosaurier.DinosaurierartID);

            if (art.Bedingungen.All(b =>
                BedingungErfuellt(b.Aufgabenbedingung, spiel)))
            {
                spielDinosaurier.IstFreigeschaltet = true;
                geaendert = true;
            }
        }

        foreach (var spielKarte in spiel.SpielKarten)
        {
            if (spielKarte.IstFreigeschaltet)
                continue;

            var karte = karten.Single(k =>
                k.KarteID == spielKarte.KarteID);

            if (karte.Bedingungen.All(b =>
                BedingungErfuellt(b.Aufgabenbedingung, spiel)))
            {
                spielKarte.IstFreigeschaltet = true;
                geaendert = true;
            }
        }

        if (geaendert)
            spielRepository.Aktualisieren(spiel);
    }
    private bool SpielzielErreicht(Spiel spiel)
    {
        var spielziel = spielzielRepository.Finden(spiel.SpielzielID);

        if (spielziel == null)
            return false;

        return spielziel.Bedingungen.All(b =>
            BedingungErfuellt(b.Aufgabenbedingung, spiel));
    }

    public Dinosaurierart? VorherigenDinosaurierFinden(int dinosaurierartID)
    {
        var dinosaurier = dinosaurierRepository.AlleLaden();

        var art = dinosaurier.Single(d => d.DinosaurierartID == dinosaurierartID);

        var bedingung = art.Bedingungen
            .Select(b => b.Aufgabenbedingung)
            .OfType<DinosaurierKaufen>()
            .FirstOrDefault();

        if (bedingung == null)
            return null;

        return dinosaurier.Single(d =>
            d.DinosaurierartID == bedingung.DinosaurierartID);
    }
    private void SpielAbschliessen(Spiel spiel)
    {
        spiel.Status = Spielstatus.Abgeschlossen;
        spiel.Endzeitpunkt = DateTime.Now;
        spiel.Abschlussdauer = spiel.Endzeitpunkt - spiel.Startzeitpunkt;
    }
    public void SpielstatusPruefen(int spielID)
    {
        var spiel = spielRepository.Finden(spielID)
            ?? throw new InvalidOperationException("Spiel nicht gefunden.");

        if (spiel.Status != Spielstatus.Laufend)
            return;

        FreischaltungenPruefen(spielID);

        if (SpielzielErreicht(spiel))
        {
            SpielAbschliessen(spiel);
            spielRepository.Aktualisieren(spiel);
        }
    }
    public IReadOnlyList<Spieler> SpielerListeLaden()
    {
        return spielerRepository.AlleLaden();
    }

    public IReadOnlyList<Spiel> SpielerSpieleLaden(int spielerID)
    {
        return spielRepository.AlleLaden()
            .Where(s => s.SpielerID == spielerID)
            .OrderByDescending(s => s.Startzeitpunkt)
            .ToList();
    }

    public void SpielerUmbenennen(int spielerID, string neuerName)
    {
        if (string.IsNullOrWhiteSpace(neuerName))
            throw new ArgumentException("Der Spielername darf nicht leer sein.");

        var spieler = spielerRepository.Finden(spielerID)
            ?? throw new InvalidOperationException("Spieler nicht gefunden.");

        spieler.Name = neuerName.Trim();
        spielerRepository.Aktualisieren(spieler);
    }

    public bool SpielerLoeschen(int spielerID)
    {
        var spieler = spielerRepository.Finden(spielerID)
            ?? throw new InvalidOperationException("Spieler nicht gefunden.");

        if (spielRepository.AlleLaden().Any(s => s.SpielerID == spielerID))
            return false;

        spielerRepository.Entfernen(spielerID);
        return true;
    }

    public void SpielLoeschen(int spielID)
    {
        var spiel = spielRepository.Finden(spielID)
            ?? throw new InvalidOperationException("Spiel nicht gefunden.");

        spielRepository.Entfernen(spielID);
    }

    public decimal DinosaurierProduktionBerechnen(Spiel spiel, SpielDinosaurier spielDinosaurier)
    {
        var dinosaurier = spielDinosaurier.Dinosaurierart;
        decimal multiplikator = ProduktionMultiplikator(spiel, spielDinosaurier);

        return dinosaurier.ProduktionProSekunde * spielDinosaurier.Anzahl * multiplikator;
    }

    public decimal DinosaurierProduktionBerechnenProStueck(Spiel spiel, SpielDinosaurier spielDinosaurier)
    {
        var dinosaurier = spielDinosaurier.Dinosaurierart;
        decimal multiplikator = ProduktionMultiplikator(spiel, spielDinosaurier);

        return dinosaurier.ProduktionProSekunde * multiplikator;
    }

    private static decimal ProduktionMultiplikator(Spiel spiel, SpielDinosaurier spielDinosaurier)
    {
        return spiel.SpielKarten
            .Where(k => k.IstGekauft &&
                        k.Karte.DinosaurierartID == spielDinosaurier.DinosaurierartID)
            .Aggregate(1m, (ergebnis, k) => ergebnis * (decimal)k.Karte.Multiplikator);
    }

    public decimal KostenBerechnen(Dinosaurierart dinosaurier, SpielDinosaurier spielDinosaurier)
    {
        return Math.Round(dinosaurier.Kaufkosten *
            (decimal)Math.Pow(1.15, spielDinosaurier.Anzahl),2);
    }

    public void SpielzeitSpeichern(int spielID, TimeSpan spielzeit)
    {
        var spiel = spielRepository.Finden(spielID)
            ?? throw new InvalidOperationException("Spiel nicht gefunden.");

        spiel.Abschlussdauer = spielzeit;
        spielRepository.Aktualisieren(spiel);
    }
    public Spielziel? SpielzielLaden(int spielzielID)
    {
        return spielzielRepository.Finden(spielzielID);
    }
}