using Microsoft.EntityFrameworkCore;
using Practice_Project.Data;
using Practice_Project.Entities;

namespace Practice_Project.Repositories;

public class AuthorRepository : IAuthorRepositories
{
    private readonly LibraryDbContext _context;

    public AuthorRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Author>> GetAllAsync(string? searchString)
    {
        var query = _context.Authors.Include(a => a.Books).AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
            query = query.Where(a => a.Name.ToLower().Contains(searchString.ToLower()));

        return await query.OrderBy(a => a.Name).ToListAsync();
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _context.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.AuthorId == id);
    }

    public async Task CreateAsync(Author author)
    {
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Author author)
    {
        var existing = await _context.Authors.FindAsync(author.AuthorId);
        if (existing == null) return;

        existing.Name = author.Name;
        existing.Biography = author.Biography;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author != null)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Authors.AnyAsync(a => a.AuthorId == id);
    }
}