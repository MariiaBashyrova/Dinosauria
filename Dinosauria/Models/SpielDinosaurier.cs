using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinosauria.Domain.Models;

public class SpielDinosaurier
{ 
    public int SpielID { get; set; }
    public Spiel Spiel { get; set; } = null!;
    public int DinosaurierartID { get; set; }
    public Dinosaurierart Dinosaurierart { get; set; } = null!;
    public int Anzahl { get; set; }
    public bool IstFreigeschaltet { get; set; }
   
}
