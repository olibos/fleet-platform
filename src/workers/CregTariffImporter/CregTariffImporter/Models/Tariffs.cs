namespace CregTariffImporter.Models;

public class Tariffs
{
    public DateOnly Month { get; set; }
    public decimal Brussels { get; set; }
    public decimal Flanders { get; set; }
    public decimal Wallonia { get; set; }
}