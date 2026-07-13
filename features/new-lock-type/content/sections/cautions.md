`System.Threading.Lock` is available only on .NET 9 and later; projects targeting older runtimes or .NET Standard must continue using `object`-based locking or a `SemaphoreSlim`.

`Lock` is a synchronous primitive and is not awaitable — if your critical section contains `await` expressions, use `SemaphoreSlim` with `WaitAsync` instead, because holding a `Lock` across an `await` will release the thread back to the pool while still logically holding the mutex, leading to subtle race conditions.
