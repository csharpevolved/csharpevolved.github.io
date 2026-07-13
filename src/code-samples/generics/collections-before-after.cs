using System;
using System.Collections;
using System.Collections.Generic;

public static class Program
{
    public static void Main()
    {
        // --- Before generics: ArrayList stores everything as object ---
        // Every value type is boxed on Add, and every read requires an explicit cast.
        // Nothing stops you from adding the wrong type — the bug surfaces at runtime.
        Console.WriteLine("Before generics (ArrayList):");
        ArrayList scores = new ArrayList();
        scores.Add(95);
        scores.Add(87);
        scores.Add(72);
        // scores.Add("oops"); // compiles fine — InvalidCastException waiting to happen

        int total = 0;
        foreach (object item in scores)
        {
            total += (int)item; // cast required; wrong type throws at runtime
        }
        Console.WriteLine($"  Total: {total}");

        // --- After generics: List<int> is type-safe and avoids boxing entirely ---
        // The compiler rejects wrong types at the call site, before you ship.
        Console.WriteLine("After generics (List<int>):");
        List<int> typedScores = new List<int>();
        typedScores.Add(95);
        typedScores.Add(87);
        typedScores.Add(72);
        // typedScores.Add("oops"); // compile error — caught before you run

        int typedTotal = 0;
        foreach (int score in typedScores)
        {
            typedTotal += score; // no cast; no boxing; type is known at compile time
        }
        Console.WriteLine($"  Total: {typedTotal}");
    }
}
