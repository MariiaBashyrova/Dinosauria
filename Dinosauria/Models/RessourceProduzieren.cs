namespace Dinosauria.Domain.Models;
public class RessourceProduzieren : Aufgabenbedingung
{
    public decimal Zielmenge { get; set; }
    public int RessourceID { get; set; }
    public Ressource Ressource { get; set; } = null!;
    
}
