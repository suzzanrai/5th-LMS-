using Practice_Project.Entities;

namespace Practice_Project.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync(string? searchString);
    Task<Category?> GetByIdAsync(int id);
    Task CreateAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

