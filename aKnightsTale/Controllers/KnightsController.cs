using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using aKnightsTale.Models;
using aKnightsTale.Services;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aKnightsTale.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class KnightsController : ControllerBase
  {
    private readonly KnightsService _kniServ;

    public KnightsController(KnightsService kniServ)
    {
      _kniServ = kniServ;
    }

    [HttpGet]
    public ActionResult<List<Knight>> Get()
    {
      try
      {
        List<Knight> knights = _kniServ.Get();
        return Ok(knights);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpGet("{id}")]
    public ActionResult<Knight> Get(int id)
    {
      try
      {
        Knight knight = _kniServ.Get(id);
        return Ok(knight);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Knight>> Create([FromBody] Knight knightData)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        knightData.CreatorId = userInfo.Id;
        Knight newKnight = _kniServ.Create(knightData);
        newKnight.Creator = userInfo;
        return Ok(newKnight);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<Knight>> EditAsync(int id, [FromBody] Knight knightData)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        knightData.Id = id;
        knightData.CreatorId = userInfo.Id;
        Knight update = _kniServ.Edit(knightData);
        return Ok(update);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<Knight>> DeleteAsync(int id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        Knight deletedKnight = _kniServ.Delete(id, userInfo.Id);
        return Ok(deletedKnight);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}