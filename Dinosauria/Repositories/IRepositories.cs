using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dinosauria.Domain.Models;

namespace Dinosauria.Domain.Repositories;

public interface ISpielRepository
{
    IReadOnlyList<Spiel> AlleLaden();
    Spiel? Finden(int spielID);
    void Hinzufuegen(Spiel spiel);
    void Aktualisieren(Spiel spiel);
    void Entfernen(int spielID);
}


public interface ISpielerRepository
{
    IReadOnlyList<Spieler> AlleLaden();
    Spieler? Finden(int spielerID);
    void Hinzufuegen(Spieler spieler);
    void Aktualisieren(Spieler spieler);
    void Entfernen(int spielerID);
}

public interface ISpielzielRepository
{
    IReadOnlyList<Spielziel> AlleLaden();
    Spielziel? Finden(int spielzielID);
}

public interface ISystemKonfigurationRepository
{
    IReadOnlyList<SystemKonfiguration> AlleLaden();
    SystemKonfiguration? Finden(int ID);
}
public interface IRessourceRepository
{
    IReadOnlyList<Ressource> AlleLaden();
    Ressource? Finden(int ressourceID);
}

public interface IDinosaurierartRepository
{
    IReadOnlyList<Dinosaurierart> AlleLaden();
    Dinosaurierart? Finden(int dinosaurierartID);
}

public interface IKarteRepository
{
    IReadOnlyList<Karte> AlleLaden();
    Karte? Finden(int karteID);
}