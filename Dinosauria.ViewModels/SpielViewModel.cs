using System.Collections.ObjectModel;
using System.Diagnostics;
using Dinosauria.Application.Services;
using Dinosauria.Domain.Models;
using Dinosauria.ViewModels.Basic;

namespace Dinosauria.ViewModels;

public class SpielViewModel : AnsichtViewModel
{
    private readonly SpielService _spielService;
    private readonly int _spielID;
    private Spiel? _spiel;
    private TimeSpan _spielzeit;
    public override string Titel => "Spielübersicht";
    public string Spielzeit => _spielzeit.ToString(@"hh\:mm\:ss");
    public string Spielername => _spiel?.Spieler.Name ?? "";
    public decimal Fossilien => _spiel?.SpielRessourcen.FirstOrDefault()?.Bestand ?? 0;

    public decimal GesamtProduziert => _spiel?.SpielRessourcen.FirstOrDefault()?.GesamtProduziert ?? 0;
    public string SpielzielName => _spiel?.Spielziel.Name ?? "";
    
    public decimal GesamtProduktion => _spiel?.SpielDinosaurier
    .Where(d => d.Anzahl > 0)
    .Sum(d => _spielService.DinosaurierProduktionBerechnen(_spiel, d)) ?? 0;
    public double SpielzielFortschritt
    {
        get
        {
            var spielziel = _spielService.SpielzielLaden(_spiel!.SpielzielID);
            var bedingung = spielziel?.Bedingungen
                .Select(b => b.Aufgabenbedingung)
                .OfType<RessourceProduzieren>()
                .FirstOrDefault();

            if (bedingung == null)
                return 0;

            var produziert = _spiel.SpielRessourcen
                .FirstOrDefault(r => r.RessourceID == bedingung.RessourceID)?
                .GesamtProduziert ?? 0;

            return Math.Min(100, (double)(produziert / bedingung.Zielmenge * 100));
        }
    }
    public string SpielzielFortschrittText =>
    $"{SpielzielFortschritt:N1} %";
    public string SpielzielAnzeige => SpielzielName;
    public ObservableCollection<SpielDinosaurierViewModel> DinosaurierListe { get; } = [];
    public ObservableCollection<SpielKarteViewModel> KartenListe { get; } = [];

    private int _aktiveTabelle;
    public int AktiveTabelle
    {
        get => _aktiveTabelle;
        set => SetProperty(ref _aktiveTabelle, value);
    }

    private SpielDinosaurierViewModel? _ausgewaehlterDinosaurier;
    public SpielDinosaurierViewModel? AusgewaehlterDinosaurier
    {
        get => _ausgewaehlterDinosaurier;
        set
        {
            if (!SetProperty(ref _ausgewaehlterDinosaurier, value)) return;
            
        }
    }

    private SpielKarteViewModel? _ausgewaehlteKarte;
    public SpielKarteViewModel? AusgewaehlteKarte
    {
        get => _ausgewaehlteKarte;
        set
        {
            if (!SetProperty(ref _ausgewaehlteKarte, value)) return;
           
        }
    }
    public SpielViewModel(SpielService spielService, int spielID)
    {
        _spielService = spielService;
        _spielID = spielID;
       
       
        Aktualisieren();
        
    }

    
    public override void Aktualisieren()
    {
        _spiel = _spielService.SpielLaden(_spielID);

        if (AktiveTabelle==0)
        {   
            var ausgewaehlterDinosaurierID = AusgewaehlterDinosaurier?.Dinosaurier.DinosaurierartID;
            DinosaurierListe.Clear();
            foreach (var dinosaurier in _spiel.SpielDinosaurier.Where(d => d.IstFreigeschaltet))
                DinosaurierListe.Add(new SpielDinosaurierViewModel(_spiel, dinosaurier, _spielService, DinosaurierKaufen));
            if (ausgewaehlterDinosaurierID != null) AusgewaehlterDinosaurier = DinosaurierListe.FirstOrDefault(d => d.DinosaurierartID == ausgewaehlterDinosaurierID);
        }
        else if (AktiveTabelle==1)
        {
            var ausgewaehlteKarteID = AusgewaehlteKarte?.Karte.KarteID;
            KartenListe.Clear();
            foreach (var karte in _spiel.SpielKarten.Where(k => k.IstFreigeschaltet))
                KartenListe.Add(new SpielKarteViewModel(_spiel, karte, KarteKaufen));
            if (ausgewaehlteKarteID != null) AusgewaehlteKarte = KartenListe.FirstOrDefault(k => k.KarteID == ausgewaehlteKarteID);
        }


        //Debug.WriteLine(AktiveTabelle);
        //Debug.WriteLine($"Выбранная строка дино: {AusgewaehlterDinosaurier?.Name ?? "нет"}");
        //Debug.WriteLine($"Выбранная строка карта: {AusgewaehlteKarte?.Name ?? "нет"}");

        OnPropertyChanged(nameof(Spielername));
        OnPropertyChanged(nameof(Fossilien));
        OnPropertyChanged(nameof(SpielzielName));
        
    }
    private void DinosaurierKaufen(int dinosaurierartID)
    {
        if (_spielService.DinosaurierKaufen(_spielID, dinosaurierartID))
            Aktualisieren();
    }

    private void KarteKaufen(int karteID)
    {
        if (_spielService.KarteKaufen(_spielID, karteID))
            Aktualisieren();
        
    }

    public void RessourcenAktualisieren(double vergangeneSekunden)
    {
        _spielService.RessourcenProduzieren(_spielID, vergangeneSekunden);
        
        OnPropertyChanged(nameof(SpielzielFortschritt));
        OnPropertyChanged(nameof(SpielzielFortschrittText));
        OnPropertyChanged(nameof(GesamtProduktion));
        OnPropertyChanged(nameof(GesamtProduziert));
        Aktualisieren();
    }

    public void SpielzeitAktualisieren(double vergangeneSekunden)
    {
        _spielzeit += TimeSpan.FromSeconds(vergangeneSekunden);
        OnPropertyChanged(nameof(Spielzeit));
    }

    public void SpielzeitSpeichern()
    {
        _spielService.SpielzeitSpeichern(_spielID, _spielzeit);
    }
}