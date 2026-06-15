using Microsoft.EntityFrameworkCore;
using Practice_Project.Data;
using Practice_Project.Entities;

namespace Practice_Project.Repositories;

public class BookIssueRepository : IBookIssueRepository
{
    private readonly LibraryDbContext _context;

    public BookIssueRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookIssue>> GetAllAsync(string? searchString)
    {
        var q = _context.BookIssues
            .Include(bi => bi.Book)
                .ThenInclude(b => b.Author)
            .Include(bi => bi.Student)
            .Include(bi => bi.Fines)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            var term = searchString.ToLower();
            q = q.Where(bi =>
                bi.Student.RollNumber.ToLower().Contains(term) ||
                bi.Book.Title.ToLower().Contains(term));
        }

        return await q.OrderByDescending(bi => bi.IssueDate).ToListAsync();
    }

    public async Task<BookIssue?> GetByIdAsync(int id)
        => await _context.BookIssues
            .Include(bi => bi.Book)
                .ThenInclude(b => b.Author)
            .Include(bi => bi.Student)
            .Include(bi => bi.Fines)
            .FirstOrDefaultAsync(bi => bi.Id == id);

    public async Task<BookIssue?> GetIssuedByIdAsync(int id)
        => await _context.BookIssues
            .Include(bi => bi.Book)
                .ThenInclude(b => b.Author)
            .Include(bi => bi.Student)
            .FirstOrDefaultAsync(bi => bi.Id == id && bi.Status == "Issued");

    public async Task<(bool Success, string? Error)> IssueBookAsync(int bookId, int studentId)
    {
        var book = await _context.Books.FindAsync(bookId);
        if (book == null || book.QuantityAvailable <= 0)
            return (false, "This book is not available for issue.");

        var alreadyIssued = await _context.BookIssues
            .AnyAsync(bi => bi.BookId == bookId && bi.StudentId == studentId && bi.Status == "Issued");
        if (alreadyIssued)
            return (false, "This student already has this book issued. A student cannot borrow the same book twice.");

        var issue = new BookIssue
        {
            BookId = bookId,
            StudentId = studentId,
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(2),
            Status = "Issued",
            FineAmount = 0
        };

        book.QuantityAvailable -= 1;
        await _context.BookIssues.AddAsync(issue);
        await _context.SaveChangesAsync();

        return (true, null);
    }

    public async Task<(bool Success, string? Error, decimal FineAmount, int OverdueDays)> ReturnBookAsync(int id, bool confirm)
    {
        var issue = await _context.BookIssues
            .Include(bi => bi.Book)
            .Include(bi => bi.Student)
            .FirstOrDefaultAsync(bi => bi.Id == id);

        if (issue == null)
            return (false, "Book issue not found.", 0, 0);

        if (issue.Status == "Returned")
            return (false, "This book has already been returned.", 0, 0);

        var returnDate = DateTime.UtcNow;
        var overdueDays = Math.Max(0, (returnDate.Date - issue.DueDate.Date).Days);
        var fineAmount = overdueDays * 5.00m;

        if (fineAmount > 0 && !confirm)
            return (false, null, fineAmount, overdueDays);

        issue.ReturnDate = returnDate;
        issue.Status = "Returned";
        issue.FineAmount = fineAmount;
        issue.Book.QuantityAvailable += 1;

        if (fineAmount > 0)
        {
            _context.Fines.Add(new Fine
            {
                BookIssueId = issue.Id,
                StudentId = issue.StudentId,
                Amount = fineAmount,
                CalculatedOn = returnDate,
                IsPaid = true,
                PaidOn = returnDate
            });
        }

        await _context.SaveChangesAsync();
        return (true, null, fineAmount, overdueDays);
    }

    public async Task CreateAsync(BookIssue issue)
    {
        await _context.BookIssues.AddAsync(issue);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BookIssue issue)
    {
        _context.BookIssues.Update(issue);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _context.BookIssues.FindAsync(id);
        if (e != null)
        {
            _context.BookIssues.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
