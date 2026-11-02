using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dinosauria.Domain.Models;
using Dinosauria.Application.Services;
using Dinosauria.ViewModels.Basic;


namespace Dinosauria.ViewModels;

public class SpielDinosaurierViewModel(Spiel spiel,
    SpielDinosaurier dinosaurier,
    SpielService _spielService,
    Action<int> kaufen)
{
    public SpielDinosaurier Dinosaurier { get; } = dinosaurier;
    public string Name => Dinosaurier.Dinosaurierart.Name;
    public int Anzahl => Dinosaurier.Anzahl;
    public decimal ProduktionProSekunde { get; } = _spielService.DinosaurierProduktionBerechnen(spiel, dinosaurier);
    public bool IstGekauft => Anzahl > 0;
    public string proKauf => $"+ {_spielService.DinosaurierProduktionBerechnenProStueck(spiel, dinosaurier), 2:N2}";

    public decimal Kosten => _spielService.KostenBerechnen(dinosaurier.Dinosaurierart,dinosaurier);
    public RelayCommand KaufenCommand { get; } =
         new(() => kaufen(dinosaurier.DinosaurierartID),
        () => spiel.Status == Spielstatus.Laufend &&
              spiel.SpielRessourcen.Single(x =>
                  x.RessourceID == dinosaurier.Dinosaurierart.KostenRessourceID).Bestand >=
              _spielService.KostenBerechnen(dinosaurier.Dinosaurierart, dinosaurier));

    public bool KannKaufen
    {
        get
        {
            var ressourcen = spiel.SpielRessourcen
                .Single(x => x.RessourceID == dinosaurier.Dinosaurierart.KostenRessourceID);

            return spiel.Status == Spielstatus.Laufend && ressourcen.Bestand >= Kosten;
        }
    }
}