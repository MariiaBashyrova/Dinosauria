using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinosauria.Domain.Models;

public class Ressource
{
    public int RessourceID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Einheit { get; set; } = string.Empty;

    public override string ToString() => Name;
    
}
