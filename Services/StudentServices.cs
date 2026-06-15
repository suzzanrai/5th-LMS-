using Practice_Project.Entities;
using Practice_Project.Models;
using Practice_Project.Repositories;

namespace Practice_Project.Services;

public class StudentServices : IStudentServices
{
    private readonly IStudentRepository _repo;

    public StudentServices(IStudentRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<StudentVm>> GetAllAsync(string? searchString)
    {
        var list = await _repo.GetAllAsync(searchString);
        return list.Select(s => new StudentVm
        {
            StudentId = s.Id,
            Name = s.Name,
            Email = s.Email,
            Phone = s.Phone ?? string.Empty,
            RollNumber = s.RollNumber
        }).ToList();
    }

    public async Task<StudentVm?> GetByIdAsync(int id)
    {
        var s = await _repo.GetByIdAsync(id);
        if (s == null) return null;
        return new StudentVm { StudentId = s.Id, Name = s.Name, Email = s.Email, Phone = s.Phone ?? string.Empty, RollNumber = s.RollNumber };
    }

    public async Task CreateAsync(StudentVm model)
    {
        var latest = await _repo.GetLatestRollNumberAsync();
        var nextNumber = (latest + 1).ToString("D4");
        var year = DateTime.UtcNow.Year;

        var entity = new Student
        {
            RollNumber = $"LMS-{year}-{nextNumber}",
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone
        };
        await _repo.CreateAsync(entity);
    }

    public async Task UpdateAsync(StudentVm model)
    {
        var e = await _repo.GetByIdAsync(model.StudentId);
        if (e == null) return;
        e.Name = model.Name;
        e.Email = model.Email;
        e.Phone = model.Phone;
        await _repo.UpdateAsync(e);
    }

    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
}

