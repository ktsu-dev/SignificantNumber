// Copyright (c) 2023-2026 ktsu-dev contributors

namespace ktsu.SignificantNumber.Benchmarks;

using System.Globalization;

/// <summary>
/// Builds the operands the benchmarks run against.
/// </summary>
/// <remarks>
/// Values are derived from a fixed digit pattern rather than a random source so that two runs of
/// the same benchmark, on the same machine or on different ones, are measuring the same work.
/// <para>
/// Everything is built by parsing text, because that is the only construction route the published
/// packages share. A benchmark that reached for an internal factory could measure the working copy
/// and nothing before it, which would leave the release chart with a single point.
/// </para>
/// </remarks>
internal static class Operands
{
	/// <summary>
	/// An arbitrary but fixed run of non-repeating digits to slice operands out of.
	/// </summary>
	private const string DigitPattern =
		"31415926535897932384626433832795028841971693993751" +
		"05820974944592307816406286208998628034825342117067" +
		"98214808651328230664709384460955058223172535940812" +
		"84811174502841027019385211055596446229489549303819";

	/// <summary>
	/// Renders decimal text with exactly <paramref name="digits"/> significant digits.
	/// </summary>
	/// <param name="digits">The number of significant digits.</param>
	/// <param name="exponent">The power of ten to scale by.</param>
	/// <param name="offset">Shifts the window into the digit pattern, so that two operands of the
	/// same length are not identical.</param>
	/// <returns>The decimal text, in scientific notation.</returns>
	internal static string Text(int digits, int exponent, int offset = 0)
	{
		char[] characters = new char[digits];
		for (int i = 0; i < digits; i++)
		{
			characters[i] = DigitPattern[(i + offset) % DigitPattern.Length];
		}

		// A leading or trailing zero would make the value's digit count differ from what was asked
		// for, because significance is counted from the first non-zero digit and trailing zeros are
		// stripped.
		if (characters[0] == '0')
		{
			characters[0] = '4';
		}

		if (characters[digits - 1] == '0')
		{
			characters[digits - 1] = '7';
		}

		return string.Create(CultureInfo.InvariantCulture, $"{new string(characters)}E{exponent}");
	}

	/// <summary>
	/// Builds a number with the given significant digit count and exponent.
	/// </summary>
	/// <param name="digits">The number of significant digits.</param>
	/// <param name="exponent">The power of ten to scale by.</param>
	/// <param name="offset">Shifts the window into the digit pattern.</param>
	/// <returns>The constructed number.</returns>
	internal static SignificantNumber Number(int digits, int exponent, int offset = 0) =>
		SignificantNumber.Parse(Text(digits, exponent, offset), CultureInfo.InvariantCulture);
}
