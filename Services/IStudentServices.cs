using Practice_Project.Models;

namespace Practice_Project.Services;

public interface IStudentServices
{
    Task<List<StudentVm>> GetAllAsync(string? searchString);
    Task<StudentVm?> GetByIdAsync(int id);
    Task CreateAsync(StudentVm model);
    Task UpdateAsync(StudentVm model);
    Task DeleteAsync(int id);
}

