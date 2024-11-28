namespace Ig.Api.Client.Model;

public class Price
{
    public decimal Ask { get; set; }
    public decimal Bid { get; set; }
    public decimal? LastTraded { get; set; }
}



public class PriceSnapshot
{
    public Price ClosePrice { get; set; } = null!;
    public Price HighPrice { get; set; } = null!;
    public decimal LastTradedVolume { get; set; }
    
    public Price LowPrice { get; set; } = null!;
    public Price OpenPrice { get; set; } = null!;
    public string SnapshotTime { get; set; } = null!;
}

public class PriceList
{
    public List<PriceSnapshot> Prices { get; set; } = null!;

    public string InstrumentType { get; set; } = null!;

    public Allowance Allowance { get; set; } = null!;
}