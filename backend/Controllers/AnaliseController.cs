using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Globalization;
using Mscc.GenerativeAI;

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

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile arquivo, [FromForm] string contexto)
    {
        if (arquivo == null || arquivo.Length == 0)
        {
            return BadRequest("Nenhum arquivo enviado.");
        }

        try
        {
            string[] headers;
            using (var reader = new StreamReader(arquivo.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                headers = csv.HeaderRecord ?? Array.Empty<string>();
            }

            var apiKey = _configuration["Gemini:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return StatusCode(500, "Chave da API do Gemini não configurada no appsettings.");
            }

            //Criando o cliente e depois pegando o modelo.
            var googleAi = new GoogleAI(apiKey);
            var model = googleAi.GenerativeModel(Model.Gemini25Flash);

            var promptText = $$"""
                Você é um analista de dados sênior. Um usuário fez o upload de uma planilha com o seguinte objetivo em mente: '{contexto}'.
                A planilha contém as seguintes colunas: [{string.Join(", ", headers)}].
                Com base no objetivo do usuário e nas colunas disponíveis, sugira as 3 análises mais relevantes. Responda APENAS com um objeto JSON válido no formato:
                [
                  { "titulo_grafico": "...", "tipo_grafico": "barras|linha|pizza", "coluna_dimensao": "...", "coluna_metrica": "..." }
                ]
                """;

            var response = await model.GenerateContent(promptText);

            // Tratamento seguro da resposta da IA.
            var responseText = response?.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(responseText))
            {
                return StatusCode(500, "A IA não retornou uma resposta de texto válida.");
            }
            
            var jsonResponse = responseText.Trim('`').Replace("json", "").Trim();

            return Content(jsonResponse, "application/json");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocorreu um erro interno: {ex.Message}");
        }
    }
}