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

It is also why nothing here touches an internal member, and why there is no formatting benchmark.
Before 2.0 this type derived from `PreciseNumber` rather than wrapping it, so `ToString` was an
inherited member: measuring it would mean referencing that package here, and the reference makes
`Parse` and `GetHashCode` ambiguous against their inherited counterparts. One operation is not
worth the whole release history before 2.0.

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
