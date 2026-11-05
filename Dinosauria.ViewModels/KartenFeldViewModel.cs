using Dinosauria.ViewModels;
using Dinosauria.ViewModels.Basic;
using Dinosauria.Domain.Models;

/// <summary>Stellt einen Kartenknoten des Spielfelds dar.</summary>
public class KartenFeldViewModel : ViewModelBase
{
    public SpielKarte Karte { get; }

    public double PositionX { get; }
    public double PositionY { get; }

    public bool IstGekauft => Karte.IstGekauft;
    public bool IstFreigeschaltet => Karte.IstFreigeschaltet;

    /// <summary>Erzeugt einen Kartennoten an der angegebenen Position.</summary>
    public KartenFeldViewModel(
        SpielKarte karte,
        double positionX,
        double positionY)
    {
        Karte = karte;
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