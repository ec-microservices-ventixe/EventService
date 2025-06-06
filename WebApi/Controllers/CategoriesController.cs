using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApi.Interfaces;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("categories")]
[Authorize(Roles ="Admin")]
public class CategoriesController(ICategoryService categoryService) : Controller
{
    private readonly ICategoryService _categoryService = categoryService;
    [HttpPost]
    public async Task<ActionResult> Create(CategoryForm form)
    {
        if (!ModelState.IsValid) return BadRequest(form);
        try
        {
            var result = await _categoryService.CreateAsync(form);
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
            var result = await _categoryService.GetAllAsync();
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
    public async Task<ActionResult> Get(int id)
    {
        try
        {
            var result = await _categoryService.GetAsync(id);
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
    public async Task<ActionResult> Update(int id, [FromBody] CategoryForm form)
    {
        try
        {
            var result = await _categoryService.UpdateAsync(id, form);
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
            var result = await _categoryService.DeleteAsync(id);
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
