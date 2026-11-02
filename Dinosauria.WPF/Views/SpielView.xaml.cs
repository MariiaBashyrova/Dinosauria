using System;
using System.Windows.Controls;
using System.Windows.Threading;
using Dinosauria.ViewModels;

namespace Dinosauria.WPF.Views;

public partial class SpielView : UserControl
{
    public event Action? HauptmenueRequested;
    private readonly SpielViewModel _viewModel;
    private readonly DispatcherTimer _timer = new();
    private DateTime _letztesUpdate;
    public SpielView(SpielViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = viewModel;

        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        _letztesUpdate = DateTime.Now;
        _timer.Start();
    }

    private void Hauptmenue_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        _timer.Stop();
        _viewModel.SpielzeitSpeichern();
        HauptmenueRequested?.Invoke();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        var jetzt = DateTime.Now;
        var vergangeneSekunden = (jetzt - _letztesUpdate).TotalSeconds;
        _letztesUpdate = jetzt;
        _viewModel.SpielzeitAktualisieren(vergangeneSekunden);
        _viewModel.RessourcenAktualisieren(vergangeneSekunden);
    }
}