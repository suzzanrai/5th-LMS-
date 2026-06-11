using Practice_Project.Entities;

namespace Practice_Project.Repositories;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync(string? searchString);
    Task<Book?> GetByIdAsync(int id);
    Task CreateAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(int id);
    Task<List<Author>> GetAuthorsAsync();
    Task<List<Category>> GetCategoriesAsync();
    Task<bool> ExistsAsync(int id);
}

