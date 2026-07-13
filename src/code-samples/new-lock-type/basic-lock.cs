using System.Threading;

// ── BEFORE: using a plain object as a lock sentinel ──────────────────────────
// Nothing in the type system communicates that _lock is a guard.
// Passing a value type here would silently box it on every entry.

public sealed class CounterBefore
{
    private readonly object _lock = new();
    private int _value;

    public void Increment()
    {
        lock (_lock)
        {
            _value++;
        }
    }

    public int Read()
    {
        lock (_lock)
        {
            return _value;
        }
    }
}

// ── AFTER: using System.Threading.Lock ───────────────────────────────────────
// The call-site syntax is identical, but the compiler now sees a Lock, not an
// object, and emits an optimized monitor-entry sequence. The field name alone
// tells readers exactly what the field is for.

public sealed class Counter
{
    private readonly Lock _lock = new();
    private int _value;

    public void Increment()
    {
        lock (_lock)   // same keyword — no call-site changes required
        {
            _value++;
        }
    }

    public int Read()
    {
        lock (_lock)
        {
            return _value;
        }
    }
}
