using System.Diagnostics;
using WebApi.Data.Entities;
using WebApi.Data.Interfaces;
using WebApi.Extensions;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services;

public class CategoryService(IEventCategoryRepository categoryRepository) : ICategoryService
{
    private readonly IEventCategoryRepository _categoryRepository = categoryRepository;

    public async Task<ServiceResult<Category>> CreateAsync(CategoryForm form)
    {
        try
        {
            var exists = await _categoryRepository.GetAsync(findBy: x => x.Name == form.Name );
            if (exists is not null) return ServiceResult<Category>.BadRequest("Category already exists");
            
            var createdCategory = await _categoryRepository.CreateAsync(form.MapTo<EventCategoryEntity>());
            if(createdCategory is null) return ServiceResult<Category>.Error("Failed to create category");

            return ServiceResult<Category>.Ok(createdCategory.MapTo<Category>());
        } catch (Exception ex) 
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<Category>.Error("Failed to create category");
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            var foundEntity = await _categoryRepository.GetAsync(findBy: x => x.Id == id);
            if (foundEntity is null) return ServiceResult<bool>.BadRequest("Category not found");

            bool result = await _categoryRepository.DeleteAsync(foundEntity);
            if (result == false) return ServiceResult<bool>.Error("Failed to delete category");

            return ServiceResult<bool>.NoContent();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<bool>.Error("Failed to delete category");
        }
    }

    public async Task<ServiceResult<IEnumerable<Category>>> GetAllAsync()
    {
        try
        {
            var entities = await _categoryRepository.GetAllAsync();
            var mapped = entities.Select(x => x.MapTo<Category>()).ToList();
            return ServiceResult<IEnumerable<Category>>.Ok(mapped);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<IEnumerable<Category>>.Error("Failed to fetch categories");
        }
    }

    public async Task<ServiceResult<Category>> GetAsync(int id)
    {
        try
        {
            var foundEntity = await _categoryRepository.GetAsync(findBy: x => x.Id == id);
            if (foundEntity is null) return ServiceResult<Category>.BadRequest("Category not found");
            return ServiceResult<Category>.Ok(foundEntity.MapTo<Category>());
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<Category>.Error("Failed to fetch category");
        }
    }

    public async Task<ServiceResult<Category>> UpdateAsync(int id, CategoryForm form)
    {
        try
        {
            var existingEntity = await _categoryRepository.GetAsync(findBy: x => x.Id == id);
            if (existingEntity is null) return ServiceResult<Category>.BadRequest("Category not found");
            existingEntity.Name = form.Name;
            var UpdatedEntity = await _categoryRepository.UpdateAsync(existingEntity);
            if (UpdatedEntity is null) return ServiceResult<Category>.Error("Failed to update event");
            return ServiceResult<Category>.Ok(UpdatedEntity.MapTo<Category>());
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<Category>.Error("Failed to update category");
        }
    }
}
