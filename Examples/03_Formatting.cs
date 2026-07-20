using System.Globalization;

namespace StringsLab.Examples;

/// <summary>
/// FORMATTING &amp; CULTURE — where "it works on my machine" bugs are born.
///
/// $"{value:FORMAT,ALIGNMENT}" — format specifiers and alignment.
/// The TRAP: number and date formatting follow the CURRENT CULTURE.
/// Croatian (hr-HR) writes 3,50 — English (en-US) writes 3.50. Code that
/// parses/serializes with the ambient culture breaks the moment it runs on
/// a server with different regional settings.
///
/// Rule: HUMANS see CultureInfo of their choosing; MACHINES (JSON, CSV, DB,
/// URLs, logs) always get InvariantCulture.
/// </summary>
public static class FormattingDemo
{
    public static void Run()
    {
        var hr = new CultureInfo("hr-HR");
        var us = new CultureInfo("en-US");
        var de = new CultureInfo("de-DE");
        decimal price = 1234.56m;
        var date = new DateTime(2026, 7, 20, 14, 30, 0);

        // ── Format specifiers ────────────────────────────────────────────────
        Helper.Heading("The format specifiers you'll use daily");
        Helper.Info($"N2  (thousands + 2 dec): {price.ToString("N2", CultureInfo.InvariantCulture)}");
        Helper.Info($"C   (currency, hr-HR):   {price.ToString("C", hr)}");
        Helper.Info($"P1  (percent):           {0.256.ToString("P1", CultureInfo.InvariantCulture)}");
        Helper.Info($"D5  (pad int):           {42.ToString("D5")}");
        Helper.Info($"X   (hex):               0x{255.ToString("X")}");
        Helper.Info($"0.00 (custom):           {3.5m.ToString("0.00", CultureInfo.InvariantCulture)}");

        // ── Alignment ────────────────────────────────────────────────────────
        Helper.Heading("Alignment — {value,WIDTH} (negative = left)");
        foreach (var (item, amount) in new[] { ("kava", 1.70m), ("sendvic", 4.20m) })
            Helper.Info($"|{item,-10}|{amount,8:0.00}|");

        // ── The culture trap, demonstrated ───────────────────────────────────
        Helper.Heading("THE trap: same number, three cultures");
        Helper.Info($"hr-HR: {price.ToString("N2", hr)}   en-US: {price.ToString("N2", us)}   de-DE: {price.ToString("N2", de)}");
        Helper.Info("Same decimal — three different strings. Now in reverse:");

        // Parsing "3,50" succeeds or fails depending on ambient culture!
        var parsedHr = decimal.TryParse("3,50", NumberStyles.Number, hr, out var vHr);
        var parsedUs = decimal.TryParse("3,50", NumberStyles.Number, us, out var vUs);
        Helper.Info($"Parse \"3,50\" as hr-HR: {(parsedHr ? vHr : "fail")}   as en-US: {(parsedUs ? vUs.ToString(CultureInfo.InvariantCulture) : "fail")} (!)");
        Helper.Info("en-US read the comma as a THOUSANDS separator -> 350! Silent data");
        Helper.Info("corruption, no exception. This bug ships every single week somewhere.");

        // ── Dates are worse ──────────────────────────────────────────────────
        Helper.Heading("Dates — the same trap, higher stakes");
        Helper.Info($"hr-HR: {date.ToString("d", hr)}   en-US: {date.ToString("d", us)}");
        Helper.Info($"Is \"03/07/2026\" March 7th or July 3rd? Depends on the culture. 💣");
        Helper.Info($"Machines get ISO 8601:  {date.ToString("o", CultureInfo.InvariantCulture)[..19]}");
        Helper.Info($"or explicit format:     {date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)}");

        // ── The rules ────────────────────────────────────────────────────────
        Helper.Heading("The two rules that prevent all of it");
        Helper.Info("1. SERIALIZING for machines (JSON/CSV/DB/logs/URLs):");
        Helper.Info("   ToString(CultureInfo.InvariantCulture), InvariantCulture parsing.");
        Helper.Info("2. DISPLAYING for people: pass the USER'S culture explicitly.");
        Helper.Info("Bonus: FormattableString.Invariant($\"...\") for interpolated strings,");
        Helper.Info("and dotnet's CA1305 analyzer nags you about every ambient call. Enable it.");
    }
}
