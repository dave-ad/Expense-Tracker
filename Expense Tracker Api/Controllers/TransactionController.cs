using Expense_Tracker_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransactionController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Transaction/All
    [HttpGet]
    [Route("All", Name = "GetAllTransactions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetAll()
    {
        var transactions = await _context.Transactions.Include(t => t.Category).ToListAsync();
        return Ok(transactions);
    }

    // GET: api/Transaction/AddOrEdit/{id}
    [HttpGet]
    [Route("{id:int}", Name = "GeTransactionById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Transaction> AddOrEdit(int id = 0)
    {
        var categories = _context.Categories.ToList();

        if (id == 0)
            return Ok(new { Transaction = new Transaction(), Categories = categories });
        else
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction != null)
                return Ok(new { Transaction = transaction, Categories = categories });
            else
                return NotFound("Transaction not found");
        }
    }

    // POST: api/Transaction/AddOrEdit
    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Transaction>> AddTransaction([FromBody] Transaction transaction)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return Ok(transaction);
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        }
    }

    // POST: api/Transaction/Edit
    [HttpPut("Edit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Transaction>> EditTransaction([FromBody] Transaction transaction)
    {
        try
        {
            if (transaction == null || transaction.TransactionId == 0)
                return BadRequest("Invalid transaction or TransactionId");

            var existingTransaction = await _context.Transactions.FindAsync(transaction.TransactionId);

            if (existingTransaction == null)
                return NotFound();

            _context.Entry(existingTransaction).CurrentValues.SetValues(transaction);

            await _context.SaveChangesAsync();
            return Ok(existingTransaction);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        }
    }

    // DELETE: api/Transaction/DeleteConfirmed/{id}
    [HttpDelete("Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        if (_context.Transactions == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Transactions' is null.", statusCode: 500);
        }

        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return NotFound("Transaction not found");
        }

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();

        return Ok("Transaction deleted successfully");
    }
}
