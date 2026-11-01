
using Dinosauria.Domain.Models;
using Dinosauria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dinosauria.Tests;

public class SpielKarteTests
{
    [Fact]
    public void Hinzufuegen_SpeichertSpielKarteMitBeziehungen()
    {
        using var context = DinosauriaDbContextFactory.Create();
        DatenbankInitialisierer.Initialisieren(context);

        var spiel = ErstellenSpiel(context);
        var karte = ErstellenKarte(context);

        var spielKarte = new SpielKarte
        {
            SpielID = spiel.SpielID,
            KarteID = karte.KarteID,
            IstFreigeschaltet = true,
            IstGekauft = false
        };

        context.SpielKarten.Add(spielKarte);
        context.SaveChanges();
        context.ChangeTracker.Clear();

        var geladen = context.SpielKarten
            .Include(sk => sk.Spiel)
            .Include(sk => sk.Karte)
            .First(sk => sk.SpielID == spiel.SpielID &&
                         sk.KarteID == karte.KarteID);

        Assert.True(geladen.IstFreigeschaltet);
        Assert.False(geladen.IstGekauft);
        Assert.NotNull(geladen.Spiel);
        Assert.NotNull(geladen.Karte);
    }

    [Fact]
    public void Hinzufuegen_DoppelteSpielKarteWirftException()
    {
        using var context = DinosauriaDbContextFactory.Create();
        DatenbankInitialisierer.Initialisieren(context);

        var spiel = ErstellenSpiel(context);
        var karte = ErstellenKarte(context);

        var spielKarte = new SpielKarte
        {
            SpielID = spiel.SpielID,
            KarteID = karte.KarteID,
            IstFreigeschaltet = true,
            IstGekauft = false
        };

        context.SpielKarten.Add(spielKarte);
        context.SaveChanges();
        context.ChangeTracker.Clear();

        var doppelteSpielKarte = new SpielKarte
        {
            SpielID = spiel.SpielID,
            KarteID = karte.KarteID,
            IstFreigeschaltet = true,
            IstGekauft = true
        };

        context.SpielKarten.Add(doppelteSpielKarte);

        Assert.Throws<DbUpdateException>(() => context.SaveChanges());
    }

    private static Spiel ErstellenSpiel(DinosauriaDbContext context)
    {
        var spieler = context.Spieler.First();

        var spielziel = new Spielziel
        {
            Name = "Testziel",
            Beschreibung = "Testbeschreibung"
        };

        context.Spielziele.Add(spielziel);
        context.SaveChanges();

        var spiel = new Spiel
        {
            SpielerID = spieler.SpielerID,
            SpielzielID = spielziel.SpielzielID,
            Startzeitpunkt = DateTime.Now,
            Status = Spielstatus.Neu
        };

        context.Spiele.Add(spiel);
        context.SaveChanges();

        return spiel;
    }

    private static Karte ErstellenKarte(DinosauriaDbContext context)
    {
        var ressource = new Ressource
        {
            Name = "Fossilien",
            Einheit = "Stück"
        };

        context.Ressourcen.Add(ressource);
        context.SaveChanges();

        var dinosaurierart = new Dinosaurierart
        {
            Name = "Testdinosaurier",
            Beschreibung = "Testbeschreibung",
            Kaufkosten = 10,
            KostenRessourceID = ressource.RessourceID,
            ProduktionProSekunde = 1,
            ProduktionsressourceID = ressource.RessourceID,
            Erdzeitalter = Erdzeitalter.Trias,
            Ernaehrung = Ernaehrung.Pflanzenfresser,
            Lebensraum = Lebensraum.Land
        };

        context.Dinosaurierarten.Add(dinosaurierart);
        context.SaveChanges();

        var karte = new Karte
        {
            Name = "Testkarte",
            Beschreibung = "Testbeschreibung",
            Kaufkosten = 20,
            Multiplikator = 2,
            KostenRessourceID = ressource.RessourceID,
            DinosaurierartID = dinosaurierart.DinosaurierartID
        };

        context.Karten.Add(karte);
        context.SaveChanges();

        return karte;
    }
}
