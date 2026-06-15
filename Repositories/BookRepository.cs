using Microsoft.EntityFrameworkCore;
using Practice_Project.Data;
using Practice_Project.Entities;

namespace Practice_Project.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllAsync(string? searchString)
    {
        var query = _context.Books.Include(b => b.Author).Include(b => b.Category).AsQueryable();
        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(b =>
                b.Title.Contains(searchString) ||
                (b.Author != null && b.Author.Name.Contains(searchString)) ||
                (b.Category != null && b.Category.Name.Contains(searchString)));
        }
        return await query.OrderBy(b => b.Title).ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
        => await _context.Books.Include(b => b.Author).Include(b => b.Category).FirstOrDefaultAsync(b => b.Id == id);

    public async Task CreateAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        var existing = await _context.Books.FindAsync(book.Id);
        if (existing == null) return;

        existing.Title = book.Title;
        existing.ISBN = book.ISBN;
        existing.TotalQuantity = book.TotalQuantity;
        existing.QuantityAvailable = book.QuantityAvailable;
        existing.PublicationDate = book.PublicationDate;
        existing.IsActive = book.IsActive;
        existing.AuthorId = book.AuthorId;
        existing.CategoryId = book.CategoryId;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Author>> GetAuthorsAsync()
        => await _context.Authors.OrderBy(a => a.Name).ToListAsync();

    public async Task<List<Category>> GetCategoriesAsync()
        => await _context.Categories.OrderBy(c => c.Name).ToListAsync();

    public async Task<bool> ExistsAsync(int id)
        => await _context.Books.AnyAsync(b => b.Id == id);
}

