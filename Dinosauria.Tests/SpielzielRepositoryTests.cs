using Dinosauria.Domain.Models;
using Dinosauria.Infrastructure.Data;
using Xunit;

namespace Dinosauria.Tests;

public class SpielzielRepositoryTests
{
    [Fact]
    public void AlleLaden_GibtAlleSpielzieleZurueck()
    {
        using var context = DinosauriaDbContextFactory.Create();
        DatenbankInitialisierer.Initialisieren(context);

        var repository = new SpielzielRepository(context);
        var spielziel = ErstellenSpielziel(context);

        var ergebnis = repository.AlleLaden();

        Assert.Contains(ergebnis, s => s.SpielzielID == spielziel.SpielzielID);
    }

    [Fact]
    public void Finden_GibtSpielzielZurueck()
    {
        using var context = DinosauriaDbContextFactory.Create();
        var repository = new SpielzielRepository(context);
        var spielziel = ErstellenSpielziel(context);

        var ergebnis = repository.Finden(spielziel.SpielzielID);

        Assert.NotNull(ergebnis);
        Assert.Equal("Testziel", ergebnis.Name);
    }

    private static Spielziel ErstellenSpielziel(DinosauriaDbContext context)
    {
        var spielziel = new Spielziel
        {
            Name = "Testziel",
            Beschreibung = "Testbeschreibung"
        };

        context.Spielziele.Add(spielziel);
        context.SaveChanges();

        return spielziel;
    }
}