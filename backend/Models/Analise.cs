namespace QuiosqueBI.API.Models;

// Representa a sugestão de análise que vem da IA, agora com índices
public class AnaliseSugerida
{
    public string? titulo_grafico { get; set; }
    public string? tipo_grafico { get; set; }
    public int indice_dimensao { get; set; } // MUDOU DE STRING PARA INT
    public int indice_metrica { get; set; }  // MUDOU DE STRING PARA INT
}

// Representa o resultado final que enviaremos para o front-end
public class ResultadoGrafico
{
    public string? Titulo { get; set; }
    public string? TipoGrafico { get; set; }
    public IEnumerable<object>? Dados { get; set; }
}

public class DebugData
{
    public IEnumerable<string>? CabecalhosDoArquivo { get; set; }
    public IEnumerable<object>? DadosBrutosDoArquivo { get; set; } // As 10 primeiras linhas que você já está enviando
    public IEnumerable<AnaliseSugerida>? PlanoDaIA { get; set; }
}