namespace Dinosauria.Domain.Models;
/// <summary>Basistyp für Bedingungen, die Freischaltungen oder Spielziele prüfen.</summary>
public abstract class Aufgabenbedingung
{
    public int AufgabenbedingungID { get; set; }
    public string Beschreibung { get; set; } = string.Empty;
   
}
