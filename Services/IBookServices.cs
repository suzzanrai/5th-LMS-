using Practice_Project.Models;

namespace Practice_Project.Services;

public interface IBookServices
{
    Task<List<BookViewModel>> GetAllAsync(string? searchString);
    Task<BookViewModel?> GetByIdAsync(int id);
    Task CreateAsync(BookViewModel model);
    Task UpdateAsync(BookViewModel model);
    Task DeleteAsync(int id);
    Task PopulateDropdownsAsync(BookViewModel model);
}

