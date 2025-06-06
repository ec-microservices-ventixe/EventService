using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("schedule-slots")]
[Authorize(Roles = "Admin")]
public class ScheduleSlotsController(IScheduleSlotService scheduleSlotService) : Controller
{
    private readonly IScheduleSlotService _scheduleSlotService = scheduleSlotService;
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ScheduleSlotForm form)
    {
        if (!ModelState.IsValid) return BadRequest(form);
        try
        {
            var result = await _scheduleSlotService.CreateAsync(form);
            if (result.Success == false) return StatusCode(result.StatusCode, result.ErrorMessage);
            return StatusCode(result.StatusCode, result.Data);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult> Get()
    {
        try
        {
            var result = await _scheduleSlotService.GetAllAsync();
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
    [AllowAnonymous]
    public async Task<ActionResult> Get([FromQuery] int id)
    {
        try
        {
            var result = await _scheduleSlotService.GetAsync(id);
            if (result.Success == false) return StatusCode(result.StatusCode, result.ErrorMessage);
            return StatusCode(result.StatusCode, result.Data);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromQuery] int id, [FromBody] ScheduleSlotForm form)
    {
        try
        {
            var result = await _scheduleSlotService.UpdateAsync(id, form);
            if (result.Success == false) return StatusCode(result.StatusCode, result.ErrorMessage);
            return NoContent();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromQuery] int id)
    {
        try
        {
            var result = await _scheduleSlotService.DeleteAsync(id);
            if (result.Success == false) return StatusCode(result.StatusCode, result.ErrorMessage);
            return StatusCode(result.StatusCode, result.Data);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return StatusCode(500, "Internal Server Error");
        }
    }
}
