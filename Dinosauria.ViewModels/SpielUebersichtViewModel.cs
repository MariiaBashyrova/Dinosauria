using Dinosauria.Domain.Models;

namespace Dinosauria.ViewModels;

public class SpielUebersichtViewModel
{
    public int SpielID { get; set; }
    public DateTime Startzeitpunkt { get; set; }
    public TimeSpan? Abschlussdauer { get; set; }
    public decimal GesamtProduziert { get; set; }
    public int FreigeschalteteDinosaurier { get; set; }
    public Spielstatus Status { get; set; }
}