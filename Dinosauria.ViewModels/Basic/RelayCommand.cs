using System.Windows.Input;

namespace Dinosauria.ViewModels.Basic;

/// <summary>
/// ICommand-Implementierung ohne WPF-Abhängigkeit (kein CommandManager).
/// Das ViewModel ruft <see cref="RaiseCanExecuteChanged"/> selbst auf, wenn sich
/// die Bedingung ändert. So bleibt es in Unit-Tests ohne Oberfläche verwendbar.
/// </summary>
public sealed class RelayCommand(Action ausfuehren, Func<bool>? kannAusfuehren = null) : ICommand
{
    public event EventHandler? CanExecuteChanged;

    /// <summary>Prüft, ob die Aktion aktuell ausgeführt werden darf.</summary>
    public bool CanExecute(object? parameter) => kannAusfuehren?.Invoke() ?? true;

    /// <summary>Führt die Aktion aus, wenn ihre Ausführbarkeitsbedingung erfüllt ist.</summary>
    public void Execute(object? parameter)
    {
        if (CanExecute(parameter))
            ausfuehren();
    }

    /// <summary>Benachrichtigt WPF, dass sich die Ausführbarkeitsbedingung geändert hat.</summary>
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
