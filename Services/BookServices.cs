using Practice_Project.Entities;
using Practice_Project.Models;
using Practice_Project.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Practice_Project.Services;

public class BookServices : IBookServices
{
    private readonly IBookRepository _repo;

    public BookServices(IBookRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<BookViewModel>> GetAllAsync(string? searchString)
    {
        var books = await _repo.GetAllAsync(searchString);
        return books.Select(b => new BookViewModel
        {
            BookId = b.Id,
            Title = b.Title,
            ISBN = b.ISBN,
            PublicationYear = b.PublicationYear,
            TotalQuantity = b.TotalQuantity,
            QuantityAvailable = b.QuantityAvailable,
            PublicationDate = b.PublicationDate,
            IsActive = b.IsActive,
            AuthorId = b.AuthorId,
            AuthorName = b.Author?.Name,
            CategoryId = b.CategoryId,
            CategoryName = b.Category?.Name
        }).ToList();
    }

    public async Task<BookViewModel?> GetByIdAsync(int id)
    {
        var b = await _repo.GetByIdAsync(id);
        if (b == null) return null;

        return new BookViewModel
        {
            BookId = b.Id,
            Title = b.Title,
            ISBN = b.ISBN,
            PublicationYear = b.PublicationYear,
            TotalQuantity = b.TotalQuantity,
            QuantityAvailable = b.QuantityAvailable,
            PublicationDate = b.PublicationDate,
            IsActive = b.IsActive,
            AuthorId = b.AuthorId,
            AuthorName = b.Author?.Name,
            CategoryId = b.CategoryId,
            CategoryName = b.Category?.Name
        };
    }

    public async Task CreateAsync(BookViewModel model)
    {
        var quantityAvailable = model.QuantityAvailable > 0
            ? model.QuantityAvailable
            : model.TotalQuantity;

        var book = new Book
        {
            Title = model.Title,
            ISBN = model.ISBN,
            PublicationYear = model.PublicationYear,
            TotalQuantity = model.TotalQuantity,
            QuantityAvailable = quantityAvailable,
            PublicationDate = model.PublicationDate.HasValue ? DateTime.SpecifyKind(model.PublicationDate.Value, DateTimeKind.Utc) : null,
            IsActive = model.IsActive,
            AuthorId = model.AuthorId,
            CategoryId = model.CategoryId
        };

        await _repo.CreateAsync(book);
    }

    public async Task UpdateAsync(BookViewModel model)
    {
        var existing = await _repo.GetByIdAsync(model.BookId);
        if (existing == null) return;

        existing.Title = model.Title;
        existing.ISBN = model.ISBN;
        existing.PublicationYear = model.PublicationYear;
        existing.TotalQuantity = model.TotalQuantity;
        existing.QuantityAvailable = model.QuantityAvailable;
        existing.PublicationDate = model.PublicationDate.HasValue ? DateTime.SpecifyKind(model.PublicationDate.Value, DateTimeKind.Utc) : null;
        existing.IsActive = model.IsActive;
        existing.AuthorId = model.AuthorId;
        existing.CategoryId = model.CategoryId;

        await _repo.UpdateAsync(existing);
    }

    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

    public async Task PopulateDropdownsAsync(BookViewModel model)
    {
        var authors = await _repo.GetAuthorsAsync();
        var categories = await _repo.GetCategoriesAsync();

        model.Authors = authors.Select(a => new SelectListItem { Value = a.AuthorId.ToString(), Text = a.Name, Selected = a.AuthorId == model.AuthorId });
        model.Categories = categories.Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name, Selected = c.CategoryId == model.CategoryId });
    }
}

