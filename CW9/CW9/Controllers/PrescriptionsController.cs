using CW9.DTOs;
using CW9.Exceptions;
using CW9.Models;
using CW9.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW9.Controllers;

[ApiController]
[Route("[controller]")]
public class PrescriptionsController(IDbService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPrescriptions()
    {
        return Ok(await service.GetPrescriptionsAsync());
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePrescription([FromBody] PrescriptionCreateDTO pr)
    {
        try
        {
            var prescription = await service.CreatePrescriptionAsync(pr);
            return CreatedAtAction(nameof(GetPrescriptions), new { id = prescription.IdPrescription }, prescription);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
}