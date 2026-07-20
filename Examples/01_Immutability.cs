namespace StringsLab.Examples;

/// <summary>
/// STRING IMMUTABILITY — every "change" creates a NEW string. Always.
///
/// Why immutable? Safe sharing between threads, safe use as Dictionary keys
/// (the hash can't go stale), interning, substring tricks — the whole platform
/// leans on strings never changing.
///
/// The flip side: s += "x" in a loop allocates a fresh, ever-longer copy EVERY
/// iteration — the classic O(n²) accident (measured in example 02).
///
/// INTERNING: identical string LITERALS share one object at runtime.
/// </summary>
public static class ImmutabilityDemo
{
    public static void Run()
    {
        // ── Methods return new strings ───────────────────────────────────────
        Helper.Heading("Replace/ToUpper/Trim NEVER touch the original");
        string original = "  kava s mlijekom  ";
        string trimmed = original.Trim();
        string upper = trimmed.ToUpper();

        Helper.Info($"original: '{original}' (untouched!)");
        Helper.Info($"trimmed:  '{trimmed}'");
        Helper.Info($"upper:    '{upper}'");
        Helper.Info("Beginner bug: s.Trim(); s.ToUpper();  — results thrown away!");
        Helper.Info("Correct:      s = s.Trim().ToUpper();");

        // ── Interning ────────────────────────────────────────────────────────
        Helper.Heading("Interning — literals share one object");
        string a = "kava";
        string b = "kava";
        string c = string.Concat("ka", "va".AsSpan());   // built at runtime

        Helper.Info($"a == b:                  {a == b}   (value comparison — always content)");
        Helper.Info($"ReferenceEquals(a, b):   {ReferenceEquals(a, b)}   (SAME object — interned literal)");
        Helper.Info($"ReferenceEquals(a, c):   {ReferenceEquals(a, c)}  (runtime string = new object)");
        Helper.Info($"after Intern(c):         {ReferenceEquals(a, string.Intern(c))}   (manually interned)");

        // ── == compares content for strings ──────────────────────────────────
        Helper.Heading("== on string means VALUE equality (operator overload)");
        Helper.Info("For every other reference type, == is reference identity;");
        Helper.Info("string overloads == to compare CONTENT. (And that's why you");
        Helper.Info("never need .Equals for ordinary string comparison in C#.)");

        // ── Each concat = new allocation ─────────────────────────────────────
        Helper.Heading("Every += allocates a new string");
        string s = "a";
        string before = s;
        s += "b";
        Helper.Info($"s += \"b\": ReferenceEquals(before, s) = {ReferenceEquals(before, s)} — new object");
        Helper.Info("1 concat = fine. 10,000 concats in a loop = O(n²) copying —");
        Helper.Info("see the measurement in example 02 (StringBuilder).");

        // ── Immutability enables safe keys ───────────────────────────────────
        Helper.Heading("Why Dictionary<string, T> can trust its keys");
        var prices = new Dictionary<string, decimal> { ["kava"] = 1.70m };
        Helper.Info($"lookup 'kava': {prices["kava"]} — the key CAN'T mutate under the");
        Helper.Info("hash table (unlike a mutable class key — project 01, example 14).");
    }
}
