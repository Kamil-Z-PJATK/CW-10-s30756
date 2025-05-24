using APBD_CW_5.DTOs;
using APBD_CW_5.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_CW_5.Controllers;

[ApiController]
[Route("[controller]")]
public class TripsController(IDbService service):ControllerBase
{
    [HttpGet]
    [Route("/trips")]
    public async Task<IActionResult> GetTripsAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var trips = await service.GetTripsAsync(page, pageSize);
        return Ok(trips);
    }

    [HttpPost]
    [Route("{id}/client")]
    public async Task<IActionResult> AddClientToTripAsync(int id, [FromBody] ClientCreateDTO client)
    {
        try
        {
            var cl = await service.CreateClientAsync(id, client);
            return Ok(cl);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
}