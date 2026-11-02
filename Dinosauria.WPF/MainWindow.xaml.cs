
using System.Windows;
using Dinosauria.Application.Services;
using Dinosauria.ViewModels;
using Dinosauria.WPF.Views;
using Dinosauria.Application.Services;

namespace Dinosauria.WPF;

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

        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        MainContent.Content = new MainMenuView(_mainMenuViewModel);
    }

    private void ShowSpiel(int spielID)
    {
        var viewModel = new SpielViewModel(_spielService, spielID);
        var view = new SpielView(viewModel);

        view.HauptmenueRequested += ShowMainMenu;
        MainContent.Content = view;
    }
}
