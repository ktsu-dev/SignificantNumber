// Copyright (c) 2023-2026 ktsu-dev contributors

namespace ktsu.SignificantNumber.Benchmarks;

using System.Globalization;

using BenchmarkDotNet.Attributes;

/// <summary>
/// Measures parsing.
/// </summary>
/// <remarks>
/// Parsing is how every operand in this suite is built, so its cost is worth knowing on its own
/// rather than only as part of a setup step.
/// <para>
/// Formatting is deliberately absent. Before 2.0 this type derived from <c>PreciseNumber</c> and
/// inherited its <c>ToString</c>, so measuring it would mean referencing that package here, and
/// the reference makes <c>Parse</c> and <c>GetHashCode</c> ambiguous against the inherited
/// members — which would cost the whole release history before 2.0 to measure one operation.
/// </para>
/// </remarks>
[MemoryDiagnoser]
public class TextBenchmarks
{
	// Left to default rather than initialised from a named constant: the constant resolves
	// through the transitive PreciseNumber package, which this project deliberately does not
	// reference, and GlobalSetup assigns both before anything is measured.
	private string text = "";

	/// <summary>
	/// Gets or sets the number of significant digits in the operand.
	/// </summary>
	[Params(8, 30, 200)]
	public int Digits { get; set; }

	/// <summary>
	/// Prepares the text.
	/// </summary>
	[GlobalSetup]
	public void Setup() => text = Operands.Text(Digits, -Digits);

	/// <summary>Parses decimal text in scientific notation.</summary>
	/// <returns>The parsed number.</returns>
	[Benchmark]
	public SignificantNumber Parse() => SignificantNumber.Parse(text, CultureInfo.InvariantCulture);
}
