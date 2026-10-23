# C# Strings & Text — Hands-on Examples

Part **09** of a hands-on C# practice series. Five runnable, heavily commented examples: immutability and interning proven with `ReferenceEquals`, the `+=`-in-a-loop disaster **measured** against `StringBuilder`, culture formatting traps (the `"3,50"` → `350` silent corruption, date ambiguity), correct comparing (`StringComparison`, the Turkish-i bug reproduced live), and regex essentials with `[GeneratedRegex]` and ReDoS timeouts.

## Running

```bash
dotnet run        # all examples
dotnet run 3      # just one (e.g. culture traps)
```

## What's inside

| # | Topic | Highlights |
|---|-------|------------|
| 01 | Immutability | methods return new strings, interning, why `==` compares content |
| 02 | StringBuilder | **20k appends measured**, the API tour, when plain strings win |
| 03 | Formatting & culture | specifiers, alignment, `"3,50"` parsed as **350** in en-US, ISO dates |
| 04 | Comparing | `StringComparison` everywhere, **Turkish-i reproduced**, ordinal vs culture sort |
| 05 | Regex | `[GeneratedRegex]`, named groups, `MatchEvaluator`, **ReDoS timeout demo** |

## The rules to remember

1. Machines get `InvariantCulture`; humans get their own culture — never the ambient default.
2. Every `Contains`/`Equals`/sort says **how** it compares (`Ordinal` for machine data).
3. Never `ToUpper` to compare — `Equals(..., OrdinalIgnoreCase)`.
4. String grows in a loop → `StringBuilder`; joining a list → `string.Join`.
5. Regex that sees user input gets a **timeout**; JSON/HTML get real parsers.

## Further reading

- [Strings (C# guide)](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/)
- [Best practices for comparing strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices-strings)
- [.NET regular expressions](https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expressions)

## The series

← Previous: [08 Modern C# 8–12](https://github.com/zlatunicm/csharp-modern-examples) · Next: [10 Files & JSON](https://github.com/zlatunicm/csharp-files-json-examples) →

| Part | Topic |
|---|---|
| 01 | [Collections](https://github.com/zlatunicm/csharp-collections-examples) |
| 02 | [LINQ](https://github.com/zlatunicm/csharp-linq-examples) |
| 03 | [Delegates & events](https://github.com/zlatunicm/csharp-delegates-events-examples) |
| 04 | [Async/await](https://github.com/zlatunicm/csharp-async-examples) |
| 05 | [Generics](https://github.com/zlatunicm/csharp-generics-examples) |
| 06 | [Types & memory](https://github.com/zlatunicm/csharp-types-memory-examples) |
| 07 | [Exceptions](https://github.com/zlatunicm/csharp-exceptions-examples) |
| 08 | [Modern C# 8–12](https://github.com/zlatunicm/csharp-modern-examples) |
| 09 | **Strings & text** (this repo) |
| 10 | [Files & JSON](https://github.com/zlatunicm/csharp-files-json-examples) |
| 11 | [MiniShop microservices (finale)](https://github.com/zlatunicm/minishop-microservices) |

## Author

**Matej Zlatunić**

## License

Released under the [MIT License](LICENSE). Feel free to use it for learning.
