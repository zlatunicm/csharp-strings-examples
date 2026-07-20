using StringsLab;
using StringsLab.Examples;

// ─────────────────────────────────────────────────────────────────────────────
//  STRINGS & TEXT  —  part 09 of the C# practice series
// ─────────────────────────────────────────────────────────────────────────────

var demos = new (string Title, Action Method)[]
{
    ("01 — Immutability & interning",             ImmutabilityDemo.Run),
    ("02 — StringBuilder (measured)",             StringBuilderDemo.Run),
    ("03 — Formatting & culture traps",           FormattingDemo.Run),
    ("04 — Searching & comparing correctly",      ComparingDemo.Run),
    ("05 — Regex essentials",                     RegexDemo.Run),
};

if (args.Length > 0 && int.TryParse(args[0], out var number) && number >= 1 && number <= demos.Length)
{
    RunDemo(demos[number - 1]);
}
else
{
    Console.WriteLine("Running ALL examples. (Tip: `dotnet run 3` runs only example #3.)");
    foreach (var demo in demos)
        RunDemo(demo);
}

static void RunDemo((string Title, Action Method) demo)
{
    Console.WriteLine();
    Console.WriteLine(new string('═', 62));
    Console.WriteLine($"  {demo.Title}");
    Console.WriteLine(new string('═', 62));
    demo.Method();
}
