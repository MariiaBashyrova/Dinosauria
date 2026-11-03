using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dinosauria.Domain.Models;
using Dinosauria.Application.Services;
using Dinosauria.ViewModels.Basic;

namespace Dinosauria.ViewModels;

public class SpielKarteViewModel
{
     
    public SpielKarte Karte { get; } 
    public string Name { get; }
    public string Beschreibung { get; }
    public decimal Kosten { get; }
    public RelayCommand KarteKaufenCommand { get; }
    public decimal Multiplikator { get; }
    public bool IstGekauft { get; }
    public bool IstFreigeschaltet { get; }

    public int KarteID => Karte.Karte.KarteID;
    public bool KannKaufen(Spiel spiel)
    {
        var ressourcen = spiel.SpielRessourcen
            .Single(x => x.RessourceID == Karte.Karte.KostenRessourceID);
        return spiel.Status == Spielstatus.Laufend && ressourcen.Bestand >= Kosten;
    }
    public string KarteKaufenCommandContent => IstGekauft ? "Gekauft" : "Kaufen";
    public Dinosaurierart? Dinosaurierart => Karte.Karte.Dinosaurierart;
    public SpielKarteViewModel(Spiel spiel, SpielKarte karte,  Action<int> kaufen)
    {
        Karte = karte;
        Name = karte.Karte.Name;
        Beschreibung = karte.Karte.Beschreibung;
        Kosten = karte.Karte.Kaufkosten;
        Multiplikator = karte.Karte.Multiplikator;
        IstGekauft = karte.IstGekauft; IstFreigeschaltet = karte.IstFreigeschaltet;
        KarteKaufenCommand = new RelayCommand(() => kaufen(karte.Karte.KarteID),
            () => spiel.Status == Spielstatus.Laufend && !IstGekauft && IstFreigeschaltet &&
                  spiel.SpielRessourcen.Single(x =>
                      x.RessourceID == karte.Karte.KostenRessourceID).Bestand >=
                  karte.Karte.Kaufkosten);
    }
    
}
