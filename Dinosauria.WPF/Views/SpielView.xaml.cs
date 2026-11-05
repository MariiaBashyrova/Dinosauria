using System;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Dinosauria.Domain.Models;
using Dinosauria.ViewModels;

namespace Dinosauria.WPF.Views;

public partial class SpielView : UserControl
{
    const int DURCHMESSER = 80;
    const int BREITE_KARTE = 80;
    const int HOEHE_KARTE = 60;
    const int ABSTAND = 40;
    public event Action? HauptmenueRequested;
    private readonly SpielViewModel _viewModel;
    private readonly DispatcherTimer _timer = new();
    private DateTime _letztesUpdate;
    public SpielView(SpielViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = viewModel;
        SpielfeldZeichnen();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        _letztesUpdate = DateTime.Now;
        _timer.Start();
        _viewModel.SpielfeldAktualisierenRequested += SpielfeldAktualisieren;

    }
    private void SpielfeldAktualisieren()
    {
        SpielfeldCanvas.Children.Clear();

        SpielfeldZeichnen();
    }
    private void Hauptmenue_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        _timer.Stop();
        _viewModel.SpielzeitSpeichern();
        HauptmenueRequested?.Invoke();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        var fokusDinosaurier = _viewModel.AktiveTabelle == 0 &&
                           DinosaurierDataGrid.IsKeyboardFocusWithin;

        var fokusKarten = _viewModel.AktiveTabelle == 1 &&
                          KartenDataGrid.IsKeyboardFocusWithin;
        var jetzt = DateTime.Now;
        var vergangeneSekunden = (jetzt - _letztesUpdate).TotalSeconds;
        _letztesUpdate = jetzt;
        _viewModel.SpielzeitAktualisieren(vergangeneSekunden);
        _viewModel.RessourcenAktualisieren(vergangeneSekunden);
        if (fokusDinosaurier || fokusKarten)
        {
            Dispatcher.BeginInvoke(() =>
            {
                if (fokusDinosaurier)
                    DinosaurierDataGrid.Focus();
                else if (fokusKarten)
                    KartenDataGrid.Focus();
            }, System.Windows.Threading.DispatcherPriority.Input);
        }
    }

    private void SpielfeldZeichnen()
    {
        SpielfeldCanvas.Children.Clear();
        SpielfeldGroesseBerechnen();
        var hintergrund = BildLaden("SpielfeldHintergrund.png", "");

        if (hintergrund != null)
        {
            SpielfeldCanvas.Children.Add(new Image
            {
                Source = hintergrund,
                Width = SpielfeldCanvas.Width,
                Height = SpielfeldCanvas.Height,
                Stretch = Stretch.Fill
            });
        }
        foreach (var verbindung in _viewModel.Spielfeld.Verbindungen)
        {
            if (!verbindung.IstFreigeschaltet)
                continue;

            var line = new Line
            {
                X1 = verbindung.StartX+ABSTAND,
                Y1 = verbindung.StartY+ABSTAND,
                X2 = verbindung.EndeX + ABSTAND,
                Y2 = verbindung.EndeY + ABSTAND,
                Stroke = Brushes.Gray,
                StrokeThickness = 2
            };

            SpielfeldCanvas.Children.Add(line);
        }

        foreach (var dinosaur in _viewModel.Spielfeld.DinosaurierListe)
        {
            if (!dinosaur.IstFreigeschaltet)
                continue;

            var feld = new Grid
            {
                Width = DURCHMESSER,
                Height = DURCHMESSER
            };

            var bild = BildLaden(dinosaur.Dinosaurier.Dinosaurierart.Bilddatei, "Dinosaurier");

            if (bild != null)
            {
                feld.Children.Add(new Image
                {
                    Source = bild,
                    Width = DURCHMESSER,
                    Height = DURCHMESSER,
                    Stretch = Stretch.UniformToFill,
                    Clip = new EllipseGeometry(new Rect(0, 0, DURCHMESSER, DURCHMESSER))
                });
            }
            else
            {
                feld.Children.Add(new Ellipse { Fill = Brushes.LightGray });
            }

            var texte = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            texte.Children.Add(new TextBlock
            {
                Text = dinosaur.Dinosaurier.Dinosaurierart.Name,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 11,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White
            });

            texte.Children.Add(new TextBlock
            {
                Text = dinosaur.Dinosaurier.Anzahl.ToString(),
                TextAlignment = TextAlignment.Center,
                FontSize = 14,
                Foreground = Brushes.White
            });

            texte.VerticalAlignment = VerticalAlignment.Bottom;
            texte.Margin = new Thickness(2, 2, 2, 8);

            foreach (TextBlock text in texte.Children)
            {
                text.Foreground = Brushes.White;
                text.Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color = Colors.Black,
                    BlurRadius = 4,
                    ShadowDepth = 1,
                    Opacity = 1
                };
            }

            feld.Children.Add(texte);

            Canvas.SetLeft(feld, dinosaur.PositionX);
            Canvas.SetTop(feld, dinosaur.PositionY);
            Debug.WriteLine($"Male Dinosaurier {dinosaur.Dinosaurier.Dinosaurierart.Name} hinzugefügt: {dinosaur.PositionX}, {dinosaur.PositionY}");
            SpielfeldCanvas.Children.Add(feld);
        }

        foreach (var karte in _viewModel.Spielfeld.KartenListe)
        {
            if (!karte.IstFreigeschaltet)
                continue;

            var feld = new Grid { Width = BREITE_KARTE, Height = HOEHE_KARTE };

            var bild = BildLaden(karte.Karte.Karte.Bilddatei, "Karten");

            if (bild != null)
            {
                feld.Children.Add(new Image
                {
                    Source = bild,
                    Width = BREITE_KARTE,
                    Height = HOEHE_KARTE,
                    Stretch = Stretch.UniformToFill
                });
            }
            else
            {
                feld.Children.Add(new Rectangle { Fill = Brushes.LightGray });
            }

            var texte = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            texte.Children.Add(new TextBlock
            {
                Text = karte.Karte.Karte.Name,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 11,
                FontWeight = FontWeights.Bold
            });

            texte.Children.Add(new TextBlock
            {
                Text = $"× {karte.Karte.Karte.Multiplikator}",
                TextAlignment = TextAlignment.Center,
                FontSize = 13
            });

            texte.VerticalAlignment = VerticalAlignment.Bottom;
            texte.Margin = new Thickness(2, 2, 2, 4);

            foreach (TextBlock text in texte.Children)
            {
                text.Foreground = Brushes.White;
                text.Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color = Colors.Black,
                    BlurRadius = 4,
                    ShadowDepth = 1,
                    Opacity = 1
                };
            }

            feld.Children.Add(texte);

            Canvas.SetLeft(feld, karte.PositionX);
            Canvas.SetTop(feld, karte.PositionY);

            SpielfeldCanvas.Children.Add(feld);
            Debug.WriteLine($"Male Karte {karte.Karte.KarteID} hinzugefügt: {karte.PositionX}, {karte.PositionY}");
        }
    }
    private void SpielfeldGroesseBerechnen()
    {
        double breite = 800;
        double hoehe = 260;

        foreach (var dinosaur in _viewModel.Spielfeld.DinosaurierListe)
        {
            breite = Math.Max(breite, dinosaur.PositionX + DURCHMESSER + ABSTAND);
            hoehe = Math.Max(hoehe, dinosaur.PositionY + DURCHMESSER + ABSTAND);
        }

        foreach (var karte in _viewModel.Spielfeld.KartenListe)
        {
            breite = Math.Max(breite, karte.PositionX + BREITE_KARTE + ABSTAND);
            hoehe = Math.Max(hoehe, karte.PositionY + HOEHE_KARTE + ABSTAND);
        }

        SpielfeldCanvas.Width = breite;
        SpielfeldCanvas.Height = hoehe;
    }

    private BitmapImage? BildLaden(string? dateiname, string ordner)
    {
        if (string.IsNullOrWhiteSpace(dateiname)) return null;

        var pfad = System.IO.Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Bilder", ordner, dateiname);

        if (!File.Exists(pfad)) return null;

        return new BitmapImage(new Uri(pfad));
    }
}
