using System;
using System.Windows.Controls;
using Dinosauria.ViewModels;

namespace Dinosauria.WPF.Views;

public partial class SpielView : UserControl
{
    public event Action? HauptmenueRequested;

    public SpielView(SpielViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void Hauptmenue_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        HauptmenueRequested?.Invoke();
    }
}