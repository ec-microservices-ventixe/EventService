using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("packages")]
[Authorize(Roles = "Admins")]
public class PackagesController(IPackageService packageService) : Controller
{
    private readonly IPackageService _packageService = packageService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PackageForm form)
    {
        if(!ModelState.IsValid) return BadRequest(form);
        try
        {
            var result = await _packageService.CreateAsync(form);
            if(!result.Success) return StatusCode(result.StatusCode, result.ErrorMessage);
            return Ok(result.Data);

        } catch(Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Internal Error");
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get()
    {
        try
        {
            var result = await _packageService.GetAllAsync();
            if (!result.Success) return StatusCode(result.StatusCode, result.ErrorMessage);
            return Ok(result.Data);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Internal Error");
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var result = await _packageService.GetAsync(id);
            if (!result.Success) return StatusCode(result.StatusCode, result.ErrorMessage);
            return Ok(result.Data);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Internal Error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PackageForm form)
    {
        if (!ModelState.IsValid) return BadRequest(form);
        try
        {
            var result = await _packageService.UpdateAsync(id, form);
            if (!result.Success) return StatusCode(result.StatusCode, result.ErrorMessage);
            return Ok(result.Data);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Internal Error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _packageService.DeleteAsync(id);
            if (!result.Success) return StatusCode(result.StatusCode, result.ErrorMessage);
            return Ok(result.Data);
        } catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Internal Error");
        }
    }
}
