using Expense_Tracker_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {

        _context = context;

    }

    // GET: api/Category/All
    [HttpGet]
    [Route("All", Name = "GetAllCategories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetAll()
    {
        if (_context.Categories == null)
        {
            return Problem("Entity set 'AppDbContext.Categories' is null.", statusCode: 500);
        }

        var categories = await _context.Categories.ToListAsync();
        return Ok(categories);
    }

    // POST: api/Category/AddOrEdit
    [HttpPost("AddOrEdit")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Category>> AddOrEdit([FromBody] Category category)
    {
        if (ModelState.IsValid)
        {
            if (category.CategoryId == 0)
                _context.Add(category);
            else
                _context.Update(category);

            await _context.SaveChangesAsync();
            return Ok(category);
        }

        return BadRequest(ModelState);
    }

    // DELETE: api/Category/DeleteConfirmed/{id}
    [HttpDelete, ActionName("Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Categories == null)
        {
            return Problem("Entity set 'AppDbContext.Categories'  is null.", statusCode: 500);
        }

        var category = await _context.Categories.FindAsync(id);
        //if (category != null)
        //{
        //    return NotFound("Category not found");
        //}

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return Ok("Category deleted successfully");
    }
}
