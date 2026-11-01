using System.Windows;
using System.Windows.Controls;

namespace Dinosauria.WPF.Views;

public partial class SpielView : UserControl
{
    public event RoutedEventHandler? HauptmenueRequested;

    public SpielView()
    {
        InitializeComponent();
    }

    private void Hauptmenue_Click(object sender, RoutedEventArgs e)
    {
        HauptmenueRequested?.Invoke(this, e);
    }
}
