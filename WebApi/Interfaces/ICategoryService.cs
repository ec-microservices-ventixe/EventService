﻿using WebApi.Models;

namespace WebApi.Interfaces;

public interface ICategoryService
{
    public Task<ServiceResult<Category>> CreateAsync(CategoryForm form);

    public Task<ServiceResult<Category>> GetAsync(int id);

    public Task<ServiceResult<IEnumerable<Category>>> GetAllAsync();

    public Task<ServiceResult<Category>> UpdateAsync(int id, CategoryForm form);

    public Task<ServiceResult<bool>> DeleteAsync(int id);
}


