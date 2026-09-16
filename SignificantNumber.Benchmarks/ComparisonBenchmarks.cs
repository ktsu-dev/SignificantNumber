// Copyright (c) 2023-2026 ktsu-dev contributors

namespace ktsu.SignificantNumber.Benchmarks;

using BenchmarkDotNet.Attributes;

/// <summary>
/// Measures comparing and hashing.
/// </summary>
/// <remarks>
/// Comparison reduces both operands to the significance they share before deciding, so unlike the
/// number underneath it these are not simply a look at two fields.
/// </remarks>
[MemoryDiagnoser]
public class ComparisonBenchmarks
{
	// Assigned in GlobalSetup before anything is measured. Initialised here because this type
	// was a class before 2.0, where an unassigned field is a null reference the compiler
	// rejects; from 2.0 it is a struct and this is simply its default.
	private SignificantNumber left = default!;
	private SignificantNumber right = default!;
	private SignificantNumber same = default!;

	/// <summary>
	/// Gets or sets the number of significant digits in the operands.
	/// </summary>
	[Params(8, 30, 200)]
	public int Digits { get; set; }

	/// <summary>
	/// Prepares the operands.
	/// </summary>
	[GlobalSetup]
	public void Setup()
	{
		left = Operands.Number(Digits, -Digits);
		right = Operands.Number(Digits, -Digits, offset: 7);
		same = Operands.Number(Digits, -Digits);
	}

	/// <summary>Compares two equal numbers.</summary>
	/// <returns>Whether they are equal.</returns>
	[Benchmark]
	public bool EqualsSame() => left == same;

	/// <summary>Compares two different numbers.</summary>
	/// <returns>Whether they are equal.</returns>
	[Benchmark]
	public bool EqualsDifferent() => left == right;

	/// <summary>Orders two numbers.</summary>
	/// <returns>Whether the left is smaller.</returns>
	[Benchmark]
	public bool LessThan() => left < right;

	/// <summary>Orders two numbers, returning the sign of their difference.</summary>
	/// <returns>The comparison result.</returns>
	[Benchmark]
	public int CompareTo() => left.CompareTo(right);

	/// <summary>Hashes a number.</summary>
	/// <returns>The hash code.</returns>
	[Benchmark]
	public int GetHashCodeBenchmark() => left.GetHashCode();
}
