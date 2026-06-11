using Practice_Project.Entities;

namespace Practice_Project.Repositories;

public interface IStudentRepository
{
    Task<List<Student>> GetAllAsync(string? searchString);
    Task<Student?> GetByIdAsync(int id);
    Task CreateAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

