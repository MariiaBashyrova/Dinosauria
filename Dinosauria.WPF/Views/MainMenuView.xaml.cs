using System.Windows;
using System.Windows.Controls;
using Dinosauria.ViewModels;

namespace Dinosauria.WPF.Views;

public partial class MainMenuView : UserControl
{
    public MainMenuView(MainMenuViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
