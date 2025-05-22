using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("schedules")]
public class ScheduleController(IScheduleService scheduleService) : Controller
{
    private readonly IScheduleService _scheduleService = scheduleService;
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ScheduleForm form)
    {
        if (!ModelState.IsValid) return BadRequest(form);
        try
        {
            var result = await _scheduleService.CreateAsync(form);
            if (result.Success == false) return StatusCode(result.StatusCode, result.ErrorMessage);
            return StatusCode(result.StatusCode, result.Data);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        try
        {
            var result = await _scheduleService.GetAsync(id);
            if (result.Success == false) return StatusCode(result.StatusCode, result.ErrorMessage);
            return StatusCode(result.StatusCode, result.Data);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var result = await _scheduleService.DeleteAsync(id);
            if (result.Success == false) return StatusCode(result.StatusCode, result.ErrorMessage);
            return NoContent();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return StatusCode(500, "Internal Server Error");
        }
    }
}
