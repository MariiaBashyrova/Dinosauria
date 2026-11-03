using Dinosauria.ViewModels.Basic;

public class VerbindungViewModel : ViewModelBase
{
    public double StartX => DinosaurierStart!.PositionX;
    public double StartY => DinosaurierStart!.PositionY;
    public double EndeX => DinosaurierEnde?.PositionX ?? KarteEnde!.PositionX;
    public double EndeY => DinosaurierEnde?.PositionY ?? KarteEnde!.PositionY;

    public bool IstFreigeschaltet =>
    DinosaurierEnde?.IstFreigeschaltet ?? KarteEnde!.IstFreigeschaltet;

   

    public DinosaurierFeldViewModel? DinosaurierStart { get; }
    public DinosaurierFeldViewModel? DinosaurierEnde { get; }
    public KartenFeldViewModel? KarteEnde { get; }
    public VerbindungViewModel(
    DinosaurierFeldViewModel start,
    DinosaurierFeldViewModel ende)
    {
        DinosaurierStart = start;
        DinosaurierEnde = ende;
    }

    public VerbindungViewModel(
        DinosaurierFeldViewModel start,
        KartenFeldViewModel ende)
    {
        DinosaurierStart = start;
        KarteEnde = ende;
    }
    public void Aktualisieren()
    {
        OnPropertyChanged(nameof(IstFreigeschaltet));
    }
}