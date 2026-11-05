using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinosauria.Domain.Models;

/// <summary>Stammdaten eines Spielers und seine gespeicherten Spielstände.</summary>
public class Spieler
{
    public int SpielerID { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime ErstelltAm { get; init; }
    public ICollection<Spiel> Spiele { get; } = new List<Spiel>();

    /// <summary>Verwendet den Spielernamen als Anzeigeform.</summary>
    public override string ToString() => Name;
    
}
