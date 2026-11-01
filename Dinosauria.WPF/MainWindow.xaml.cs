using System.Windows;
using Dinosauria.ViewModels;
using Dinosauria.WPF.Views;

namespace Dinosauria.WPF;

public partial class MainWindow : Window
{
    private readonly MainMenuViewModel _mainMenuViewModel;

    public MainWindow(MainMenuViewModel viewModel)
    {
        InitializeComponent();
        _mainMenuViewModel = viewModel;
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        var view = new MainMenuView(_mainMenuViewModel);
        MainContent.Content = view;
    }
}