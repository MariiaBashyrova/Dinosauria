using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinosauria.Domain.Models;

public class Spiel
{
    public int SpielID { get; set; }
    public int SpielerID { get; set; }
    public Spieler Spieler { get; set; } = null!;
    public DateTime Startzeitpunkt { get; init; }
    public DateTime? Endzeitpunkt { get; set; }
    public TimeSpan? Abschlussdauer {  get; set; }
    public Spielstatus Status { get; set; }
    public int SpielzielID { get; set; }
    public Spielziel Spielziel { get; set; } = null!;
    public ICollection<SpielRessource> SpielRessourcen { get; } = new List<SpielRessource>();
    public ICollection<SpielDinosaurier> SpielDinosaurier { get; } = new List<SpielDinosaurier>();
    public ICollection<SpielKarte> SpielKarten { get; } = new List<SpielKarte>();

    public override string ToString() => $"Spiel vom {Startzeitpunkt:dd.MM.yyyy}";

}
