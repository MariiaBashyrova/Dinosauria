using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Dinosauria.ViewModels;

namespace Dinosauria.WPF.Views;

/// <summary>Visuelle Oberfläche des Hauptmenüs.</summary>
public partial class MainMenuView : UserControl
{
    public MainMenuView(MainMenuViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Dispatcher.BeginInvoke(() =>
        {
            if (viewModel.AusgewaehlterSpieler != null)
            {
                SpielerListBox.Focus();
                SpielerListBox.ScrollIntoView(viewModel.AusgewaehlterSpieler);
            }
        }, DispatcherPriority.Input);
    }
}
