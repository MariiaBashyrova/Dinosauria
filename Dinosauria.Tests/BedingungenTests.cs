
using Dinosauria.Domain.Models;
using Dinosauria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dinosauria.Tests;

public class BedingungenTests
{
    [Fact]
    public void Hinzufuegen_SpeichertDinosaurierUndKartenbedingungen()
    {
        using var context = DinosauriaDbContextFactory.Create();
        DatenbankInitialisierer.Initialisieren(context);

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

        var dinosaurierBedingung = new DinosaurierKaufen
        {
            Beschreibung = "10 Dinosaurier kaufen",
            Anzahl = 10,
            DinosaurierartID = dinosaurierart.DinosaurierartID
        };

        var kartenBedingung = new RessourceProduzieren
        {
            Beschreibung = "10.000 Fossilien produzieren",
            Zielmenge = 10000,
            RessourceID = ressource.RessourceID
        };

        context.Aufgabenbedingungen.AddRange(dinosaurierBedingung, kartenBedingung);
        context.SaveChanges();

        context.Dinosaurierbedingungen.Add(new Dinosaurierbedingung
        {
            DinosaurierartID = dinosaurierart.DinosaurierartID,
            AufgabenbedingungID = dinosaurierBedingung.AufgabenbedingungID
        });

        context.Kartenbedingungen.Add(new Kartenbedingung
        {
            KarteID = karte.KarteID,
            AufgabenbedingungID = kartenBedingung.AufgabenbedingungID
        });

        context.SaveChanges();
        context.ChangeTracker.Clear();

        var geladeneDinoBedingung = context.Dinosaurierbedingungen
            .Include(b => b.Dinosaurierart)
            .Include(b => b.Aufgabenbedingung)
            .First(b => b.DinosaurierartID == dinosaurierart.DinosaurierartID);

        var geladeneKartenBedingung = context.Kartenbedingungen
            .Include(b => b.Karte)
            .Include(b => b.Aufgabenbedingung)
            .First(b => b.KarteID == karte.KarteID);

        Assert.NotNull(geladeneDinoBedingung.Dinosaurierart);
        Assert.IsType<DinosaurierKaufen>(geladeneDinoBedingung.Aufgabenbedingung);
        Assert.Equal(10, ((DinosaurierKaufen)geladeneDinoBedingung.Aufgabenbedingung).Anzahl);

        Assert.NotNull(geladeneKartenBedingung.Karte);
        Assert.IsType<RessourceProduzieren>(geladeneKartenBedingung.Aufgabenbedingung);
        Assert.Equal(10000, ((RessourceProduzieren)geladeneKartenBedingung.Aufgabenbedingung).Zielmenge);
    }
}
