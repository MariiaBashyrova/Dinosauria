using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinosauria.Domain.Models;

public class Spielziel
{
    public int SpielzielID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public ICollection<Spielbedingung> Bedingungen { get; } = new List<Spielbedingung>();
        
    public override string ToString() => Beschreibung;
    
}
