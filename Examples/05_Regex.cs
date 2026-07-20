using System.Text.RegularExpressions;

namespace StringsLab.Examples;

/// <summary>
/// REGEX ESSENTIALS — System.Text.RegularExpressions, the 20% you use 95% of
/// the time: IsMatch, Match + named groups, Matches, Replace, Split.
///
/// Modern .NET notes:
///   - [GeneratedRegex] (.NET 7): the pattern compiles at BUILD time into
///     specialized C# — fastest option, typo'd patterns fail the build.
///   - Always set a TIMEOUT on patterns fed with user input — catastrophic
///     backtracking is a real DoS class (ReDoS).
///   - Regex is a chainsaw: for "does it contain X" use Contains; for parsing
///     structured formats (JSON!) use a real parser, not regex.
/// </summary>
public static partial class RegexDemo
{
    // Build-time compiled patterns (.NET 7 source generator):
    [GeneratedRegex(@"^[\w.+-]+@[\w-]+\.[\w.]+$")]
    private static partial Regex EmailPattern();

    [GeneratedRegex(@"(?<kolicina>\d+)\s*x\s*(?<artikl>[a-zčćžšđ]+)", RegexOptions.IgnoreCase)]
    private static partial Regex OrderLinePattern();

    public static void Run()
    {
        // ── IsMatch — validation ─────────────────────────────────────────────
        Helper.Heading("IsMatch — validation (with [GeneratedRegex])");
        foreach (var email in new[] { "ana@mikson.hr", "nije-email", "iva+test@gmail.com" })
            Helper.Info($"{email,-22} -> {(EmailPattern().IsMatch(email) ? "valjan" : "NIJE valjan")}");
        Helper.Info("(The pattern was compiled at BUILD time — zero startup cost, max speed.)");

        // ── Named groups — extraction ────────────────────────────────────────
        Helper.Heading("Match + named groups — pulling data out");
        string narudzba = "2 x kava, 1 x kolac, 3 x sok";
        foreach (Match m in OrderLinePattern().Matches(narudzba))
        {
            var kolicina = int.Parse(m.Groups["kolicina"].Value);
            var artikl = m.Groups["artikl"].Value;
            Helper.Info($"artikl='{artikl}', kolicina={kolicina}");
        }
        Helper.Info("Named groups (?<ime>...) beat numbered ones — refactor-safe and readable.");

        // ── Replace — with substitutions and callbacks ───────────────────────
        Helper.Heading("Replace — patterns and MatchEvaluator");
        string text = "Racun 042 za korisnika 007";
        Helper.Info($"pad numbers:  {Regex.Replace(text, @"\d+", m => m.Value.PadLeft(4, '0'))}");
        Helper.Info($"swap order:   {Regex.Replace("Zlatunic, Matej", @"(\w+), (\w+)", "$2 $1")}");
        Helper.Info($"mask digits:  {Regex.Replace("kartica 1234-5678", @"\d", "*")}");

        // ── Split with a pattern ─────────────────────────────────────────────
        Helper.Heading("Split on a pattern");
        var parts = Regex.Split("jedan ;dva;  tri ; cetiri", @"\s*;\s*");
        Helper.Info($"';' with any spacing: {Helper.AsText(parts)}");

        // ── Timeouts — the ReDoS insurance ───────────────────────────────────
        Helper.Heading("ALWAYS timeout patterns that see user input");
        var risky = new Regex(@"^(\w+\s?)+$", RegexOptions.None, TimeSpan.FromMilliseconds(100));
        try
        {
            // A crafted input that makes this pattern backtrack explosively:
            risky.IsMatch(new string('a', 30) + "!");
            Helper.Info("(fast input passed fine)");
        }
        catch (RegexMatchTimeoutException)
        {
            Helper.Info("RegexMatchTimeoutException after 100 ms — the service survived a");
            Helper.Info("catastrophic-backtracking input instead of burning a CPU forever.");
        }

        // ── Know when NOT to regex ───────────────────────────────────────────
        Helper.Heading("When NOT to regex");
        Helper.Info("substring check     -> Contains / StartsWith (clearer, faster)");
        Helper.Info("fixed-char splits   -> string.Split(';')");
        Helper.Info("JSON / HTML / XML   -> real parsers. Regexing HTML summons Zalgo. 😈");
        Helper.Info("complex validation  -> often: TryParse + a few ifs read better");
    }
}
