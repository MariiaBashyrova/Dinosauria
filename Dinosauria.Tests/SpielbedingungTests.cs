
using Dinosauria.Domain.Models;
using Dinosauria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dinosauria.Tests;

public class SpielbedingungTests
{
    [Fact]
    public void Hinzufuegen_SpeichertSpielzielMitBeidenBedingungen()
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

        var spielziel = new Spielziel
        {
            Name = "Testziel",
            Beschreibung = "Kaufe 10 Dinosaurier und produziere 10.000 Fossilien"
        };

        var dinosaurierKaufen = new DinosaurierKaufen
        {
            Beschreibung = "10 Dinosaurier kaufen",
            Anzahl = 10,
            DinosaurierartID = dinosaurierart.DinosaurierartID
        };

        var ressourceProduzieren = new RessourceProduzieren
        {
            Beschreibung = "10.000 Fossilien produzieren",
            Zielmenge = 10000,
            RessourceID = ressource.RessourceID
        };

        context.Spielziele.Add(spielziel);
        context.Aufgabenbedingungen.AddRange(dinosaurierKaufen, ressourceProduzieren);
        context.SaveChanges();

        spielziel.Bedingungen.Add(new Spielbedingung
        {
            SpielzielID = spielziel.SpielzielID,
            AufgabenbedingungID = dinosaurierKaufen.AufgabenbedingungID
        });

        spielziel.Bedingungen.Add(new Spielbedingung
        {
            SpielzielID = spielziel.SpielzielID,
            AufgabenbedingungID = ressourceProduzieren.AufgabenbedingungID
        });

        context.SaveChanges();
        context.ChangeTracker.Clear();

        var geladen = context.Spielziele
            .Include(sz => sz.Bedingungen)
            .ThenInclude(b => b.Aufgabenbedingung)
            .First(sz => sz.SpielzielID == spielziel.SpielzielID);

        Assert.Equal(2, geladen.Bedingungen.Count);

        var kaufbedingung = geladen.Bedingungen
            .Select(b => b.Aufgabenbedingung)
            .OfType<DinosaurierKaufen>()
            .Single();

        Assert.Equal(10, kaufbedingung.Anzahl);
        Assert.Equal(dinosaurierart.DinosaurierartID, kaufbedingung.DinosaurierartID);

        var produktionsbedingung = geladen.Bedingungen
            .Select(b => b.Aufgabenbedingung)
            .OfType<RessourceProduzieren>()
            .Single();

        Assert.Equal(10000, produktionsbedingung.Zielmenge);
        Assert.Equal(ressource.RessourceID, produktionsbedingung.RessourceID);
    }
}
