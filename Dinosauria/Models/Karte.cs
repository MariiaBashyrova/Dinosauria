namespace Dinosauria.Domain.Models;

public class Karte
{
    public int KarteID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public decimal Kaufkosten { get; set; }
    public decimal Multiplikator { get; set; }
    public int KostenRessourceID { get; set; }
    public Ressource KostenRessource { get; set; } = null!;
    public int DinosaurierartID { get; set; }
    public Dinosaurierart Dinosaurierart { get; set; } = null!;
    public ICollection<Kartenbedingung> Bedingungen { get; } = new List<Kartenbedingung>();
}
