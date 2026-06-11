using Practice_Project.Entities;
using Practice_Project.Models;
using Practice_Project.Repositories;

namespace Practice_Project.Services;

public class CategoryServices : ICategoryServices
{
    private readonly ICategoryRepository _repo;

    public CategoryServices(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<CategoryViewModel>> GetAllAsync(string? searchString)
    {
        var list = await _repo.GetAllAsync(searchString);
        return list.Select(c => new CategoryViewModel
        {
            CategoryId = c.CategoryId,
            Name = c.Name,
            Description = c.Description,
            BooksCount = c.Books?.Count ?? 0
        }).ToList();
    }

    public async Task<CategoryViewModel?> GetByIdAsync(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c == null) return null;
        return new CategoryViewModel
        {
            CategoryId = c.CategoryId,
            Name = c.Name,
            Description = c.Description,
            BooksCount = c.Books?.Count ?? 0
        };
    }

    public async Task CreateAsync(CategoryViewModel model)
    {
        var entity = new Category { Name = model.Name, Description = model.Description };
        await _repo.CreateAsync(entity);
    }

    public async Task UpdateAsync(CategoryViewModel model)
    {
        var e = await _repo.GetByIdAsync(model.CategoryId);
        if (e == null) return;
        e.Name = model.Name;
        e.Description = model.Description;
        await _repo.UpdateAsync(e);
    }

    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
}

