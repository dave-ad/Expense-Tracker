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
            return Problem("Entity set 'AppDbContext.Categories' is null.", statusCode: 500);

        var categories = await _context.Categories.ToListAsync();
        return Ok(categories);
    }

    // GET: api/Category/AddOrEdit/{id}
    [HttpGet]
    [Route("{id:int}", Name = "GetCategoryById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> AddOrEdit(int id = 0)
    {
        if (id == 0)
            return Ok(new Category());
        else
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                return Ok(category);
            }
            else
            {
                return NotFound("Category not found");
            }
        }
    }

    // POST: api/Category/Add
    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Category>> AddCategory([FromBody] Category category)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return CreatedAtAction("AddCategory", new { id = category.CategoryId }, category);
            }
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it according to your application's error handling strategy
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    // PUT: api/Category/Edit
    [HttpPut("Edit")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Category>> EditCategory([FromBody] Category category)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var existingCategory = await _context.Categories.FindAsync(category.CategoryId);

                if (existingCategory == null)
                    return NotFound();

                _context.Entry(existingCategory).CurrentValues.SetValues(category);
                await _context.SaveChangesAsync();

                return Ok(existingCategory);
            }
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    // DELETE: api/Category/DeleteConfirmed/{id}
    [HttpDelete("Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Categories == null)
            return Problem("Entity set 'AppDbContext.Categories'  is null.", statusCode: 500);

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
