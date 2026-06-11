using Practice_Project.Entities;

namespace Practice_Project.Repositories;

public interface IBookIssueRepository
{
    Task<List<BookIssue>> GetAllAsync(string? searchString);
    Task<BookIssue?> GetByIdAsync(int id);
    Task<(bool Success, string? Error)> IssueBookAsync(int bookId, int studentId);
    Task<BookIssue?> GetIssuedByIdAsync(int id);
    Task<(bool Success, string? Error, decimal FineAmount, int OverdueDays)> ReturnBookAsync(int id, bool confirm);
    Task CreateAsync(BookIssue issue);
    Task UpdateAsync(BookIssue issue);
    Task DeleteAsync(int id);
}

