using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dinosauria.ViewModels.Basic;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? eigenschaft = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(eigenschaft));

    /// <returns>true, wenn sich der Wert geändert hat.</returns>
    protected bool SetProperty<T>(ref T feld, T wert, [CallerMemberName] string? eigenschaft = null)
    {
        if (EqualityComparer<T>.Default.Equals(feld, wert))
            return false;
        feld = wert;
        OnPropertyChanged(eigenschaft);
        return true;
    }
}

/// <summary>Eine Ansicht, die im Hauptfenster angezeigt werden kann.</summary>
public abstract class AnsichtViewModel : ViewModelBase
{
    public abstract string Titel { get; }

    /// <summary>Wird aufgerufen, bevor die Ansicht angezeigt wird.</summary>
    public abstract void Aktualisieren();
}