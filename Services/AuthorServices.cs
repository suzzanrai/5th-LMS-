using Practice_Project.Entities;
using Practice_Project.Models;
using Practice_Project.Repositories;

namespace Practice_Project.Services;

public class AuthorService : IAuthorServices
{
    private readonly IAuthorRepositories _repo;

    public AuthorService(IAuthorRepositories repo)
    {
        _repo = repo;
    }

    // Returns raw Entity list (Index view uses Author directly)
    public Task<List<Author>> GetAllAsync(string? searchString)
        => _repo.GetAllAsync(searchString);

    // Returns ViewModel (for Edit form)
    public async Task<AuthorViewModel?> GetByIdAsync(int id)
    {
        var author = await _repo.GetByIdAsync(id);
        if (author == null) return null;

        return new AuthorViewModel
        {
            AuthorId = author.AuthorId,
            Name = author.Name,
            Biography = author.Biography,
            Books = author.Books ?? new List<Book>()
        };
    }

    // Converts ViewModel → Entity then saves
    public Task CreateAsync(AuthorViewModel model)
    {
        var author = new Author
        {
            Name = model.Name,
            Biography = model.Biography
        };
        return _repo.CreateAsync(author);
    }

    public Task UpdateAsync(AuthorViewModel model)
    {
        var author = new Author
        {
            AuthorId = model.AuthorId,
            Name = model.Name,
            Biography = model.Biography
        };
        return _repo.UpdateAsync(author);
    }

    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

    public Task<bool> ExistsAsync(int id) => _repo.ExistsAsync(id);
}
