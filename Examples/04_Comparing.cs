using System.Globalization;

namespace StringsLab.Examples;

/// <summary>
/// SEARCHING &amp; COMPARING — every Contains/StartsWith/Equals/sort should say
/// HOW it compares. The enum: StringComparison.
///
///   Ordinal            — byte-by-byte. Fast, culture-free. For IDs, keys,
///                        protocol values, file paths, anything machine.
///   OrdinalIgnoreCase  — same, case-insensitive. HTTP headers, env vars...
///   CurrentCulture     — linguistic rules of the user's language. For UI
///                        sorting/searching what the USER sees.
///   InvariantCulture   — linguistic but fixed. Rare; stable linguistic sort.
///
/// The famous bug: in Turkish, uppercase of 'i' is 'İ' (dotted capital I) —
/// so "file".ToUpper() != "FILE" on a Turkish server. Ordinal is immune.
/// </summary>
public static class ComparingDemo
{
    public static void Run()
    {
        // ── Say how you compare ──────────────────────────────────────────────
        Helper.Heading("Contains/StartsWith with explicit comparison");
        string header = "Content-Type: application/JSON";
        Helper.Info($"Contains 'json' (Ordinal):           {header.Contains("json", StringComparison.Ordinal)}");
        Helper.Info($"Contains 'json' (OrdinalIgnoreCase): {header.Contains("json", StringComparison.OrdinalIgnoreCase)}");
        Helper.Info("No enum argument = Ordinal for Equals/==, but CULTURE for Compare/");
        Helper.Info("IndexOf — inconsistent defaults are exactly why you spell it out.");

        // ── The Turkish i, live ──────────────────────────────────────────────
        Helper.Heading("The Turkish-i bug, reproduced");
        var tr = new CultureInfo("tr-TR");
        string file = "file";
        Helper.Info($"\"file\".ToUpper() invariant:  {file.ToUpperInvariant()}");
        Helper.Info($"\"file\".ToUpper(tr-TR):       {file.ToUpper(tr)}   <- dotted capital İ!");
        Helper.Info($"equals \"FILE\" via tr upper:  {file.ToUpper(tr) == "FILE"}");
        Helper.Info("Any 'normalize with ToUpper then compare' code dies on a Turkish");
        Helper.Info("locale. Fix: string.Equals(a, b, OrdinalIgnoreCase) — no ToUpper at all.");

        // ── Culture-aware vs ordinal sorting ─────────────────────────────────
        Helper.Heading("Sorting: ordinal vs culture");
        string[] words = ["cesta", "čaj", "banana", "šuma", "auto"];
        var ordinal = words.Order(StringComparer.Ordinal).ToArray();
        var croatian = words.Order(StringComparer.Create(new CultureInfo("hr-HR"), ignoreCase: false)).ToArray();
        Helper.Info($"Ordinal:  {Helper.AsText(ordinal)}   (č i š izbaceni na kraj — po bajtovima)");
        Helper.Info($"hr-HR:    {Helper.AsText(croatian)}   (abeceda kako je covjek ocekuje)");
        Helper.Info("UI lists -> culture comparer. Internal keys/caches -> Ordinal.");

        // ── Dictionaries & sets pick a comparer too ──────────────────────────
        Helper.Heading("Collections take the comparer ONCE, centrally");
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Content-Type"] = "text/plain",
        };
        Helper.Info($"headers[\"content-type\"] found: {headers.ContainsKey("content-type")}");
        Helper.Info("Decide at the collection, not at every lookup — one source of truth.");

        // ── StartsWith perf note ─────────────────────────────────────────────
        Helper.Heading("Perf footnote");
        Helper.Info("Ordinal comparisons are SIMD-fast memory compares; culture-aware");
        Helper.Info("ones consult ICU linguistic tables (10-50x slower). One more reason");
        Helper.Info("machine paths (parsers, routers) should say Ordinal explicitly.");
    }
}
