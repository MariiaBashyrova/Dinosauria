
using Dinosauria.Domain.Models;
using Dinosauria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dinosauria.Tests;

public class SpielRessourceTests
{
    [Fact]
    public void Hinzufuegen_SpeichertSpielRessourceMitBeziehungen()
    {
        using var context = DinosauriaDbContextFactory.Create();
        DatenbankInitialisierer.Initialisieren(context);

        var spiel = ErstellenSpiel(context);
        var ressource = ErstellenRessource(context);

        var spielRessource = new SpielRessource
        {
            SpielID = spiel.SpielID,
            RessourceID = ressource.RessourceID,
            Bestand = 100,
            GesamtProduziert = 250
        };

        context.SpielRessourcen.Add(spielRessource);
        context.SaveChanges();
        context.ChangeTracker.Clear();

        var geladen = context.SpielRessourcen
            .Include(sr => sr.Spiel)
            .Include(sr => sr.Ressource)
            .First(sr => sr.SpielID == spiel.SpielID &&
                         sr.RessourceID == ressource.RessourceID);

        Assert.Equal(100, geladen.Bestand);
        Assert.Equal(250, geladen.GesamtProduziert);
        Assert.NotNull(geladen.Spiel);
        Assert.NotNull(geladen.Ressource);
    }

    [Fact]
    public void Hinzufuegen_DoppelteSpielRessourceWirftException()
    {
        using var context = DinosauriaDbContextFactory.Create();
        DatenbankInitialisierer.Initialisieren(context);

        var spiel = ErstellenSpiel(context);
        var ressource = ErstellenRessource(context);

        var spielRessource = new SpielRessource
        {
            SpielID = spiel.SpielID,
            RessourceID = ressource.RessourceID,
            Bestand = 100,
            GesamtProduziert = 250
        };

        context.SpielRessourcen.Add(spielRessource);
        context.SaveChanges();
        context.ChangeTracker.Clear();

        var doppelteSpielRessource = new SpielRessource
        {
            SpielID = spiel.SpielID,
            RessourceID = ressource.RessourceID,
            Bestand = 200,
            GesamtProduziert = 300
        };

        context.SpielRessourcen.Add(doppelteSpielRessource);

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

    private static Ressource ErstellenRessource(DinosauriaDbContext context)
    {
        var ressource = new Ressource
        {
            Name = "Testressource",
            Einheit = "Stück"
        };

        context.Ressourcen.Add(ressource);
        context.SaveChanges();

        return ressource;
    }
}
