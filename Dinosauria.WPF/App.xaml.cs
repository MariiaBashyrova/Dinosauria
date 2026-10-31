using System.Windows;
using Dinosauria.Infrastructure.Data;

namespace Dinosauria.WPF;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        using var context = DinosauriaDbContextFactory.Create();
        DatenbankInitialisierer.Initialisieren(context);


    }
}