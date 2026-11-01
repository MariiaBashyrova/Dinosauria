
using Dinosauria.Domain.Models;
using Dinosauria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dinosauria.Tests;

public class SpielDinosaurierTests
{
    [Fact]
    public void Hinzufuegen_SpeichertSpielDinosaurierMitBeziehungen()
    {
        using var context = DinosauriaDbContextFactory.Create();
        DatenbankInitialisierer.Initialisieren(context);

        var spiel = ErstellenSpiel(context);
        var dinosaurierart = ErstellenDinosaurierart(context);

        var spielDinosaurier = new SpielDinosaurier
        {
            SpielID = spiel.SpielID,
            DinosaurierartID = dinosaurierart.DinosaurierartID,
            Anzahl = 5,
            IstFreigeschaltet = true
        };

        context.SpielDinosaurier.Add(spielDinosaurier);
        context.SaveChanges();
        context.ChangeTracker.Clear();

        var geladen = context.SpielDinosaurier
            .Include(sd => sd.Spiel)
            .Include(sd => sd.Dinosaurierart)
            .First(sd => sd.SpielID == spiel.SpielID &&
                         sd.DinosaurierartID == dinosaurierart.DinosaurierartID);

        Assert.Equal(5, geladen.Anzahl);
        Assert.True(geladen.IstFreigeschaltet);
        Assert.NotNull(geladen.Spiel);
        Assert.NotNull(geladen.Dinosaurierart);
    }

    [Fact]
    public void Hinzufuegen_DoppelterSpielDinosaurierWirftException()
    {
        using var context = DinosauriaDbContextFactory.Create();
        DatenbankInitialisierer.Initialisieren(context);

        var spiel = ErstellenSpiel(context);
        var dinosaurierart = ErstellenDinosaurierart(context);

        var spielDinosaurier = new SpielDinosaurier
        {
            SpielID = spiel.SpielID,
            DinosaurierartID = dinosaurierart.DinosaurierartID,
            Anzahl = 5,
            IstFreigeschaltet = true
        };

        context.SpielDinosaurier.Add(spielDinosaurier);
        context.SaveChanges();
        context.ChangeTracker.Clear();

        var doppelterSpielDinosaurier = new SpielDinosaurier
        {
            SpielID = spiel.SpielID,
            DinosaurierartID = dinosaurierart.DinosaurierartID,
            Anzahl = 10,
            IstFreigeschaltet = false
        };

        context.SpielDinosaurier.Add(doppelterSpielDinosaurier);

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
