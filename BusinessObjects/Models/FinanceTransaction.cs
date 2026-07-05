using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class FinanceTransaction
{
    public int TransactionId { get; set; }

    public int UserId { get; set; }

    public int WalletId { get; set; }

    public int CategoryId { get; set; }

    public string TransactionType { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateOnly TransactionDate { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;
}
