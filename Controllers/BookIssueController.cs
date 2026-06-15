using Microsoft.AspNetCore.Mvc;
using Practice_Project.Models;
using Practice_Project.Services;

namespace Practice_Project.Controllers
{
    public class BookIssueController : Controller
    {
        private readonly IBookIssueServices _bookIssueService;

        public BookIssueController(IBookIssueServices bookIssueService)
        {
            _bookIssueService = bookIssueService;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var issues = await _bookIssueService.GetAllAsync(searchString);
            return View(issues);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Books = await _bookIssueService.GetAvailableBooksAsync();
            ViewBag.Students = await _bookIssueService.GetStudentsAsync();
            return View(new BookIssueViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookIssueViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Books = await _bookIssueService.GetAvailableBooksAsync();
                ViewBag.Students = await _bookIssueService.GetStudentsAsync();
                return View(vm);
            }

            var result = await _bookIssueService.CreateAsync(vm);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Error ?? "Unable to issue this book.");
                ViewBag.ErrorMessage = result.Error;
                ViewBag.Books = await _bookIssueService.GetAvailableBooksAsync();
                ViewBag.Students = await _bookIssueService.GetStudentsAsync();
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Return(int id)
        {
            var issue = await _bookIssueService.GetIssuedByIdAsync(id);
            if (issue == null)
            {
                TempData["Error"] = "Book issue not found or already returned.";
                return RedirectToAction(nameof(Index));
            }

            return View(issue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id, string? action)
        {
            var result = await _bookIssueService.ProcessReturnAsync(id, action == "confirm");

            if (!result.Success && result.Error != null)
            {
                if (result.Error.Contains("already been returned"))
                    TempData["Info"] = result.Error;
                else
                    TempData["Error"] = result.Error;

                return RedirectToAction(nameof(Index));
            }

            if (!result.Success && result.Issue != null)
            {
                ViewBag.CalculatedFine = result.FineAmount;
                ViewBag.OverdueDays = result.OverdueDays;
                ViewBag.ReturnDate = DateTime.UtcNow.ToString("dd MMM yyyy");
                return View("ReturnConfirm", result.Issue);
            }

            TempData["Success"] = result.FineAmount > 0
                ? $"Book returned successfully. Fine of ₹{result.FineAmount} collected ({result.OverdueDays} days overdue)."
                : "Book returned successfully!";

            return RedirectToAction(nameof(Index));
        }
    }
}
