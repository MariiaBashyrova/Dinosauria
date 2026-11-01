using System.Windows;
using Dinosauria.Infrastructure.Data;
using System.IO;

namespace Dinosauria.WPF;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        using var context = DinosauriaDbContextFactory.Create();
        DatenbankInitialisierer.Initialisieren(context);
        var lader = new CsvKonfigurationLader(
       context,       Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration"));

        lader.Laden();

    }
}