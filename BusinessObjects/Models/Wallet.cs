using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Wallet
{
    public int WalletId { get; set; }

    public int UserId { get; set; }

    public string WalletName { get; set; } = null!;

    public string WalletType { get; set; } = null!;

    public decimal InitialBalance { get; set; }

    public decimal Balance { get; set; }

    public string? Note { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<FinanceTransaction> FinanceTransactions { get; set; } = new List<FinanceTransaction>();

    public virtual User User { get; set; } = null!;
}
