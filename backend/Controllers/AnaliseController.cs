using Microsoft.AspNetCore.Mvc;
using QuiosqueBI.API.Services;

namespace QuiosqueBI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnaliseController : ControllerBase
{
    private readonly IAnaliseService _analiseService;

    public AnaliseController(IAnaliseService analiseService)
    {
        _analiseService = analiseService;
    }

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
}