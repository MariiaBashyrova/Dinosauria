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
    public event Action<int>? SpielOeffnenRequested;
    public event Action<int>? SpielLoeschenRequested;
    public ObservableCollection<Spieler> SpielerListe { get; } = [];
    public ObservableCollection<RanglistenEintragViewModel> Rangliste { get; } = [];
    public ObservableCollection<SpielUebersichtViewModel> SpielerSpiele { get; } = [];

    public RelayCommand SpielerHinzufuegenCommand { get; }
    public RelayCommand NeuesSpielStartenCommand { get; }
    public RelayCommand SpielLoeschenCommand { get; }

    public RelayCommand SpielFortsetzenCommand { get; }
    public RelayCommand SpielerUmbenennenCommand { get; }

    public RelayCommand SpielerLoeschenCommand { get; }

    public RelayCommand BeendenCommand { get; }

    private bool SpielerHatSpiele =>
    AusgewaehlterSpieler != null &&
    _spielService.SpielerSpieleLaden(AusgewaehlterSpieler.SpielerID).Count > 0;

    public string NeuerSpielerName
    {
        get => _neuerSpielerName;
        set
        {
            if (SetProperty(ref _neuerSpielerName, value))
            {
                SpielerHinzufuegenCommand.RaiseCanExecuteChanged();
                SpielerUmbenennenCommand.RaiseCanExecuteChanged();
            }
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
                SpielerUmbenennenCommand.RaiseCanExecuteChanged();
                SpielerLoeschenCommand.RaiseCanExecuteChanged();
            }
        }
    }

    private SpielUebersichtViewModel? _ausgewaehltesSpiel;
    public SpielUebersichtViewModel? AusgewaehltesSpiel
    {
        get => _ausgewaehltesSpiel;
        set
        {
            if (SetProperty(ref _ausgewaehltesSpiel, value))
            {
                SpielLoeschenCommand.RaiseCanExecuteChanged();
                SpielFortsetzenCommand.RaiseCanExecuteChanged();
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
       
        SpielerUmbenennenCommand = new RelayCommand(
            SpielerUmbenennen,
            () => AusgewaehlterSpieler != null &&
          !string.IsNullOrWhiteSpace(NeuerSpielerName));

        SpielerLoeschenCommand = new RelayCommand(
            SpielerLoeschen,
            () => AusgewaehlterSpieler != null && !SpielerHatSpiele);
        Aktualisieren();

        SpielLoeschenCommand = new RelayCommand(SpielLoeschen, () => AusgewaehltesSpiel != null);
        SpielFortsetzenCommand = new RelayCommand(SpielFortsetzen,   () => AusgewaehltesSpiel != null &&  AusgewaehltesSpiel.Status == Spielstatus.Laufend);
        BeendenCommand = new RelayCommand(Beenden);
    }

    public override void Aktualisieren()
    {
        var ausgewaehlterSpielerID = AusgewaehlterSpieler?.SpielerID;

        SpielerListe.Clear();

        foreach (var spieler in _spielService.SpielerListeLaden())
            SpielerListe.Add(spieler);

        RanglisteLaden();

        AusgewaehlterSpieler = ausgewaehlterSpielerID != null
       ? SpielerListe.FirstOrDefault(s => s.SpielerID == ausgewaehlterSpielerID)
       : SpielerListe.FirstOrDefault();
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

        var spiel = _spielService.NeuesSpielErstellen(AusgewaehlterSpieler.SpielerID);

        SpielOeffnenRequested?.Invoke(spiel.SpielID);
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

    private void SpielerUmbenennen()
    {
        if (AusgewaehlterSpieler == null)
            return;

        int spielerID = AusgewaehlterSpieler.SpielerID;

        _spielService.SpielerUmbenennen(spielerID, NeuerSpielerName);

        var ausgewaehlterSpieler = AusgewaehlterSpieler;
        ausgewaehlterSpieler.Name = NeuerSpielerName.Trim();

        _spielService.SpielerUmbenennen(
            ausgewaehlterSpieler.SpielerID, NeuerSpielerName);

        var index = SpielerListe.IndexOf(ausgewaehlterSpieler);

        SpielerListe.RemoveAt(index);
        SpielerListe.Insert(index, ausgewaehlterSpieler);

        AusgewaehlterSpieler = ausgewaehlterSpieler;
        NeuerSpielerName = "";
    }

    private void SpielerLoeschen()
    {
        if (AusgewaehlterSpieler == null)
            return;

        bool geloescht = _spielService.SpielerLoeschen(
            AusgewaehlterSpieler.SpielerID);

        if (!geloescht)
            return;

        SpielerListe.Remove(AusgewaehlterSpieler);
        AusgewaehlterSpieler = null;
    }
    private void Beenden()
    {
        BeendenRequested?.Invoke();
    }

    public event Action? BeendenRequested;

    private void SpielLoeschen()
    {
        if (AusgewaehltesSpiel == null)
            return;

       // _spielService.SpielLoeschen(AusgewaehltesSpiel.SpielID);

        SpielLoeschenRequested?.Invoke(AusgewaehltesSpiel.SpielID);

        //SpielerSpieleLaden();
        //RanglisteLaden();

        //AusgewaehltesSpiel = null;
    }
    private void SpielFortsetzen()
    {
        if (AusgewaehltesSpiel == null) return;

        SpielOeffnenRequested?.Invoke(AusgewaehltesSpiel.SpielID);
    }
}