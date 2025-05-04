// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

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
	/// If the input number is already a <see cref="SignificantNumber"/>, it is returned as-is.
	/// Otherwise, the input is converted to a <see cref="PreciseNumber"/> and then to a <see cref="SignificantNumber"/>.
	/// </remarks>
	public static SignificantNumber ToSignificantNumber<TInput>(this INumber<TInput> input)
		where TInput : INumber<TInput>
	{
		ArgumentNullException.ThrowIfNull(input);

		var inputType = input.GetType();
		var significantNumberType = typeof(SignificantNumber);
		var isSignificantNumber = inputType == significantNumberType || inputType.IsSubclassOf(significantNumberType);

		if (isSignificantNumber)
		{
			return (SignificantNumber)(object)input;
		}

		var preciseNumber = input.ToPreciseNumber();

		return SignificantNumber.CreateFromComponents(preciseNumber.Exponent, preciseNumber.Significand);
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
	public static SignificantNumber ToSignificantNumber<TInput>(this INumber<TInput> input, int significantDigits)
		where TInput : INumber<TInput>
	{
		if (significantDigits <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(significantDigits), "Significant digits must be greater than zero.");
		}

		var preciseNumber = input
			.ToPreciseNumber()
			.ReduceSignificance(significantDigits);

		return SignificantNumber.CreateFromComponents(preciseNumber.Exponent, preciseNumber.Significand);
	}
}
