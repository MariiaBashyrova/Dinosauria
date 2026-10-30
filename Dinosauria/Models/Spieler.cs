using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinosauria.Domain.Models;

public class Spieler
{
    public int SpielerID { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime ErstelltAm { get; init; }
    public ICollection<Spiel> Spiele { get; } = new List<Spiel>();

    public override string ToString() => Name;
    
}
