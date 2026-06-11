using Practice_Project.Entities;
using Practice_Project.Models;
using Practice_Project.Repositories;

namespace Practice_Project.Services;

public class BookIssueServices : IBookIssueServices
{
    private readonly IBookIssueRepository _repo;
    private readonly IBookRepository _bookRepo;
    private readonly IStudentRepository _studentRepo;

    public BookIssueServices(
        IBookIssueRepository repo,
        IBookRepository bookRepo,
        IStudentRepository studentRepo)
    {
        _repo = repo;
        _bookRepo = bookRepo;
        _studentRepo = studentRepo;
    }

    public Task<List<BookIssue>> GetAllAsync(string? searchString)
        => _repo.GetAllAsync(searchString);

    public Task<BookIssue?> GetIssuedByIdAsync(int id)
        => _repo.GetIssuedByIdAsync(id);

    public Task<(bool Success, string? Error)> CreateAsync(BookIssueViewModel model)
        => _repo.IssueBookAsync(model.BookId, model.StudentId);

    public async Task<(bool Success, string? Error, decimal FineAmount, int OverdueDays, BookIssue? Issue)> ProcessReturnAsync(int id, bool confirm)
    {
        var issue = await _repo.GetByIdAsync(id);
        if (issue == null)
            return (false, "Book issue not found.", 0, 0, null);

        var result = await _repo.ReturnBookAsync(id, confirm);
        if (!result.Success && result.Error == null)
            return (false, null, result.FineAmount, result.OverdueDays, issue);

        return (result.Success, result.Error, result.FineAmount, result.OverdueDays, issue);
    }

    public async Task<List<Book>> GetAvailableBooksAsync()
    {
        var books = await _bookRepo.GetAllAsync(null);
        return books.Where(b => b.QuantityAvailable > 0).OrderBy(b => b.Title).ToList();
    }

    public async Task<List<Student>> GetStudentsAsync()
    {
        return await _studentRepo.GetAllAsync(null);
    }
}
