using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinosauria.Domain.Models;

/// <summary>Statischer Ressourcentyp wie Fossilien.</summary>
public class Ressource
{
    public int RessourceID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Einheit { get; set; } = string.Empty;

    /// <summary>Verwendet den Ressourcennamen als Anzeigeform.</summary>
    public override string ToString() => Name;
    
}
