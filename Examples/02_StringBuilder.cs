using System.Diagnostics;
using System.Text;

namespace StringsLab.Examples;

/// <summary>
/// StringBuilder — MUTABLE text building (System.Text).
///
/// A StringBuilder keeps an internal growable char buffer; Append writes into
/// it without creating intermediate strings. ToString() at the end produces
/// the single final string.
///
/// Rule of thumb:
///   2-4 known pieces           -> interpolation / concat (compiler optimizes)
///   a LOOP that grows a string -> StringBuilder, always
///   joining a collection       -> string.Join (it pre-sizes perfectly)
/// </summary>
public static class StringBuilderDemo
{
    public static void Run()
    {
        // ── The measurement that settles it ──────────────────────────────────
        Helper.Heading("Measured: 20,000 appends");
        const int N = 20_000;

        var sw = Stopwatch.StartNew();
        string concat = "";
        for (int i = 0; i < N; i++)
            concat += "x";                       // new string EVERY pass (O(n^2) copying)
        sw.Stop();
        var concatMs = sw.Elapsed.TotalMilliseconds;

        sw.Restart();
        var sb = new StringBuilder();
        for (int i = 0; i < N; i++)
            sb.Append('x');                      // writes into the buffer
        string built = sb.ToString();
        sw.Stop();

        Helper.Info($"string += : {concatMs:F1} ms      StringBuilder: {sw.Elapsed.TotalMilliseconds:F2} ms");
        Helper.Info($"(equal results: {concat.Length} == {built.Length} chars)");
        Helper.Info("The += version copied ~200 MILLION chars in total. The builder: 20k.");

        // ── The API tour ─────────────────────────────────────────────────────
        Helper.Heading("The API you'll actually use");
        var receipt = new StringBuilder(capacity: 256);      // pre-size when you can
        receipt.AppendLine("RACUN #042");
        receipt.AppendLine("----------");
        receipt.Append("kava").Append(" x2").AppendLine("   3.40");
        receipt.AppendFormat("PDV: {0:0.00}", 0.85m).AppendLine();
        receipt.Insert(0, "* ").Replace("kava", "KAVA");

        foreach (var line in receipt.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            Helper.Info(line);

        // ── Interpolation into a builder (no garbage) ────────────────────────
        Helper.Heading("AppendLine($\"...\") is optimized (.NET 6+)");
        var log = new StringBuilder();
        var (user, count) = ("Ana", 3);
        log.Append($"korisnik {user} ima {count} narudzbe");   // interpolation handler —
        Helper.Info(log.ToString());                            // appends parts directly!
        Helper.Info("The compiler routes interpolated parts straight into the buffer.");

        // ── When NOT to bother ───────────────────────────────────────────────
        Helper.Heading("When plain strings are fine (don't over-engineer)");
        string name = "Marko";
        string greeting = $"pozdrav, {name}!";                 // 2-3 pieces: perfect as-is
        Helper.Info($"one-off interpolation: '{greeting}' — no builder needed");
        Helper.Info($"string.Join for lists:  '{string.Join(", ", new[] { "a", "b", "c" })}' — pre-sized, optimal");
        Helper.Info("StringBuilder shines ONLY when text grows across iterations/calls.");
    }
}
