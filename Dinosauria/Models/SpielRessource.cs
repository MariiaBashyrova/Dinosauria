namespace Dinosauria.Domain.Models;
public class SpielRessource
{
    public int SpielID { get; set; }
    public Spiel Spiel { get; set; } = null!;

    public int RessourceID { get; set; }
    public Ressource Ressource { get; set; } = null!;
    public decimal Bestand { get; set; }
    public decimal GesamtProduziert { get; set; }

    
}
