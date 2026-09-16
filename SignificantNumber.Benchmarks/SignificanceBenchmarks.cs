// Copyright (c) 2023-2026 ktsu-dev contributors

namespace ktsu.SignificantNumber.Benchmarks;

using BenchmarkDotNet.Attributes;

/// <summary>
/// Measures reducing a number to a stated number of significant digits.
/// </summary>
/// <remarks>
/// This is the operation the library is named for, and the one every arithmetic result pays for
/// internally, so its cost sets a floor under everything in <see cref="ArithmeticBenchmarks"/>.
/// The work tracks how many digits are being discarded rather than how many are kept.
/// </remarks>
[MemoryDiagnoser]
public class SignificanceBenchmarks
{
	// Assigned in GlobalSetup before anything is measured. Initialised here because this type
	// was a class before 2.0, where an unassigned field is a null reference the compiler
	// rejects; from 2.0 it is a struct and this is simply its default.
	private SignificantNumber number = default!;

	/// <summary>
	/// Gets or sets the number of significant digits in the operand.
	/// </summary>
	[Params(8, 30, 200)]
	public int Digits { get; set; }

	/// <summary>
	/// Prepares the operand.
	/// </summary>
	[GlobalSetup]
	public void Setup() => number = Operands.Number(Digits, -Digits);

	/// <summary>Reduces to three significant digits, discarding most of them.</summary>
	/// <returns>The reduced number.</returns>
	[Benchmark]
	public SignificantNumber ReduceToThree() => number.ToSignificantNumber(3);

	/// <summary>Reduces to half the digits it has, so the work scales with the operand.</summary>
	/// <returns>The reduced number.</returns>
	[Benchmark]
	public SignificantNumber ReduceToHalf() => number.ToSignificantNumber(Digits / 2);
}
