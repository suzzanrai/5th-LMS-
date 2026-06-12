using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practice_Project.Data;
using Practice_Project.Entities;
using Practice_Project.Models;
using Practice_Project.Services;

namespace Practice_Project.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IBookServices _bookService;

        public BooksController(IBookServices bookService) => _bookService = bookService;

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var books = await _bookService.GetAllAsync(searchString);
            return View(books);
        }


        public async Task<IActionResult> Create()
        {
            var vm = new BookViewModel();
            await _bookService.PopulateDropdownsAsync(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (model.QuantityAvailable > model.TotalQuantity)
                ModelState.AddModelError("QuantityAvailable", "Available copies cannot exceed total copies.");

            if (ModelState.IsValid)
            {
                await _bookService.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }

            await _bookService.PopulateDropdownsAsync(model);
            return View(model);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var model = await _bookService.GetByIdAsync(id.Value);
            if (model == null) return NotFound();

            await _bookService.PopulateDropdownsAsync(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookViewModel model)
        {
            if (id <= 0) id = model.BookId;
            if (id != model.BookId) return NotFound();

            if (model.QuantityAvailable > model.TotalQuantity)
                ModelState.AddModelError("QuantityAvailable", "Available copies cannot exceed total copies.");

            if (ModelState.IsValid)
            {
                await _bookService.UpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }

            await _bookService.PopulateDropdownsAsync(model);
            return View(model);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var model = await _bookService.GetByIdAsync(id.Value);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool BookExits(int id)
        {
            // Keep legacy check but use service/repo ideally
            return false; // left intentionally simple; rely on repo's ExistsAsync when needed
        }
    }
}