using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dinosauria.Domain.Models;
using Dinosauria.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dinosauria.Infrastructure.Data;

/// <summary>Persistiert Spielstände und lädt deren vollständige Spielbeziehungen.</summary>
public sealed class SpielRepository(DinosauriaDbContext context) : ISpielRepository
{
    /// <summary>Lädt alle Spielstände mit ihren zentralen Navigationen.</summary>
    public IReadOnlyList<Spiel> AlleLaden()
    {
        return context.Spiele
            .Include(s => s.Spieler)
            .Include(s => s.SpielRessourcen)
            .Include(s => s.SpielDinosaurier)
            .ToList();
    }
    /// <summary>Lädt einen Spielstand einschließlich Ressourcen, Dinosauriern und Karten.</summary>
    public Spiel? Finden(int spielID) =>
    context.Spiele
        .Include(s => s.Spieler)
        .Include(s => s.Spielziel)
        .Include(s => s.SpielRessourcen)
            .ThenInclude(sr => sr.Ressource)
        .Include(s => s.SpielDinosaurier)
            .ThenInclude(sd => sd.Dinosaurierart)
        .Include(s => s.SpielKarten)
            .ThenInclude(sk => sk.Karte)
        .FirstOrDefault(s => s.SpielID == spielID);

    /// <summary>Fügt einen neuen Spielstand hinzu und speichert ihn sofort.</summary>
    public void Hinzufuegen(Spiel spiel)
    {
        context.Spiele.Add(spiel);
        context.SaveChanges();
    }

    /// <summary>Speichert Änderungen an einem Spielstand.</summary>
    public void Aktualisieren(Spiel spiel)
    {
        context.Spiele.Update(spiel);
        context.SaveChanges();
    }

    /// <summary>Entfernt einen Spielstand, falls er gefunden wird.</summary>
    public void Entfernen(int spielID)
    {
        var spiel = Finden(spielID);
        if (spiel is null) return;

        context.Spiele.Remove(spiel);
        context.SaveChanges();
    }
}

/// <summary>Verwaltet Spieler und ihre grundlegenden Stammdaten.</summary>
public sealed class SpielerRepository(DinosauriaDbContext context) : ISpielerRepository
{
    /// <summary>Lädt alle Spieler.</summary>
    public IReadOnlyList<Spieler> AlleLaden() =>
        context.Spieler.ToList();
    /// <summary>Findet einen Spieler anhand seiner ID.</summary>
    public Spieler? Finden(int spielerID) =>
        context.Spieler.FirstOrDefault(s => s.SpielerID == spielerID);

    /// <summary>Speichert einen neuen Spieler.</summary>
    public void Hinzufuegen(Spieler spieler)
    {
        context.Spieler.Add(spieler);
        context.SaveChanges();
    }
    /// <summary>Speichert Änderungen an einem Spieler.</summary>
    public void Aktualisieren(Spieler spieler)
    {
        context.Spieler.Update(spieler);
        context.SaveChanges();
    }
    /// <summary>Entfernt einen Spieler, falls er existiert.</summary>
    public void Entfernen(int spielerID)
    {
        var spieler = Finden(spielerID);
        if (spieler is null) return;
        context.Spieler.Remove(spieler);
        context.SaveChanges();
    }
}

/// <summary>Lädt Spielziele einschließlich ihrer Aufgabenbedingungen.</summary>
public sealed class SpielzielRepository(DinosauriaDbContext context) : ISpielzielRepository
{
    /// <summary>Lädt alle Spielziele.</summary>
    public IReadOnlyList<Spielziel> AlleLaden() =>
        context.Spielziele.ToList();

    /// <summary>Lädt ein Spielziel mit seinen Bedingungen.</summary>
    public Spielziel? Finden(int spielzielID) =>
    context.Spielziele
        .Include(s => s.Bedingungen)
            .ThenInclude(b => b.Aufgabenbedingung)
        .FirstOrDefault(s => s.SpielzielID == spielzielID);
}

/// <summary>Lädt die zentrale Startkonfiguration des Spiels.</summary>
public sealed class SystemKonfigurationRepository(DinosauriaDbContext context) : ISystemKonfigurationRepository
{
    /// <summary>Lädt Konfigurationen einschließlich ihrer Referenzen.</summary>
    public IReadOnlyList<SystemKonfiguration> AlleLaden() =>
        context.SystemKonfigurationen
        .Include(x => x.Spielziel)
        .Include(x => x.Ressource)
        .Include(x => x.ErsterDinosaurier)
        .ToList();

    /// <summary>Findet eine Systemkonfiguration anhand ihrer ID.</summary>
    public SystemKonfiguration? Finden(int ID) =>
    context.SystemKonfigurationen.FirstOrDefault(k => k.ID == ID);
}
/// <summary>Greift auf die statischen Ressourcentypen zu.</summary>
public sealed class RessourceRepository(DinosauriaDbContext context) : IRessourceRepository
{
    /// <summary>Lädt alle Ressourcentypen.</summary>
    public IReadOnlyList<Ressource> AlleLaden() =>
        context.Ressourcen.ToList();

    /// <summary>Findet einen Ressourcentyp anhand seiner ID.</summary>
    public Ressource? Finden(int ressourceID) =>
        context.Ressourcen.FirstOrDefault(r => r.RessourceID == ressourceID);
}

/// <summary>Lädt Dinosaurierarten mit Ressourcen und Freischaltbedingungen.</summary>
public sealed class DinosaurierartRepository(DinosauriaDbContext context) : IDinosaurierartRepository
{
    /// <summary>Lädt alle Dinosaurierarten inklusive ihrer Beziehungen.</summary>
    public IReadOnlyList<Dinosaurierart> AlleLaden() =>
    context.Dinosaurierarten
        .Include(d => d.KostenRessource)
        .Include(d => d.Produktionsressource)
        .Include(d => d.Bedingungen)
            .ThenInclude(b => b.Aufgabenbedingung)
        .ToList();

    /// <summary>Findet eine Dinosaurierart anhand ihrer ID.</summary>
    public Dinosaurierart? Finden(int dinosaurierartID) =>
        context.Dinosaurierarten
            .Include(d => d.KostenRessource)
            .Include(d => d.Produktionsressource)
            .FirstOrDefault(d => d.DinosaurierartID == dinosaurierartID);
}

/// <summary>Lädt Karten mit Kaufressource, Zielart und Bedingungen.</summary>
public sealed class KarteRepository(DinosauriaDbContext context) : IKarteRepository
{
    /// <summary>Lädt alle Karten inklusive ihrer Beziehungen.</summary>
    public IReadOnlyList<Karte> AlleLaden() =>
    context.Karten
        .Include(k => k.KostenRessource)
        .Include(k => k.Dinosaurierart)
        .Include(k => k.Bedingungen)
            .ThenInclude(b => b.Aufgabenbedingung)
        .ToList();

    /// <summary>Findet eine Karte anhand ihrer ID.</summary>
    public Karte? Finden(int karteID) =>
        context.Karten
            .Include(k => k.KostenRessource)
            .Include(k => k.Dinosaurierart)
            .FirstOrDefault(k => k.KarteID == karteID);
}