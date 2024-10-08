using Microsoft.AspNetCore.Mvc;
using PicPaySimplificado.Models.Requests;
using PicPaySimplificado.Services.Transferencias;

namespace PicPaySimplificado.Controllers;

[ApiController]
[Route("transfer")]
public class TransferenciaController : ControllerBase
{
    private readonly ITransferenciaService _transferenciaService;

    public TransferenciaController(ITransferenciaService transferenciaService)
    {
        _transferenciaService = transferenciaService;
    }
    
    [HttpPost]
    public async Task<IActionResult> PostTransfer(TransferenciaRequest request)
    {

        var result = await _transferenciaService.ExecuteAsync(request);
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Ok(result);
    }
}