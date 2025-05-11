using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApi.Data.Context;
using WebApi.Data.Entities;

namespace WebApi.Controllers;

[ApiController]
[Route("events")]
public class EventsController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Create([FromBody] EventEntity entity) {
        if(!ModelState.IsValid) return BadRequest(entity);

        try
        {
            _context.Add(entity);
            _context.SaveChanges();
            return Ok("successfully created event");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return StatusCode(500, "Failed to create event");
        }
        
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var events = await _context.Set<EventEntity>().ToListAsync();
        return Ok(events);
    }
}
