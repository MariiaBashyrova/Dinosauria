using System.Collections.ObjectModel;
using Dinosauria.Application.Services;
using Dinosauria.Domain.Models;
using Dinosauria.ViewModels.Basic;

namespace Dinosauria.ViewModels;

public class MainMenuViewModel : AnsichtViewModel
{
    private readonly SpielService _spielService;
    private Spieler? _ausgewaehlterSpieler;
    private string _neuerSpielerName = "";

    public override string Titel => "Hauptmenü";

    public ObservableCollection<Spieler> SpielerListe { get; } = [];
    public ObservableCollection<RanglistenEintragViewModel> Rangliste { get; } = [];
    public ObservableCollection<SpielUebersichtViewModel> SpielerSpiele { get; } = [];

    public RelayCommand SpielerHinzufuegenCommand { get; }
    public RelayCommand NeuesSpielStartenCommand { get; }

    public string NeuerSpielerName
    {
        get => _neuerSpielerName;
        set
        {
            if (SetProperty(ref _neuerSpielerName, value))
                SpielerHinzufuegenCommand.RaiseCanExecuteChanged();
        }
    }

    public Spieler? AusgewaehlterSpieler
    {
        get => _ausgewaehlterSpieler;
        set
        {
            if (SetProperty(ref _ausgewaehlterSpieler, value))
            {
                SpielerSpieleLaden();
                OnPropertyChanged(nameof(SpielerSpieleTitel));
                NeuesSpielStartenCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public string SpielerSpieleTitel =>
        AusgewaehlterSpieler == null
            ? "Spiele des ausgewählten Spielers"
            : $"Spiele von: {AusgewaehlterSpieler.Name}";

    public MainMenuViewModel(SpielService spielService)
    {
        _spielService = spielService;

        SpielerHinzufuegenCommand = new RelayCommand(
            SpielerHinzufuegen,
            () => !string.IsNullOrWhiteSpace(NeuerSpielerName));

        NeuesSpielStartenCommand = new RelayCommand(
            NeuesSpielStarten,
            () => AusgewaehlterSpieler != null);

        Aktualisieren();
    }

    public override void Aktualisieren()
    {
        SpielerListe.Clear();

        foreach (var spieler in _spielService.SpielerListeLaden())
            SpielerListe.Add(spieler);

        RanglisteLaden();

        if (AusgewaehlterSpieler != null)
            AusgewaehlterSpieler = SpielerListe
                .FirstOrDefault(s => s.SpielerID == AusgewaehlterSpieler.SpielerID);
        else
            SpielerSpieleLaden();
    }

    private void SpielerHinzufuegen()
    {
        var spieler = _spielService.SpielerHinzufuegen(NeuerSpielerName);

        SpielerListe.Add(spieler);
        NeuerSpielerName = "";
        AusgewaehlterSpieler = spieler;
    }

    private void NeuesSpielStarten()
    {
        if (AusgewaehlterSpieler == null)
            return;

        _spielService.NeuesSpielErstellen(AusgewaehlterSpieler.SpielerID);

        SpielerSpieleLaden();
    }

    private void RanglisteLaden()
    {
        Rangliste.Clear();

        var gruppen = _spielService.RanglisteErstellen()
            .GroupBy(s => s.SpielerID)
            .Select(g => new
            {
                Spielername = g.First().Spieler.Name,
                Bestzeit = g.Min(s => s.Abschlussdauer),
                AnzahlSpiele = g.Count()
            })
            .OrderBy(g => g.Bestzeit)
            .ToList();

        for (int i = 0; i < gruppen.Count; i++)
        {
            Rangliste.Add(new RanglistenEintragViewModel
            {
                Platz = i + 1,
                Spielername = gruppen[i].Spielername,
                Bestzeit = gruppen[i].Bestzeit,
                AnzahlSpiele = gruppen[i].AnzahlSpiele
            });
        }
    }

    private void SpielerSpieleLaden()
    {
        SpielerSpiele.Clear();

        if (AusgewaehlterSpieler == null)
            return;

        foreach (var spiel in _spielService.SpielerSpieleLaden(AusgewaehlterSpieler.SpielerID))
        {
            SpielerSpiele.Add(new SpielUebersichtViewModel
            {
                SpielID = spiel.SpielID,
                Startzeitpunkt = spiel.Startzeitpunkt,
                Abschlussdauer = spiel.Abschlussdauer,
                GesamtProduziert = spiel.SpielRessourcen.Sum(r => r.GesamtProduziert),
                FreigeschalteteDinosaurier = spiel.SpielDinosaurier.Count(d => d.IstFreigeschaltet),
                Status = spiel.Status
            });
        }
    }
}