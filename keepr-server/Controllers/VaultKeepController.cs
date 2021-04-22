using System.Threading.Tasks;
using keepr_server.Models;
using keepr_server.Services;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace keepr_server.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class VaultKeepsController : ControllerBase
  {
    private readonly VaultKeepService _service;

    public VaultKeepsController(VaultKeepService service)
    {
      _service = service;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<VaultKeep>> CreateAsync([FromBody] VaultKeep newVaultKeep)
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        newVaultKeep.CreatorId = userInfo.Id;
        return Ok(_service.Create(newVaultKeep));
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> DeleteAsync(int id)
    {
      try
      {
        Profile userData = await HttpContext.GetUserInfoAsync<Profile>();
        _service.Delete(id, userData.Id);
        return Ok("deleted");
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }
  }
}