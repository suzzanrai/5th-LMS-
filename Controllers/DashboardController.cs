using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Practice_Project.Data;

using Practice_Project.Models;

namespace Practice_Project.Controllers
{
    public class DashboardController : Controller
    {
        private readonly LibraryDbContext _context;

        public DashboardController(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel();

            // Total Counts
            model.TotalStudents = await _context.Students.CountAsync();
            model.TotalBooks = await _context.Books.CountAsync();
            model.TotalAuthors = await _context.Authors.CountAsync();
            model.TotalCategories = await _context.Categories.CountAsync();

            // Currently Issued Books
            var issuedCount = await _context.BookIssues
                .Where(bi => bi.ReturnDate == null)
                .CountAsync();

            model.CurrentlyIssued = issuedCount;
            model.AvailableBooks = model.TotalBooks - issuedCount;

        //overdue
           var overdueIssues = await _context.BookIssues
               .Include(bi => bi.Book)
               .Include(bi => bi.Student)
               .Where(bi => bi.ReturnDate == null && bi.DueDate < DateTime.UtcNow)
               .ToListAsync();

           model.OverdueCount = overdueIssues.Count;

           decimal totalFine = 0;
           foreach (var issue in overdueIssues)
           {
               int daysOverdue = (DateTime.Now - issue.DueDate).Days;
               issue.FineAmount = daysOverdue * 5.0m;
               totalFine += issue.FineAmount;
           }

           model.OverdueBooks = overdueIssues;
           model.TotalPendingFine = $"रु {totalFine:F2}";

             return View(model);
        }
    }
}