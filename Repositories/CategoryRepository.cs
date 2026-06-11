using Microsoft.EntityFrameworkCore;
using Practice_Project.Data;
using Practice_Project.Entities;

namespace Practice_Project.Repositories;

public class CategoryRepository : ICategoryRepository
{
	private readonly LibraryDbContext _context;

	public CategoryRepository(LibraryDbContext context)
	{
		_context = context;
	}

	public async Task<List<Category>> GetAllAsync(string? searchString)
	{
		var query = _context.Categories.Include(c => c.Books).AsQueryable();
		if (!string.IsNullOrEmpty(searchString))
			query = query.Where(c => c.Name.Contains(searchString));
		return await query.OrderBy(c => c.Name).ToListAsync();
	}

	public async Task<Category?> GetByIdAsync(int id)
		=> await _context.Categories.Include(c => c.Books).FirstOrDefaultAsync(c => c.CategoryId == id);

	public async Task CreateAsync(Category category)
	{
		await _context.Categories.AddAsync(category);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateAsync(Category category)
	{
		var existing = await _context.Categories.FindAsync(category.CategoryId);
		if (existing == null) return;

		existing.Name = category.Name;
		existing.Description = category.Description;
		await _context.SaveChangesAsync();
	}

	public async Task DeleteAsync(int id)
	{
		var category = await _context.Categories.FindAsync(id);
		if (category != null)
		{
			_context.Categories.Remove(category);
			await _context.SaveChangesAsync();
		}
	}

	public async Task<bool> ExistsAsync(int id)
		=> await _context.Categories.AnyAsync(c => c.CategoryId == id);
}


