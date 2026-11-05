using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dinosauria.Domain.Models;
using Dinosauria.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dinosauria.Infrastructure.Data;

public sealed class SpielRepository(DinosauriaDbContext context) : ISpielRepository
{
    public IReadOnlyList<Spiel> AlleLaden()
    {
        return context.Spiele
            .Include(s => s.Spieler)
            .Include(s => s.SpielRessourcen)
            .Include(s => s.SpielDinosaurier)
            .ToList();
    }
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

    public void Hinzufuegen(Spiel spiel)
    {
        context.Spiele.Add(spiel);
        context.SaveChanges();
    }

    public void Aktualisieren(Spiel spiel)
    {
        context.Spiele.Update(spiel);
        context.SaveChanges();
    }

    public void Entfernen(int spielID)
    {
        var spiel = Finden(spielID);
        if (spiel is null) return;

        context.Spiele.Remove(spiel);
        context.SaveChanges();
    }
}

public sealed class SpielerRepository(DinosauriaDbContext context) : ISpielerRepository
{
    public IReadOnlyList<Spieler> AlleLaden() =>
        context.Spieler.ToList();
    public Spieler? Finden(int spielerID) =>
        context.Spieler.FirstOrDefault(s => s.SpielerID == spielerID);

    public void Hinzufuegen(Spieler spieler)
    {
        context.Spieler.Add(spieler);
        context.SaveChanges();
    }
    public void Aktualisieren(Spieler spieler)
    {
        context.Spieler.Update(spieler);
        context.SaveChanges();
    }
    public void Entfernen(int spielerID)
    {
        var spieler = Finden(spielerID);
        if (spieler is null) return;
        context.Spieler.Remove(spieler);
        context.SaveChanges();
    }
}

public sealed class SpielzielRepository(DinosauriaDbContext context) : ISpielzielRepository
{
    public IReadOnlyList<Spielziel> AlleLaden() =>
        context.Spielziele.ToList();

    public Spielziel? Finden(int spielzielID) =>
    context.Spielziele
        .Include(s => s.Bedingungen)
            .ThenInclude(b => b.Aufgabenbedingung)
        .FirstOrDefault(s => s.SpielzielID == spielzielID);
}

public sealed class SystemKonfigurationRepository(DinosauriaDbContext context) : ISystemKonfigurationRepository
{
    public IReadOnlyList<SystemKonfiguration> AlleLaden() =>
        context.SystemKonfigurationen
        .Include(x => x.Spielziel)
        .Include(x => x.Ressource)
        .Include(x => x.ErsterDinosaurier)
        .ToList();

    public SystemKonfiguration? Finden(int ID) =>
    context.SystemKonfigurationen.FirstOrDefault(k => k.ID == ID);
}
public sealed class RessourceRepository(DinosauriaDbContext context) : IRessourceRepository
{
    public IReadOnlyList<Ressource> AlleLaden() =>
        context.Ressourcen.ToList();

    public Ressource? Finden(int ressourceID) =>
        context.Ressourcen.FirstOrDefault(r => r.RessourceID == ressourceID);
}

public sealed class DinosaurierartRepository(DinosauriaDbContext context) : IDinosaurierartRepository
{
    public IReadOnlyList<Dinosaurierart> AlleLaden() =>
    context.Dinosaurierarten
        .Include(d => d.KostenRessource)
        .Include(d => d.Produktionsressource)
        .Include(d => d.Bedingungen)
            .ThenInclude(b => b.Aufgabenbedingung)
        .ToList();

    public Dinosaurierart? Finden(int dinosaurierartID) =>
        context.Dinosaurierarten
            .Include(d => d.KostenRessource)
            .Include(d => d.Produktionsressource)
            .FirstOrDefault(d => d.DinosaurierartID == dinosaurierartID);
}

public sealed class KarteRepository(DinosauriaDbContext context) : IKarteRepository
{
    public IReadOnlyList<Karte> AlleLaden() =>
    context.Karten
        .Include(k => k.KostenRessource)
        .Include(k => k.Dinosaurierart)
        .Include(k => k.Bedingungen)
            .ThenInclude(b => b.Aufgabenbedingung)
        .ToList();

    public Karte? Finden(int karteID) =>
        context.Karten
            .Include(k => k.KostenRessource)
            .Include(k => k.Dinosaurierart)
            .FirstOrDefault(k => k.KarteID == karteID);
}