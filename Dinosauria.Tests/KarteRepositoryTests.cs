using Dinosauria.Domain.Models;
using Dinosauria.Infrastructure.Data;
using Xunit;

namespace Dinosauria.Tests;

public class KarteRepositoryTests
{
    [Fact]
    public void AlleLaden_LaedtKartenMitBeziehungen()
    {
        using var context = DinosauriaDbContextFactory.Create();
        DatenbankInitialisierer.Initialisieren(context);

        var repository = new KarteRepository(context);
        var karte = ErstellenKarte(context);

        var ergebnis = repository.AlleLaden();
        var geladen = ergebnis.First(k => k.KarteID == karte.KarteID);

        Assert.NotNull(geladen.KostenRessource);
        Assert.NotNull(geladen.Dinosaurierart);
    }

    [Fact]
    public void Finden_LaedtKarteMitBeziehungen()
    {
        using var context = DinosauriaDbContextFactory.Create();
        var repository = new KarteRepository(context);
        var karte = ErstellenKarte(context);

        var geladen = repository.Finden(karte.KarteID);

        Assert.NotNull(geladen);
        Assert.NotNull(geladen.KostenRessource);
        Assert.NotNull(geladen.Dinosaurierart);
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
            Multiplikator = 1.5m,
            KostenRessourceID = ressource.RessourceID,
            DinosaurierartID = dinosaurierart.DinosaurierartID
        };

        context.Karten.Add(karte);
        context.SaveChanges();

        return karte;
    }
}