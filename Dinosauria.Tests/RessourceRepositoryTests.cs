using Dinosauria.Domain.Models;
using Dinosauria.Infrastructure.Data;

using Xunit;

namespace Dinosauria.Tests;

public class RessourceRepositoryTests
{
    [Fact]
    public void AlleLaden_GibtAlleRessourcenZurueck()
    {
        using var context = DinosauriaDbContextFactory.Create();
        DatenbankInitialisierer.Initialisieren(context);

        var repository = new RessourceRepository(context);
        var ressource = ErstellenRessource(context);

        var ergebnis = repository.AlleLaden();

        Assert.Contains(ergebnis, r => r.RessourceID == ressource.RessourceID);
    }

    [Fact]
    public void Finden_GibtRessourceZurueck()
    {
        using var context = DinosauriaDbContextFactory.Create();
        var repository = new RessourceRepository(context);
        var ressource = ErstellenRessource(context);

        var ergebnis = repository.Finden(ressource.RessourceID);

        Assert.NotNull(ergebnis);
        Assert.Equal("Fossilien", ergebnis.Name);
    }

    private static Ressource ErstellenRessource(DinosauriaDbContext context)
    {
        var ressource = new Ressource
        {
            Name = "Fossilien",
            Einheit = "Stück"
        };

        context.Ressourcen.Add(ressource);
        context.SaveChanges();

        return ressource;
    }
}