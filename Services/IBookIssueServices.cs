using Practice_Project.Entities;
using Practice_Project.Models;

namespace Practice_Project.Services;

public interface IBookIssueServices
{
    Task<List<BookIssue>> GetAllAsync(string? searchString);
    Task<BookIssue?> GetIssuedByIdAsync(int id);
    Task<(bool Success, string? Error)> CreateAsync(BookIssueViewModel model);
    Task<(bool Success, string? Error, decimal FineAmount, int OverdueDays, BookIssue? Issue)> ProcessReturnAsync(int id, bool confirm);
    Task<List<Book>> GetAvailableBooksAsync();
    Task<List<Student>> GetStudentsAsync();
    Task<Student?> GetStudentByRollNumberAsync(string rollNumber);
}
