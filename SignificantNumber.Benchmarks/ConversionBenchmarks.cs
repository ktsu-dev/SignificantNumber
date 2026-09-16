// Copyright (c) 2023-2026 ktsu-dev contributors

namespace ktsu.SignificantNumber.Benchmarks;

using BenchmarkDotNet.Attributes;

/// <summary>
/// Measures conversion to and from the primitive numeric types.
/// </summary>
/// <remarks>
/// These are the boundary a caller crosses to get a value in and an answer out, so they are on the
/// path of anything that uses the library at all rather than only of code doing arithmetic in it.
/// </remarks>
[MemoryDiagnoser]
public class ConversionBenchmarks
{
	// Read from fields rather than written as literals, so that the JIT cannot fold a conversion
	// into its own answer at compile time.
	private double doubleValue;
	private int intValue;
	// Assigned in GlobalSetup before anything is measured. Initialised here because this type
	// was a class before 2.0, where an unassigned field is a null reference the compiler
	// rejects; from 2.0 it is a struct and this is simply its default.
	private SignificantNumber number = default!;

	/// <summary>
	/// Prepares the operands.
	/// </summary>
	[GlobalSetup]
	public void Setup()
	{
		doubleValue = 123.456789;
		intValue = 123456;
		number = Operands.Number(30, -15);
	}

	/// <summary>Converts a double in.</summary>
	/// <returns>The converted number.</returns>
	[Benchmark]
	public SignificantNumber FromDouble() => doubleValue.ToSignificantNumber();

	/// <summary>Converts an int in.</summary>
	/// <returns>The converted number.</returns>
	[Benchmark]
	public SignificantNumber FromInt32() => intValue.ToSignificantNumber();

	/// <summary>Converts out to a double.</summary>
	/// <returns>The converted value.</returns>
	[Benchmark]
	public double ToDouble() => double.CreateTruncating(number);
}
