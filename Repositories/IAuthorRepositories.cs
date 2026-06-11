using Practice_Project.Entities;

namespace Practice_Project.Repositories;

public interface IAuthorRepositories
{
    Task<List<Author>> GetAllAsync(string? searchString);
    Task<Author?> GetByIdAsync(int id);
    Task CreateAsync(Author author);
    Task UpdateAsync(Author author);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}