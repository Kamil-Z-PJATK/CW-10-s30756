using APBD_CW_5.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_CW_5.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientsController(IDbService service): ControllerBase
{
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        try
        {
            await service.DeleteClientAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}