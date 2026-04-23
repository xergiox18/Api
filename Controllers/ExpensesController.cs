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
}