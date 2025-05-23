using CW9.DTOs;
using CW9.Exceptions;
using CW9.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW9.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientsController(IDbService service) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientInfoById(int id)
    {
        try
        {
            return Ok(await service.GetPatientInfoByIdAsync(id));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}