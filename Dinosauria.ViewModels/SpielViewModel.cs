using System.Collections.ObjectModel;
using Dinosauria.Application.Services;
using Dinosauria.Domain.Models;
using Dinosauria.ViewModels.Basic;

namespace Dinosauria.ViewModels;

public class SpielViewModel : AnsichtViewModel
{
    private readonly SpielService _spielService;
    private readonly int _spielID;
    private Spiel? _spiel;

    public override string Titel => "Spielübersicht";

    public string Spielername => _spiel?.Spieler.Name ?? "";
    public decimal Fossilien => _spiel?.SpielRessourcen.FirstOrDefault()?.Bestand ?? 0;
    public string SpielzielName => _spiel?.Spielziel.Name ?? "";

    public ObservableCollection<SpielDinosaurier> DinosaurierListe { get; } = [];
    public ObservableCollection<SpielKarte> KartenListe { get; } = [];

    public SpielViewModel(SpielService spielService, int spielID)
    {
        _spielService = spielService;
        _spielID = spielID;
        Aktualisieren();
    }

    public override void Aktualisieren()
    {
        _spiel = _spielService.SpielLaden(_spielID);

        DinosaurierListe.Clear();
        KartenListe.Clear();

        foreach (var dinosaurier in _spiel.SpielDinosaurier)
            DinosaurierListe.Add(dinosaurier);

        foreach (var karte in _spiel.SpielKarten)
            KartenListe.Add(karte);

        OnPropertyChanged(nameof(Spielername));
        OnPropertyChanged(nameof(Fossilien));
        OnPropertyChanged(nameof(SpielzielName));
    }
}