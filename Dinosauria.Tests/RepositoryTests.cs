using Dinosauria.Domain.Models;
using Dinosauria.Infrastructure.Data;
using Xunit;

namespace Dinosauria.Tests;

public class RepositoryTests
{
    [Fact]
    public void Hinzufuegen_und_Finden_Spieler()
    {
        using var context = DinosauriaDbContextFactory.Create();
        DatenbankInitialisierer.Initialisieren(context);

        var repository = new SpielerRepository(context);

        var spieler = new Spieler
        {
            Name = "Testspieler",
            ErstelltAm = DateTime.Now
        };

        repository.Hinzufuegen(spieler);

        var geladen = repository.Finden(spieler.SpielerID);

        Assert.NotNull(geladen);
        Assert.Equal("Testspieler", geladen.Name);
    }

    
    [Fact]
    public void Hinzufuegen_und_Finden_Spiel()
    {
        using var context = DinosauriaDbContextFactory.Create();
        DatenbankInitialisierer.Initialisieren(context);

        var repository = new SpielRepository(context);

        var spiel = ErstellenSpiel(context);

        repository.Hinzufuegen(spiel);

        var geladen = repository.Finden(spiel.SpielID);

        Assert.NotNull(geladen);
        Assert.Equal(spiel.SpielID, geladen.SpielID);
    }

    [Fact]
    public void AlleLaden_GibtAlleSpieleZurueck()
    {
        using var context = DinosauriaDbContextFactory.Create();
        var repository = new SpielRepository(context);

        var spiel = ErstellenSpiel(context);
        repository.Hinzufuegen(spiel);

        var spiele = repository.AlleLaden();

        Assert.Contains(spiele, s => s.SpielID == spiel.SpielID);
    }

    [Fact]
    public void Aktualisieren_AendertSpiel()
    {
        using var context = DinosauriaDbContextFactory.Create();
        var repository = new SpielRepository(context);

        var spiel = ErstellenSpiel(context);
        repository.Hinzufuegen(spiel);

        spiel.Endzeitpunkt = DateTime.Now;
        repository.Aktualisieren(spiel);

        var geladen = repository.Finden(spiel.SpielID);

        Assert.NotNull(geladen);
        Assert.NotNull(geladen.Endzeitpunkt);
    }

    [Fact]
    public void Entfernen_LoeschtSpiel()
    {
        using var context = DinosauriaDbContextFactory.Create();
        var repository = new SpielRepository(context);

        var spiel = ErstellenSpiel(context);
        repository.Hinzufuegen(spiel);

        repository.Entfernen(spiel.SpielID);

        Assert.Null(repository.Finden(spiel.SpielID));
    }

    

    private static Spiel ErstellenSpiel(DinosauriaDbContext context)
    {
        var spieler = context.Spieler.First();
        if (spieler == null)
        {
            spieler = new Spieler
            {
                Name = "Testspieler",
                ErstelltAm = DateTime.Now
            };
            context.Spieler.Add(spieler);
        }
        var spielziel = context.Spielziele.First();
        if (spielziel == null)
        {
            spielziel = new Spielziel
            {
                Name = "Testziel",
                Beschreibung = "Testbeschreibung"
            };
            context.Spielziele.Add(spielziel);
        }

        
        
        context.SaveChanges();

        return new Spiel
        {
            SpielerID = spieler.SpielerID,
            SpielzielID = spielziel.SpielzielID,
            Startzeitpunkt = DateTime.Now,
            Status = Spielstatus.Neu
        };
    }
}
