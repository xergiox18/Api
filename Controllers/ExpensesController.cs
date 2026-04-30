using System;
using Expenses.Models;
using Expenses.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    private static readonly List<Expense> _expenses = [];

    [HttpPost]
    public IActionResult RegisterExpense([FromBody] ExpenseDTO expense)
    {
        Expense newExpense = new()
        {
            Id = Guid.NewGuid(),
            Description = expense.Description,
            Amount = expense.Amount,
            Date = expense.Date,
            Category = expense.Category,
            PaymentMethod = expense.PaymentMethod
        };

        _expenses.Add(newExpense);

        return CreatedAtAction(nameof(RegisterExpense), new { id = newExpense.Id }, newExpense);
    }

    [HttpGet]
    public IActionResult GetExpenses()
    {
        return Ok(_expenses);
    }

    [HttpGet("filter")]
    public IActionResult GetExpensesByFilter([FromQuery] string? category, [FromQuery] string? paymentMethod)
    {
        var expenses = _expenses.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(category))
        {
            expenses = expenses.Where(x => x.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(paymentMethod))
        {
            expenses = expenses.Where(x => x.PaymentMethod.Equals(paymentMethod, StringComparison.OrdinalIgnoreCase));
        }

        return Ok(expenses);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateExpense(Guid id, [FromBody] ExpenseDTO expense)
    {
        var existingExpense = _expenses.FirstOrDefault(x => x.Id == id);

        if (existingExpense is null)
        {
            return NotFound(new { message = "Expense not found." });
        }

        existingExpense.Description = expense.Description;
        existingExpense.Amount = expense.Amount;
        existingExpense.Date = expense.Date;
        existingExpense.Category = expense.Category;
        existingExpense.PaymentMethod = expense.PaymentMethod;

        return Ok(existingExpense);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteExpense(Guid id)
    {
        var existingExpense = _expenses.FirstOrDefault(x => x.Id == id);

        if (existingExpense is null)
        {
            return NotFound(new { message = "Expense not found." });
        }

        _expenses.Remove(existingExpense);

        return NoContent();
    }
}
