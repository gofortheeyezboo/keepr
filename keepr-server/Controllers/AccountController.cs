using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using keepr_server.Models;
using keepr_server.Services;

namespace keepr_server.Controllers
{
  [ApiController]
  [Route("[controller]")]
  // REVIEW[epic=Authentication] this tag enforces the user must be logged in
  [Authorize]
  public class AccountController : ControllerBase
  {
    private readonly ProfileService _ps;
    private readonly KeepService _ks;
    private readonly VaultService _vs;

    public AccountController(ProfileService ps, KeepService ks, VaultService vs)
    {
      _ps = ps;
      _ks = ks;
      _vs = vs;
    }

    [HttpGet]
    public async Task<ActionResult<Profile>> Get()
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        return Ok(_ps.GetOrCreateProfile(userInfo));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet("vaults")]
    public async Task<ActionResult<IEnumerable<Vault>>> GetVaultsByAccountId() {
      Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
      return Ok(_vs.GetVaultsByAccountId(userInfo.Id));
    }
    [HttpGet("keeps")]
    public async Task<ActionResult<IEnumerable<Keep>>> GetKeepsByAccountId() {
      Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
      return Ok(_ks.GetKeepsByAccountId(userInfo.Id));
        }
  }
}