// Copyright (c) 2023-2026 ktsu-dev contributors

namespace ktsu.SignificantNumber;

using System.Numerics;

using ktsu.PreciseNumber;

/// <summary>
/// Provides extension methods for converting numbers to <see cref="SignificantNumber"/>.
/// </summary>
public static class SignificantNumberExtensions
{
	/// <summary>
	/// Converts the input number to a <see cref="SignificantNumber"/>.
	/// </summary>
	/// <typeparam name="TInput">The type of the input number.</typeparam>
	/// <param name="input">The input number to convert.</param>
	/// <returns>The converted <see cref="SignificantNumber"/>.</returns>
	/// <remarks>
	/// If the input number is already a <see cref="SignificantNumber"/>, it is returned unchanged.
	/// Otherwise, the input is converted to a <see cref="PreciseNumber"/>, which the result holds.
	/// </remarks>
	public static SignificantNumber ToSignificantNumber<TInput>(this TInput input)
		where TInput : INumber<TInput>
	{
		// Ensure.NotNull cannot be used with INumber<T> due to CS8920 (static abstract interface members)
#pragma warning disable KTSU0003
		ArgumentNullException.ThrowIfNull(input);
#pragma warning restore KTSU0003

		return typeof(TInput) == typeof(SignificantNumber)
			? (SignificantNumber)(object)input
			: new SignificantNumber(ToPreciseNumberValue(input));
	}

	/// <summary>
	/// Converts the input number to a <see cref="SignificantNumber"/> with a specified number of significant digits.
	/// </summary>
	/// <typeparam name="TInput">The type of the input number.</typeparam>
	/// <param name="input">The input number to convert.</param>
	/// <param name="significantDigits">The number of significant digits to retain in the resulting <see cref="SignificantNumber"/>.</param>
	/// <returns>The converted <see cref="SignificantNumber"/> with the specified number of significant digits.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="significantDigits"/> is less than or equal to zero.
	/// </exception>
	public static SignificantNumber ToSignificantNumber<TInput>(this TInput input, int significantDigits)
		where TInput : INumber<TInput>
	{
		if (significantDigits <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(significantDigits), "Significant digits must be greater than zero.");
		}

		PreciseNumber preciseNumber = ToPreciseNumberValue(input)
			.ReduceSignificance(significantDigits);

		return new SignificantNumber(preciseNumber);
	}

	/// <summary>
	/// Converts a number to a <see cref="PreciseNumber"/>, unwrapping a <see cref="SignificantNumber"/> directly.
	/// </summary>
	/// <typeparam name="TInput">The type of the input number.</typeparam>
	/// <param name="input">The input number to convert.</param>
	/// <returns>The input as a <see cref="PreciseNumber"/>.</returns>
	/// <remarks>
	/// <see cref="PreciseNumberExtensions.ToPreciseNumber{TInput}(TInput)"/> recognizes only built-in numeric types and
	/// <see cref="PreciseNumber"/> itself, so a <see cref="SignificantNumber"/> has to be unwrapped here.
	/// </remarks>
	private static PreciseNumber ToPreciseNumberValue<TInput>(TInput input)
		where TInput : INumber<TInput> =>
		typeof(TInput) == typeof(SignificantNumber)
			? ((SignificantNumber)(object)input).Value
			: input.ToPreciseNumber();
}
