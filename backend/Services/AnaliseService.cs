using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Mscc.GenerativeAI;
using QuiosqueBI.API.Models;
using System.Globalization;
using System.Text.Json;

namespace QuiosqueBI.API.Services
{
    public class AnaliseService : IAnaliseService
    {
        private readonly IConfiguration _configuration;

        public AnaliseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<ResultadoGrafico>> GerarResultadosAnaliseAsync(IFormFile arquivo, string contexto)
        {
            var dadosCsv = await LerDadosCsvAsync(arquivo);
            var planoDeAnalise = await ObterPlanoDeAnaliseAsync(dadosCsv.Headers, contexto);
            var resultadosFinais = new List<ResultadoGrafico>();
            
            if (planoDeAnalise != null)
            {
                foreach (var analise in planoDeAnalise)
                {
                    if (analise.indice_dimensao < 0 || analise.indice_dimensao >= dadosCsv.Headers.Length ||
                        analise.indice_metrica < 0 || analise.indice_metrica >= dadosCsv.Headers.Length)
                    {
                        continue;
                    }

                    var cabecalhoDimensaoReal = dadosCsv.Headers[analise.indice_dimensao];
                    var cabecalhoMetricaReal = dadosCsv.Headers[analise.indice_metrica];

                    var dadosAgrupados = dadosCsv.Records
                        .GroupBy(rec => (object)((IDictionary<string, object>)rec)[cabecalhoDimensaoReal])
                        .Select(g => new
                        {
                            Categoria = g.Key,
                            Valor = g.Sum(rec => {
                                var stringValue = Convert.ToString(((IDictionary<string, object>)rec)[cabecalhoMetricaReal]);
                                return ConverterStringParaDecimal(stringValue);
                            })
                        })
                        .ToList();

                    // Ordenação condicional
                    if (cabecalhoDimensaoReal.ToLower().Contains("data"))
                    {
                        dadosAgrupados = dadosAgrupados.OrderBy(x => TentarConverterParaData(x.Categoria)).ToList();
                    }
                    else
                    {
                        dadosAgrupados = dadosAgrupados.OrderByDescending(x => x.Valor).ToList();
                    }

                    resultadosFinais.Add(new ResultadoGrafico
                    {
                        Titulo = analise.titulo_grafico,
                        TipoGrafico = analise.tipo_grafico,
                        Dados = dadosAgrupados
                    });
                }
            }

            return resultadosFinais;
        }

        public async Task<DebugData> GerarDadosDebugAsync(IFormFile arquivo, string contexto)
        {
            var dadosCsv = await LerDadosCsvAsync(arquivo);
            var planoDeAnalise = await ObterPlanoDeAnaliseAsync(dadosCsv.Headers, contexto);

            return new DebugData
            {
                CabecalhosDoArquivo = dadosCsv.Headers,
                DadosBrutosDoArquivo = dadosCsv.Records.Take(10),
                PlanoDaIA = planoDeAnalise
            };
        }

        // ===================================================================
        // MÉTODOS PRIVADOS REUTILIZÁVEIS
        // ===================================================================

        private async Task<DadosCSV.DadosCsv> LerDadosCsvAsync(IFormFile arquivo)
        {
            byte[] arquivoBytes;
            using (var ms = new MemoryStream())
            {
                await arquivo.CopyToAsync(ms);
                arquivoBytes = ms.ToArray();
            }

            string delimitador = ",";
            using (var previewReader = new StreamReader(new MemoryStream(arquivoBytes)))
            {
                var primeiraLinha = await previewReader.ReadLineAsync();
                if (primeiraLinha != null && primeiraLinha.Contains(";"))
                {
                    delimitador = ";";
                }
            }

            using (var reader = new StreamReader(new MemoryStream(arquivoBytes)))
            using (var csv = new CsvReader(reader, new CsvConfiguration(new CultureInfo("pt-BR"))
            {
                Delimiter = delimitador,
                Quote = '"',
                BadDataFound = null
            }))
            {
                csv.Read();
                csv.ReadHeader();
                string[] headers = csv.HeaderRecord ?? Array.Empty<string>();
                List<dynamic> records = csv.GetRecords<dynamic>().ToList();
                return new DadosCSV.DadosCsv(headers, records);
            }
        }

        private async Task<List<AnaliseSugerida>?> ObterPlanoDeAnaliseAsync(string[] headers, string contexto)
        {
            var apiKey = _configuration["Gemini:ApiKey"] ?? throw new InvalidOperationException("Chave da API do Gemini não configurada.");
            
            var googleAi = new GoogleAI(apiKey);
            var model = googleAi.GenerativeModel(Model.Gemini25Flash);

            var colunasComIndices = string.Join(", ", headers.Select((h, i) => $"'{i}': '{h}'"));
            
            var promptText = $$"""
                               Sua tarefa é agir como um motor de análise de dados. Você receberá um objetivo e uma lista de colunas com seus respectivos índices numéricos. Sua resposta DEVE usar os índices.
                               O objetivo da análise do usuário é: '{{contexto}}'.
                               A lista de colunas e seus índices disponíveis é: {{{colunasComIndices}}}.
                               Com base no objetivo e nas colunas, sugira até 5 análises relevantes de dimensão e métrica. Para cada sugestão, forneça o índice numérico da coluna de dimensão e o índice numérico da coluna de métrica.
                               Responda APENAS com um objeto JSON válido no formato:
                               [
                                 { "titulo_grafico": "...", "tipo_grafico": "barras|linha|pizza", "indice_dimensao": <numero>, "indice_metrica": <numero> }
                               ]
                               """;
            
            var response = await model.GenerateContent(promptText);
            var responseText = response?.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(responseText)) return null;

            var jsonPlanoAnalise = responseText.Trim('`').Replace("json", "").Trim();
            var opcoesJson = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<List<AnaliseSugerida>>(jsonPlanoAnalise, opcoesJson);
        }

        private decimal ConverterStringParaDecimal(string? stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return 0m;
            
            string cleanString = stringValue.Replace("R$", "").Trim().Replace(",", ".");
            
            if (decimal.TryParse(cleanString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedValue))
            {
                return parsedValue;
            }
            return 0m;
        }

        private DateTime TentarConverterParaData(object valorCategoria)
        {
            if (DateTime.TryParse(Convert.ToString(valorCategoria), new CultureInfo("pt-BR"), DateTimeStyles.None, out DateTime dt))
            {
                return dt;
            }
            return DateTime.MinValue;
        }
    }
}