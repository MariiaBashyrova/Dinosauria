using System.IO;
using System.Windows;
using Dinosauria.Infrastructure.Data;
using Dinosauria.Application.Services;
using Dinosauria.ViewModels;

namespace Dinosauria.WPF;

public partial class App : System.Windows.Application
{
    private DinosauriaDbContext? _context;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _context = DinosauriaDbContextFactory.Create();
        DatenbankInitialisierer.Initialisieren(_context);

        var lader = new CsvKonfigurationLader(
            _context,
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration"));

        lader.Laden();

        var spielRepository = new SpielRepository(_context);
        var spielerRepository = new SpielerRepository(_context);
        var ressourceRepository = new RessourceRepository(_context);
        var dinosaurierRepository = new DinosaurierartRepository(_context);
        var karteRepository = new KarteRepository(_context);
        var spielzielRepository = new SpielzielRepository(_context);
        var systemKonfiguration = new SystemKonfigurationRepository(_context);


        var spielService = new SpielService(
            spielRepository,
            spielerRepository,
            ressourceRepository,
            dinosaurierRepository,
            karteRepository,
            spielzielRepository, 
            systemKonfiguration);

        var mainMenuViewModel = new MainMenuViewModel(spielService);

        var mainWindow = new MainWindow(mainMenuViewModel, spielService);
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _context?.Dispose();
        base.OnExit(e);
    }
}