using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuiosqueBI.API.Services;

namespace QuiosqueBI.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AnaliseController : ControllerBase
{
    private readonly IAnaliseService _analiseService;

    public AnaliseController(IAnaliseService analiseService)
    {
        _analiseService = analiseService;
    }
    
    // Rota para upload de arquivo e geração de resultados
    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Upload(IFormFile arquivo, [FromForm] string contexto)
    {
        if (arquivo == null || arquivo.Length == 0)
        {
            return BadRequest("Nenhum arquivo enviado.");
        }

        try
        {
            var resultadosFinais = await _analiseService.GerarResultadosAnaliseAsync(arquivo, contexto);
            return Ok(new { Resultados = resultadosFinais });
        }
        catch (Exception ex)
        {
            // O ideal é logar o ex.StackTrace, não retorná-lo para o cliente
            return StatusCode(500, $"Ocorreu um erro interno: {ex.Message}");
        }
    }
    // Rota para depuração, que retorna dados brutos e sugestões da IA
    [HttpPost("debug")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDebugData(IFormFile arquivo, [FromForm] string contexto)
    {
        if (arquivo == null || arquivo.Length == 0)
        {
            return BadRequest("Nenhum arquivo enviado.");
        }

        try
        {
            var debugData = await _analiseService.GerarDadosDebugAsync(arquivo, contexto);
            return Ok(debugData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocorreu um erro interno na rota de depuração: {ex.Message}");
        }
    }
    
    // Rota para listar análises salvas
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("historico")]
    public async Task<IActionResult> ListarHistorico()
    {
        try
        {
            var historico = await _analiseService.ListarAnalisesSalvasAsync();
            return Ok(historico);
        }
        catch (Exception ex)
        {
            // Idealmente, logar o erro
            return StatusCode(500, $"Ocorreu um erro interno: {ex.Message}");
        }
    }
    
    // Rota para obter uma análise salva por ID
    [HttpGet("historico/{id}")]
    public async Task<IActionResult> ObterHistoricoPorId(int id)
    {
        try
        {
            var analise = await _analiseService.ObterAnaliseSalvaPorIdAsync(id);

            if (analise == null)
            {
                return NotFound(); // Retorna 404 Not Found se o ID não existir
            }

            return Ok(analise);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocorreu um erro interno: {ex.Message}");
        }
    }
}