namespace Dinosauria.Domain.Models;

public class Dinosaurierart
{
    public int DinosaurierartID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public decimal Kaufkosten { get; set; }
    public int KostenRessourceID { get; set; }
    public Ressource KostenRessource { get; set; } = null!;
    public decimal ProduktionProSekunde { get; set; }
    public int ProduktionsressourceID { get; set; }
    public Ressource Produktionsressource { get; set; } = null!;
    public Erdzeitalter Erdzeitalter { get; set; }
    public Ernaehrung Ernaehrung { get; set; }
    public Lebensraum Lebensraum { get; set; }
    public ICollection<Karte> Karten { get; } = new List<Karte>();
    public ICollection<Dinosaurierbedingung> Bedingungen { get; } = new List<Dinosaurierbedingung>();
}
