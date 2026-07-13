using System;
using System.Collections.Generic;
using System.Threading;

// EnterScope() returns a ref struct that releases the lock when disposed.
// This is useful when the guarded region spans multiple steps and you want
// the enter/exit boundary to be explicit rather than hidden inside a lock block.

public sealed class AccountLedger
{
    private readonly Lock _lock = new();
    private readonly List<string> _entries = new();
    private decimal _balance;

    /// <summary>
    /// Applies a debit and records the journal entry as a single atomic operation.
    /// EnterScope() guarantees the lock is released even if an exception is thrown
    /// while building the entry string.
    /// </summary>
    public void ApplyDebit(decimal amount, string description)
    {
        using (_lock.EnterScope())
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Debit amount must be positive.");

            if (_balance < amount)
                throw new InvalidOperationException("Insufficient funds.");

            // Both mutations happen under the same lock scope — no window between them.
            _balance -= amount;
            _entries.Add($"DEBIT  {amount,12:F2}  {description}  (balance: {_balance:F2})");
        }
        // Lock is released here by the ref-struct Dispose(), not by the lock keyword.
    }

    public void ApplyCredit(decimal amount, string description)
    {
        using (_lock.EnterScope())
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Credit amount must be positive.");

            _balance += amount;
            _entries.Add($"CREDIT {amount,12:F2}  {description}  (balance: {_balance:F2})");
        }
    }

    public (decimal Balance, IReadOnlyList<string> Entries) Snapshot()
    {
        using (_lock.EnterScope())
        {
            return (_balance, _entries.AsReadOnly());
        }
    }
}
