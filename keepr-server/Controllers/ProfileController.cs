using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using keepr_server.Models;
using keepr_server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace keepr_server.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProfilesController : ControllerBase
  {
    private readonly ProfileService _service;
    private readonly KeepService _kservice;
    private readonly VaultService _vservice;

    public ProfilesController(ProfileService service, KeepService kservice, VaultService vservice)
    {
      _service = service;
      _kservice = kservice;
      _vservice = vservice;

    }

    [HttpGet("{id}")]
    public ActionResult<Profile> Get(string id)
    {
      try
      {
        Profile profile = _service.GetProfileById(id);
        return Ok(profile);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet("{id}/keeps")]
    public ActionResult<IEnumerable<KeepVaultViewModel>> GetKeeps(string id)
    {
      try
      {
        return Ok(_kservice.GetKeepsByProfileId(id));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
        [HttpGet("{id}/vaults")]
    public async Task<ActionResult<IEnumerable<VaultKeepViewModel>>> GetVaultsAsync(string id)
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        return Ok(_vservice.GetVaultsByProfileId(id));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}
