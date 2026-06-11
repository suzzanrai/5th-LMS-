using Microsoft.AspNetCore.Mvc;
using Practice_Project.Models;
using Practice_Project.Services;

namespace Practice_Project.Controllers;

public class StudentController : Controller
{
    private readonly IStudentServices _studentService;

    public StudentController(IStudentServices studentService) => _studentService = studentService;

    public async Task<IActionResult> Index(string searchString)
    {
        ViewData["CurrentFilter"] = searchString;
        var students = await _studentService.GetAllAsync(searchString);
        return View(students);
    }

    public IActionResult Create()
        => View(new StudentVm());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _studentService.CreateAsync(model);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var model = await _studentService.GetByIdAsync(id.Value);
        if (model == null) return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, StudentVm model)
    {
        if (id <= 0) id = model.StudentId;
        if (id != model.StudentId) return NotFound();

        if (!ModelState.IsValid)
            return View(model);

        await _studentService.UpdateAsync(model);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var model = await _studentService.GetByIdAsync(id.Value);
        if (model == null) return NotFound();

        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _studentService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
