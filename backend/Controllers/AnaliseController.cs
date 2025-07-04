using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Globalization;
using Mscc.GenerativeAI;
using System.Text.Json;
using QuiosqueBI.API.Models;
using System.Text.RegularExpressions;

namespace QuiosqueBI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnaliseController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AnaliseController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // Rota original para upload e análise (agora sem o DebugInfo direto na resposta principal)
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile arquivo, [FromForm] string contexto)
    {
        if (arquivo == null || arquivo.Length == 0)
        {
            return BadRequest("Nenhum arquivo enviado.");
        }

        try
        {
            List<dynamic> records;
            string[] headers;
            // Lê o arquivo inteiro em memória para evitar problemas de stream já lido
            byte[] arquivoBytes;
            using (var ms = new MemoryStream())
            {
                await arquivo.CopyToAsync(ms);
                arquivoBytes = ms.ToArray();
            }

            // Detecta delimitador automaticamente
            string delimitador = ",";
            using (var previewStream = new MemoryStream(arquivoBytes))
            using (var previewReader = new StreamReader(previewStream, leaveOpen: true))
            {
                var primeiraLinha = previewReader.ReadLine();
                if (primeiraLinha != null && primeiraLinha.Contains(";"))
                {
                    delimitador = ";";
                }
            }
            using (var csvStream = new MemoryStream(arquivoBytes))
            using (var reader = new StreamReader(csvStream))
            using (var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(new CultureInfo("pt-BR"))
            {
                Delimiter = delimitador,
                Quote = '"',
                BadDataFound = null // Ignora linhas mal formatadas
            }))
            {
                csv.Read();
                csv.ReadHeader();
                headers = csv.HeaderRecord ?? Array.Empty<string>();
                records = csv.GetRecords<dynamic>().ToList();
            }

            var apiKey = _configuration["Gemini:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return StatusCode(500, "Chave da API do Gemini não configurada.");
            }

            var googleAi = new GoogleAI(apiKey);
            var model = googleAi.GenerativeModel(Model.Gemini25Flash);

            var colunasComIndices = string.Join(", ", headers.Select((h, i) => $"'{i}': '{h}'"));
            
            var promptText = $$"""
                Sua tarefa é agir como um motor de análise de dados. Você receberá um objetivo e uma lista de colunas com seus respectivos índices numéricos. Sua resposta DEVE usar os índices.
                O objetivo da análise do usuário é: '{contexto}'.
                A lista de colunas e seus índices disponíveis é: {{{colunasComIndices}}}.
                Com base no objetivo e nas colunas, sugira até 3 análises relevantes de dimensão e métrica. Para cada sugestão, forneça o índice numérico da coluna de dimensão e o índice numérico da coluna de métrica.
                Responda APENAS com um objeto JSON válido no formato:
                [
                  { "titulo_grafico": "...", "tipo_grafico": "barras|linha|pizza", "indice_dimensao": <numero>, "indice_metrica": <numero> }
                ]
                """;
            
            var response = await model.GenerateContent(promptText);
            var responseText = response?.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(responseText))
            {
                return StatusCode(500, "A IA não retornou uma resposta de texto válida.");
            }
            
            var jsonPlanoAnalise = responseText.Trim('`').Replace("json", "").Trim();
            
            var opcoesJson = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var planoDeAnalise = JsonSerializer.Deserialize<List<AnaliseSugerida>>(jsonPlanoAnalise, opcoesJson);

            var resultadosFinais = new List<ResultadoGrafico>();

            if (planoDeAnalise != null)
            {
                foreach (var analise in planoDeAnalise)
                {
                    if (analise.indice_dimensao < 0 || analise.indice_dimensao >= headers.Length ||
                        analise.indice_metrica < 0 || analise.indice_metrica >= headers.Length)
                    {
                        continue;
                    }

                    var cabecalhoDimensaoReal = headers[analise.indice_dimensao];
                    var cabecalhoMetricaReal = headers[analise.indice_metrica];

                    // AnaliseController.cs, dentro do Select para o cálculo do Valor
                    // ===================================================================
                    // NOVA CORREÇÃO: Lógica de conversão de string para decimal
                    // ===================================================================
                    var dadosAgrupados = records
                        .GroupBy(rec => (object)((IDictionary<string, object>)rec)[cabecalhoDimensaoReal])
                        .Select(g => new
                        {
                            Categoria = g.Key,
                            Valor = g.Sum(rec => {
                                var stringValue = Convert.ToString(((IDictionary<string, object>)rec)[cabecalhoMetricaReal]);
                                decimal parsedValue = 0m; // Inicializa com 0
            
                                if (!string.IsNullOrEmpty(stringValue))
                                {
                                    // 1. Remover "R$" se presente
                                    string cleanString = stringValue.Replace("R$", "").Trim();
                
                                    // 2. Substituir vírgula por ponto para CultureInfo.InvariantCulture
                                    cleanString = cleanString.Replace(",", ".");

                                    // 3. Tentar fazer o parse com InvariantCulture e NumberStyles.Any
                                    // NumberStyles.Any permite tratar espaços em branco, símbolos monetários (já removemos o R$), etc.
                                    if (!decimal.TryParse(cleanString, NumberStyles.Any, CultureInfo.InvariantCulture, out parsedValue))
                                    {
                                        // Se a primeira tentativa falhar, podemos adicionar um log aqui para depuração
                                        // Console.WriteLine($"Falha ao converter '{stringValue}' para decimal.");
                                    }
                                }
                                return parsedValue; // Retorna o valor parseado (ou 0 se a conversão falhou)
                            })
                        })
                        .OrderByDescending(x => x.Valor)
                        .ToList();

                    resultadosFinais.Add(new ResultadoGrafico
                    {
                        Titulo = analise.titulo_grafico,
                        TipoGrafico = analise.tipo_grafico,
                        Dados = dadosAgrupados
                    });
                }
            }
            
            // Retorna apenas os resultados dos gráficos
            return Ok(new
            {
                Resultados = resultadosFinais
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocorreu um erro interno: {ex.Message}\nStackTrace: {ex.StackTrace}");
        }
    }

    // NOVA ROTA PARA DADOS DE DEPURACAO
    [HttpPost("debug")]
    public async Task<IActionResult> GetDebugData(IFormFile arquivo, [FromForm] string contexto)
    {
        if (arquivo == null || arquivo.Length == 0)
        {
            return BadRequest("Nenhum arquivo enviado.");
        }

        try
        {
            List<dynamic> records;
            string[] headers;
            // Lê o arquivo inteiro em memória para evitar problemas de stream já lido
            byte[] arquivoBytes;
            using (var ms = new MemoryStream())
            {
                await arquivo.CopyToAsync(ms);
                arquivoBytes = ms.ToArray();
            }

            // Detecta delimitador automaticamente
            string delimitador = ",";
            using (var previewStream = new MemoryStream(arquivoBytes))
            using (var previewReader = new StreamReader(previewStream, leaveOpen: true))
            {
                var primeiraLinha = previewReader.ReadLine();
                if (primeiraLinha != null && primeiraLinha.Contains(";"))
                {
                    delimitador = ";";
                }
            }
            using (var csvStream = new MemoryStream(arquivoBytes))
            using (var reader = new StreamReader(csvStream))
            using (var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(new CultureInfo("pt-BR"))
            {
                Delimiter = delimitador,
                Quote = '"',
                BadDataFound = null // Ignora linhas mal formatadas
            }))
            {
                csv.Read();
                csv.ReadHeader();
                headers = csv.HeaderRecord ?? Array.Empty<string>();
                records = csv.GetRecords<dynamic>().ToList();
            }

            var apiKey = _configuration["Gemini:ApiKey"];
            // Não é estritamente necessário para depuração de dados brutos, mas para consistência
            if (string.IsNullOrEmpty(apiKey))
            {
                return StatusCode(500, "Chave da API do Gemini não configurada.");
            }

            var googleAi = new GoogleAI(apiKey);
            var model = googleAi.GenerativeModel(Model.Gemini25Flash);

            var colunasComIndices = string.Join(", ", headers.Select((h, i) => $"'{i}': '{h}'"));
            
            // O mesmo prompt para obter o plano de análise da IA
            var promptText = $$"""
                Sua tarefa é agir como um motor de análise de dados. Você receberá um objetivo e uma lista de colunas com seus respectivos índices numéricos. Sua resposta DEVE usar os índices.
                O objetivo da análise do usuário é: '{contexto}'.
                A lista de colunas e seus índices disponíveis é: {{{colunasComIndices}}}.
                Com base no objetivo e nas colunas, sugira até 3 análises relevantes de dimensão e métrica. Para cada sugestão, forneça o índice numérico da coluna de dimensão e o índice numérico da coluna de métrica.
                Responda APENAS com um objeto JSON válido no formato:
                [
                  { "titulo_grafico": "...", "tipo_grafico": "barras|linha|pizza", "indice_dimensao": <numero>, "indice_metrica": <numero> }
                ]
                """;
            
            var response = await model.GenerateContent(promptText);
            var responseText = response?.Text?.Trim() ?? string.Empty;
            
            List<AnaliseSugerida>? planoDeAnalise = null;
            if (!string.IsNullOrEmpty(responseText))
            {
                var jsonPlanoAnalise = responseText.Trim('`').Replace("json", "").Trim();
                var opcoesJson = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                planoDeAnalise = JsonSerializer.Deserialize<List<AnaliseSugerida>>(jsonPlanoAnalise, opcoesJson);
            }

            var debugData = new DebugData
            {
                CabecalhosDoArquivo = headers,
                DadosBrutosDoArquivo = records.Take(10), // Limita a 10 para não sobrecarregar
                PlanoDaIA = planoDeAnalise
            };

            return Ok(debugData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocorreu um erro interno na rota de depuração: {ex.Message}\nStackTrace: {ex.StackTrace}");
        }
    }
}