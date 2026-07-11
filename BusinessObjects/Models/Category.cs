using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public int UserId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string CategoryType { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();

    public virtual ICollection<FinanceTransaction> FinanceTransactions { get; set; } = new List<FinanceTransaction>();

    public virtual User User { get; set; } = null!;
}
