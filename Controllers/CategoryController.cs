using Microsoft.AspNetCore.Mvc;
using Practice_Project.Models;
using Practice_Project.Services;

namespace Practice_Project.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryServices _categoryService;

        public CategoriesController(ICategoryServices categoryService) => _categoryService = categoryService;

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var categories = await _categoryService.GetAllAsync(searchString);
            return View(categories);
        }

        public IActionResult Create() => View(new CategoryViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _categoryService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var model = await _categoryService.GetByIdAsync(id.Value);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryViewModel model)
        {
            if (id <= 0) id = model.CategoryId;
            if (id != model.CategoryId) return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            await _categoryService.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var model = await _categoryService.GetByIdAsync(id.Value);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
