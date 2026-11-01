namespace Dinosauria.ViewModels;

public class RanglistenEintragViewModel
{
    public int Platz { get; set; }
    public string Spielername { get; set; } = "";
    public TimeSpan? Bestzeit { get; set; }
    public int AnzahlSpiele { get; set; }
}