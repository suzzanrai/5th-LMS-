using Practice_Project.Models;

namespace Practice_Project.Services;

public interface ICategoryServices
{
    Task<List<CategoryViewModel>> GetAllAsync(string? searchString);
    Task<CategoryViewModel?> GetByIdAsync(int id);
    Task CreateAsync(CategoryViewModel model);
    Task UpdateAsync(CategoryViewModel model);
    Task DeleteAsync(int id);
}

