// Copyright (c) 2023-2026 ktsu-dev contributors

namespace ktsu.SignificantNumber.Benchmarks;

using BenchmarkDotNet.Attributes;

/// <summary>
/// Measures the arithmetic operators.
/// </summary>
/// <remarks>
/// Every operation here does two things: the arithmetic itself, and then the rounding back to the
/// significance the operands justify. The second half is what separates this library from the
/// number underneath it, and it is why these cost more than the same operation on a
/// <c>PreciseNumber</c>.
/// </remarks>
[MemoryDiagnoser]
public class ArithmeticBenchmarks
{
	// Assigned in GlobalSetup before anything is measured. Initialised here because this type
	// was a class before 2.0, where an unassigned field is a null reference the compiler
	// rejects; from 2.0 it is a struct and this is simply its default.
	private SignificantNumber left = default!;
	private SignificantNumber right = default!;

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
	}

	/// <summary>Adds two numbers.</summary>
	/// <returns>The sum.</returns>
	[Benchmark]
	public SignificantNumber Add() => left + right;

	/// <summary>Subtracts one number from another.</summary>
	/// <returns>The difference.</returns>
	[Benchmark]
	public SignificantNumber Subtract() => left - right;

	/// <summary>Multiplies two numbers.</summary>
	/// <returns>The product.</returns>
	[Benchmark]
	public SignificantNumber Multiply() => left * right;

	/// <summary>Divides one number by another.</summary>
	/// <returns>The quotient.</returns>
	[Benchmark]
	public SignificantNumber Divide() => left / right;

	/// <summary>Negates a number.</summary>
	/// <returns>The negated number.</returns>
	[Benchmark]
	public SignificantNumber Negate() => -left;
}
