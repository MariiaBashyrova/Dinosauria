using Dinosauria.Domain.Models;
using Dinosauria.Infrastructure.Data;
using Xunit;

namespace Dinosauria.Tests;

public class DinosaurierartRepositoryTests
{
    [Fact]
    public void AlleLaden_LaedtDinosaurierartenMitRessourcen()
    {
        using var context = DinosauriaDbContextFactory.Create();
        DatenbankInitialisierer.Initialisieren(context);

        var repository = new DinosaurierartRepository(context);
        var dinosaurierart = ErstellenDinosaurierart(context);

        var ergebnis = repository.AlleLaden();

        var geladen = ergebnis.First(d => d.DinosaurierartID == dinosaurierart.DinosaurierartID);

        Assert.NotNull(geladen.KostenRessource);
        Assert.NotNull(geladen.Produktionsressource);
    }

    [Fact]
    public void Finden_LaedtDinosaurierartMitRessourcen()
    {
        using var context = DinosauriaDbContextFactory.Create();
        var repository = new DinosaurierartRepository(context);
        var dinosaurierart = ErstellenDinosaurierart(context);

        var geladen = repository.Finden(dinosaurierart.DinosaurierartID);

        Assert.NotNull(geladen);
        Assert.NotNull(geladen.KostenRessource);
        Assert.NotNull(geladen.Produktionsressource);
    }

    private static Dinosaurierart ErstellenDinosaurierart(DinosauriaDbContext context)
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

        return dinosaurierart;
    }
}