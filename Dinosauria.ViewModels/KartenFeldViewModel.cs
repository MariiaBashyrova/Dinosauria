using Dinosauria.ViewModels;
using Dinosauria.ViewModels.Basic;
using Dinosauria.Domain.Models;

public class KartenFeldViewModel : ViewModelBase
{
    public SpielKarte Karte { get; }

    public double PositionX { get; }
    public double PositionY { get; }

    public bool IstGekauft => Karte.IstGekauft;
    public bool IstFreigeschaltet => Karte.IstFreigeschaltet;

    public KartenFeldViewModel(
        SpielKarte karte,
        double positionX,
        double positionY)
    {
        Karte = karte;
        PositionX = positionX;
        PositionY = positionY;
    }
    public void Aktualisieren()
    {
        OnPropertyChanged(nameof(IstGekauft));
        OnPropertyChanged(nameof(IstFreigeschaltet));
    }
}