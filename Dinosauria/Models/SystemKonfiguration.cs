using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinosauria.Domain.Models;

public class SystemKonfiguration
{
    public int ID { get; set; }

    public int SpielzielID { get; set; }
    public Spielziel Spielziel { get; set; } = null!;

    public int RessourceID { get; set; }
    public Ressource Ressource { get; set; } = null!;

    public int DinosaurierartID { get; set; }
    public Dinosaurierart ErsterDinosaurier { get; set; } = null!;

    public decimal Startmenge { get; set; }
}
