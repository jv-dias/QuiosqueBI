namespace QuiosqueBI.API.Models;

public class DadosCSV
{
    public record DadosCsv(string[] Headers, List<dynamic> Records);
}