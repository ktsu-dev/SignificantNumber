# SignificantNumber Benchmarks

A [BenchmarkDotNet](https://benchmarkdotnet.org) suite covering the operations that dominate real
use of `SignificantNumber`: arithmetic, comparison, reducing significance, parsing, and conversion
from the primitive numeric types.

## Running

From the repository root:

```bash
# Pick benchmarks from an interactive list
dotnet run -c Release --project SignificantNumber.Benchmarks

# Run everything
dotnet run -c Release --project SignificantNumber.Benchmarks -- --filter '*'

# Run one class, or one method
dotnet run -c Release --project SignificantNumber.Benchmarks -- --filter '*ArithmeticBenchmarks*'
dotnet run -c Release --project SignificantNumber.Benchmarks -- --filter '*.Divide'
```

## Measuring a published release

Set `BenchmarkAgainstVersion` and the suite measures that package instead of the working copy:

```bash
BenchmarkAgainstVersion=1.4.40 dotnet run -c Release --project SignificantNumber.Benchmarks -- --filter '*.Add'
```

Set it **in the environment, not with `-p:`**. BenchmarkDotNet generates and builds a project of its
own for each run, and a property passed on the command line does not reach that project — it would
build the benchmark assembly against the version you asked for and the harness against the one
pinned centrally, which fails to compile if the type changed shape between them. MSBuild reads
environment variables as properties in every project, so the environment form reaches both.

This switch is how `docs/benchmarks/` is filled. No tag in this repository carries a benchmark
project, so there is no older source to check out and run; and measuring packages is the better
comparison anyway, because every version is timed by identical benchmark code rather than by
whatever each tag happened to ship.

### What the older packages cannot be asked

The history starts at 1.3.0, and one panel of it starts at 1.4.20.

`Operands` builds every value by parsing text, because that is the only construction route all the
published versions share. 1.2.2 and 1.2.7 throw `NotSupportedException` from `Parse`, so they
compile against these benchmarks, run, and report a table of `NA`. Ingest refuses a run with no
measurement in it rather than putting a release on the axis with nothing under it, and the backfill
reports it and moves on.

Significance reduction is spelled three ways across the versions: 1.2.x has no precision overload of
`ToSignificantNumber` at all, and 1.3 and 1.4.0 hang it off the `PreciseNumber` base, where the
receiver's type argument has to be written out. Carrying three spellings of one benchmark would
measure the spellings, so `SignificanceBenchmarks.cs` is left out of builds against anything older
than 1.4.20 and the chart draws that panel with a gap. Everything else in those versions is still
measured.

### Why nothing here touches an internal member

Measuring published packages is also why nothing here reaches for an internal. The
`InternalsVisibleTo` that would expose one is not in the packages already published, so a benchmark
built on internals could only ever measure the working copy.

It is why there is no formatting benchmark, too. Before 2.0 this type derived from `PreciseNumber`
rather than wrapping it, so `ToString` was an inherited member: measuring it would mean referencing
that package here, and the reference makes `Parse` and `GetHashCode` ambiguous against their
inherited counterparts. One operation is not worth the whole release history before 2.0.

## What this type costs against a bare double

`AbstractionCostBenchmarks` is the one benchmark here whose answer is a ratio rather than a
duration. Every other class says how long an operation takes, which is only readable beside
something; this supplies the something — the primitive a caller would otherwise have used.

The same class, with the same loops and the same methodology, is in `ktsu.PreciseNumber` and
`ktsu.Semantics`. Against PreciseNumber in particular the difference is what significance tracking
adds, since this type is built on that one.

| release | `Add` | `Multiply` |
|---|---|---|
| 1.3.0 | 1377.7× | 5809.6× |
| 1.4.40 | 1352.4× | 5944.6× |
| 2.0.0 | **89.7×** | **550.5×** |
| 2.0.1 | 89.9× | 569.3× |

Becoming a value type in 2.0 took roughly **15× off add and 10× off multiply**. For comparison,
the same change in `ktsu.PreciseNumber` underneath moved its ratios by about 15% — so most of what
2.0 recovered here was this layer's own allocation, not the number beneath it.

**The ratio is not expected to be 1 and is not a defect for being large.** Every operation pays
twice: once for the arbitrary-precision arithmetic and again for rounding the result back to the
significance the operands justify, and both buy something a `double` cannot do at all. What the
chart's third section is for is noticing the day it moves.

Three things decide how the number should be read:

- **These are loops.** A single operation over operands that do not change is loop-invariant and
  the JIT hoists it out, which would leave the `double` side indistinguishable from an empty method
  and the ratio meaningless. Each iteration feeds the next, so there is nothing to hoist.
- **The loop's own cost biases toward 1**, being paid by both sides, so a ratio is a floor on the
  real cost rather than the whole of it.
- **Both loops accumulate rather than compound**, because the number underneath carries as many
  digits as the arithmetic produces and a compounding chain would measure that growth instead of
  the operation. How the cost grows with digits is a different question, and `ArithmeticBenchmarks`
  answers it across the `Digits` axis.

## Reading the results

Most classes are parameterised by `Digits` (8, 30, 200). That axis is the point: significance is
carried in a `BigInteger`, so anything touching digits one at a time looks fine at 8 and collapses
at 200. Read across the `Digits` column, not down one value of it.

Allocation is reported alongside time and matters just as much. Between 1.4.40 and 2.0.1 a 200-digit
`Add` went from 54,688 bytes to 112, and from 128 μs to 340 ns — the type became a value type and
stopped allocating an object per intermediate.

Every operation pays twice: the arithmetic, and then the rounding back to the significance the
operands justify. `SignificanceBenchmarks` measures that second half on its own, so it sets a floor
under everything in `ArithmeticBenchmarks`.
