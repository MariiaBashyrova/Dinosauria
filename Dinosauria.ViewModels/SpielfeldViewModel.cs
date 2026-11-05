using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Dinosauria.Application.Services;
using Dinosauria.Domain.Models;
using Dinosauria.ViewModels;
using Dinosauria.ViewModels.Basic;

/// <summary>Erzeugt und aktualisiert die räumliche Darstellung von Dinosauriern und Karten.</summary>
public class SpielfeldViewModel : ViewModelBase
{
    const int BEGIN = 50;
    const int BEGINY = 100;
    const int DISTANCEX = 150;
    const int DISTANCEY = 100;
    const int OFFSET = 50;
    public ObservableCollection<DinosaurierFeldViewModel> DinosaurierListe { get; } = [];
    public ObservableCollection<KartenFeldViewModel> KartenListe { get; } = [];
    public ObservableCollection<VerbindungViewModel> Verbindungen { get; } = [];

    private readonly Dictionary<int, DinosaurierFeldViewModel> _dinosaurier = [];

    private readonly Dictionary<int, KartenFeldViewModel> _karten = [];
    
    private readonly SpielService _spielService;
    public SpielfeldViewModel(SpielService spielService)
    {
        _spielService = spielService;
    }
    /// <summary>
    /// Ordnet Dinosaurier horizontal und Karten abwechselnd oberhalb bzw. unterhalb an.
    /// Danach werden Verbindungslinien zu vorherigen Dinosauriern und zu den zugehörigen
    /// Karten erzeugt.
    /// </summary>
    public void SpielfeldErstellen(Spiel spiel)
    {
        int counter = 0; int counter2 = 0;
        // Dinosaurier erstellen
        foreach (var dino in spiel.SpielDinosaurier)
        {
            var vorherigerID = _spielService
                .VorherigenDinosaurierFinden(dino.DinosaurierartID)?.DinosaurierartID;

            var vorheriger = vorherigerID != null
                ? _dinosaurier.GetValueOrDefault(vorherigerID.Value)
                : null;

            var feldDino = new DinosaurierFeldViewModel(
                dino, vorheriger, BEGIN + counter * DISTANCEX, BEGINY);
            counter++;
            _dinosaurier.Add(dino.DinosaurierartID, feldDino);
            DinosaurierListe.Add(feldDino);
            Debug.WriteLine($"Dinosaurier {dino.DinosaurierartID} hinzugefügt: {feldDino.PositionX}, {feldDino.PositionY}");
        }

        // Karten erstellen
        foreach (var karte in spiel.SpielKarten)
        {
            double positionY = counter2 % 2 == 0 ? BEGINY + DISTANCEY : BEGINY - DISTANCEY;

            var feldKarte = new KartenFeldViewModel(
                karte,
                BEGIN + OFFSET + counter2/2 * DISTANCEX,
                positionY);

            counter2++;
            _karten.Add(karte.KarteID, feldKarte);
            KartenListe.Add(feldKarte);

            Debug.WriteLine($"Karte {karte.KarteID} hinzugefügt: {feldKarte.PositionX}, {feldKarte.PositionY}");
        }

        // Verbindungen zwischen Dinosauriern
        foreach (var dino in DinosaurierListe)
        {
            if (dino.VorherigerDinosaurier == null)
                continue;

            Verbindungen.Add(new VerbindungViewModel(dino.VorherigerDinosaurier, dino));
        }

        // Verbindungen zwischen Dinosauriern und Karten
        foreach (var karte in KartenListe)
        {
            var feldDino = _dinosaurier[karte.Karte.Karte.DinosaurierartID];

            Verbindungen.Add(new VerbindungViewModel(feldDino, karte));
           
        }
    }

    /// <summary>Aktualisiert Zustände der Knoten und anschließend alle Verbindungslinien.</summary>
    public void SpielfeldAktualisieren()
    {
        foreach (var dino in DinosaurierListe)
            dino.Aktualisieren();

        foreach (var karte in KartenListe)
            karte.Aktualisieren();

        foreach (var verbindung in Verbindungen)
            verbindung.Aktualisieren();
    }
}
