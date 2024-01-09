//using Expense_Tracker.Models;
//using Expense_Tracker.Controllers;
//using Microsoft.AspNetCore.Mvc;



//namespace Expense_Tracker_Api.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//public class CategoryControllerAPI : ControllerBase
//{
//    private readonly CategoryController _categoryController;
//    public CategoryControllerAPI(CategoryController categoryController)
//    {
//        _categoryController = categoryController;
//    }

//    // GET: Category
//    [HttpGet("GetAll")]
//    public async Task<IActionResult> GetAll([FromBody] Category command)
//    {
//        try
//        {
//            return _context.Categories != null ?
//                          View(await _context.Categories.ToListAsync()) :
//                          Problem("Entity set 'ApplicationDbContext.Categories' is null.");
//        }
//        catch (Exception ex)
//        {
//            return BadRequest(new { ErrorMessage = ex.Message });
//        }
//    }

//    // GET: Category/AddOrEdit
//    [HttpGet("AddOrEdit")]
//    public async Task<IActionResult> AddOrEdit([Bind("CategoryId, Titile, Icon, Type")] Category category)
//    {
//        try
//        {
//            if (id == 0)
//                return View(new Category());
//            else
//                return View(_context.Categories.Find(id));
//        }
//        catch (Exception)
//        {

//            throw;
//        }
//    }

//    // POST: Category/AddOrEdit
//    [HttpPost("AddOrEdit")]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> AddOrEdit([Bind("CategoryId, Titile, Icon, Type")] Category category)
//    {
//        try
//        {
//            if (ModelState.IsValid)
//            {
//                if (category.CategoryId == 0)
//                    _context.Add(category);
//                else
//                    _context.Update(category);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(category);
//        }
//        catch (Exception)
//        {

//            throw;
//        }
//    }

//    [HttpDelete, ActionName("Delete")]
//    public async Task<IActionResult> DeleteConfirmed(int id)
//    {
//        if (_context.Categories == null)
//        {
//            return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
//        }
//        var category = await _context.Categories.FindAsync(id);
//        if (category != null)
//        {
//            _context.Categories.Remove(category);
//        }

//        await _context.SaveChangesAsync();
//        return RedirectToAction(nameof(Index));
//    }

//}
