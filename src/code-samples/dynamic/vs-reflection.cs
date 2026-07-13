// dynamic vs. reflection — calling an unknown method on a plugin type
//
// Both approaches invoke Calculate(double, double) on a type loaded at
// runtime. Reflection is explicit but verbose; dynamic is readable but
// gives up compile-time verification. Neither is appropriate when you
// own the type — use an interface instead.

using System;
using System.Reflection;

// Simulate a type loaded from an external assembly at runtime.
// In a real scenario this would come from Assembly.Load / Activator.CreateInstance.
object plugin = new TaxCalculator();

// ── BEFORE: reflection ───────────────────────────────────────────────────────

Type pluginType = plugin.GetType();
MethodInfo method = pluginType.GetMethod("Calculate",
    new[] { typeof(double), typeof(double) })!;

double reflectionResult = (double)method.Invoke(plugin, new object[] { 1_000.00, 0.08 })!;
Console.WriteLine($"[Reflection] Tax due: {reflectionResult:C}");

// ── AFTER: dynamic ───────────────────────────────────────────────────────────
// The DLR resolves Calculate at runtime — same behaviour, far less noise.
// Trade-off: a typo or wrong signature becomes a RuntimeBinderException.

dynamic dynamicPlugin = plugin;
double dynamicResult = dynamicPlugin.Calculate(1_000.00, 0.08);
Console.WriteLine($"[Dynamic]    Tax due: {dynamicResult:C}");

// Output:
// [Reflection] Tax due: $80.00
// [Dynamic]    Tax due: $80.00

// ─────────────────────────────────────────────────────────────────────────────
// Supporting type (would normally live in a separately loaded assembly)
public class TaxCalculator
{
    public double Calculate(double subtotal, double rate) => subtotal * rate;
}
