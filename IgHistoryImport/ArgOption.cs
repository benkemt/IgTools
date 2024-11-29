using System.Runtime.InteropServices;
using CommandLine;

namespace IgHistoryImport;

public class ArgOption
{
    [Option('e', "epic", Required = true, HelpText = "Instrument epic")]
    public string Epic { get; set; } = null!;

    [Option('s', "startDate", Required = true, HelpText = "Start date and time in the format yyyy-MM-dd")]
    public DateTime StartDate { get; set; }

    [Option('e', "endDate", Required = true, HelpText = "End date and time in the format yyyy-MM-dd")]
    public DateTime EndDate { get; set; }
}