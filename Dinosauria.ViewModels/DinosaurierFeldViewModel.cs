using Dinosauria.Domain.Models;
using Dinosauria.ViewModels;
using Dinosauria.ViewModels.Basic;

/// <summary>Stellt einen Dinosaurierknoten des Spielfelds dar.</summary>
public class DinosaurierFeldViewModel : ViewModelBase
{
    public SpielDinosaurier Dinosaurier { get; }

    public double PositionX { get; }
    public double PositionY { get; }

    public bool IstGekauft => Dinosaurier.Anzahl > 0;
    public bool IstFreigeschaltet => Dinosaurier.IstFreigeschaltet;

    public DinosaurierFeldViewModel? VorherigerDinosaurier { get;  }
    /// <summary>Erzeugt einen Knoten mit Position und optionalem Vorgängerknoten.</summary>
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
    /// <summary>Meldet geänderte Kauf- und Freischaltungszustände an die Oberfläche.</summary>
    public void Aktualisieren()
    {
        OnPropertyChanged(nameof(IstGekauft));
        OnPropertyChanged(nameof(IstFreigeschaltet));
    }
}