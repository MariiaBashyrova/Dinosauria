using Dinosauria.Domain.Models;
using Dinosauria.ViewModels;
using Dinosauria.ViewModels.Basic;

public class DinosaurierFeldViewModel : ViewModelBase
{
    public SpielDinosaurier Dinosaurier { get; }

    public double PositionX { get; }
    public double PositionY { get; }

    public bool IstGekauft => Dinosaurier.Anzahl > 0;
    public bool IstFreigeschaltet => Dinosaurier.IstFreigeschaltet;

    public DinosaurierFeldViewModel? VorherigerDinosaurier { get;  }
    public DinosaurierFeldViewModel(
    SpielDinosaurier dinosaurier,
    DinosaurierFeldViewModel? vorherigerDinosaurier,
    double positionX,
    double positionY)
    {
        Dinosaurier = dinosaurier;
        VorherigerDinosaurier = vorherigerDinosaurier;
        PositionX = positionX;
        PositionY = positionY;
    }
    public void Aktualisieren()
    {
        OnPropertyChanged(nameof(IstGekauft));
        OnPropertyChanged(nameof(IstFreigeschaltet));
    }
}