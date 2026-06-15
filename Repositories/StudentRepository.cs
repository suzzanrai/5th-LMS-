using Microsoft.EntityFrameworkCore;
using Practice_Project.Data;
using Practice_Project.Entities;

namespace Practice_Project.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly LibraryDbContext _context;

    public StudentRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Student>> GetAllAsync(string? searchString)
    {
        var q = _context.Students.AsQueryable();
        if (!string.IsNullOrEmpty(searchString))
            q = q.Where(s =>
                s.Name.Contains(searchString) ||
                s.Email.Contains(searchString) ||
                (s.Phone != null && s.Phone.Contains(searchString)));
        return await q.OrderBy(s => s.Name).ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(int id) => await _context.Students.FindAsync(id);

    public async Task CreateAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Student student)
    {
        var existing = await _context.Students.FindAsync(student.Id);
        if (existing == null) return;

        existing.Name = student.Name;
        existing.Email = student.Email;
        existing.Phone = student.Phone;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var s = await _context.Students.FindAsync(id);
        if (s != null)
        {
            _context.Students.Remove(s);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id) => await _context.Students.AnyAsync(s => s.Id == id);

    public async Task<int> GetLatestRollNumberAsync()
    {
        var last = await _context.Students
            .OrderByDescending(s => s.Id)
            .Select(s => s.RollNumber)
            .FirstOrDefaultAsync();
        if (string.IsNullOrEmpty(last)) return 0;
        var parts = last.Split('-');
        if (parts.Length < 3 || !int.TryParse(parts[2], out var num)) return 0;
        return num;
    }
}

