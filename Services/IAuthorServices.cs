using Practice_Project.Entities;
using Practice_Project.Models;

namespace Practice_Project.Services;

public interface IAuthorServices
{
    Task<List<Author>> GetAllAsync(string? searchString);
    Task<AuthorViewModel?> GetByIdAsync(int id);
    Task CreateAsync(AuthorViewModel model);
    Task UpdateAsync(AuthorViewModel model);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}