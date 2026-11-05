
using System.Windows;
using Dinosauria.Application.Services;
using Dinosauria.ViewModels;
using Dinosauria.WPF.Views;

namespace Dinosauria.WPF;

    /// <summary>Hauptfenster, das zwischen Hauptmenü und Spielansicht navigiert.</summary>
    public partial class MainWindow : Window
{
    private readonly MainMenuViewModel _mainMenuViewModel;
    private readonly SpielService _spielService;

    public MainWindow(MainMenuViewModel viewModel, SpielService spielService)
    {
        InitializeComponent();

        _mainMenuViewModel = viewModel;
        _spielService = spielService;

        _mainMenuViewModel.BeendenRequested += Close;
        _mainMenuViewModel.SpielOeffnenRequested += ShowSpiel;
        _mainMenuViewModel.SpielLoeschenRequested += SpielLoeschen;
        ShowMainMenu();
    }

        /// <summary>Zeigt das Hauptmenü und verbindet dessen Navigationsevents.</summary>
        private void ShowMainMenu()
    {
        _mainMenuViewModel.Aktualisieren();
        MainContent.Content = new MainMenuView(_mainMenuViewModel);
    }

    private void ShowSpiel(int spielID)
    {
        var viewModel = new SpielViewModel(_spielService, spielID);
        var view = new SpielView(viewModel);

        view.HauptmenueRequested += ShowMainMenu;
        MainContent.Content = view;
    }

    private void SpielLoeschen(int spielID)
    {
        var ergebnis = MessageBox.Show(
            "Möchtest du dieses Spiel wirklich löschen?",
            "Spiel löschen",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (ergebnis != MessageBoxResult.Yes) return;

        _spielService.SpielLoeschen(spielID);
        ShowMainMenu();
    }
}
