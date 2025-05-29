using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("events")]
public class EventsController(IEventService eventService) : Controller
{
   private readonly IEventService _eventService = eventService;

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] EventForm form)
    {
        if (!ModelState.IsValid) return BadRequest(form);
        try
        {
            var result = await _eventService.CreateAsync(form);
            if(result.Success == false) return StatusCode(result.StatusCode, result.ErrorMessage);
            return StatusCode(result.StatusCode, result.Data);
        } catch(Exception ex)
        {
            Debug.WriteLine(ex);
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        try
        {
            var result = await _eventService.GetAllAsync();
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
            var result = await _eventService.GetAsync(id);
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
    public async Task<ActionResult> Update(int id, [FromBody] EventForm form)
    {
        try
        {
            var result = await _eventService.UpdateAsync(id, form);
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
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var result = await _eventService.DeleteAsync(id);
            if (result.Success == false) return StatusCode(result.StatusCode, result.ErrorMessage);
            return NoContent();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return NoContent();
        }
    }

}
