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
	public static SignificantNumber ToSignificantNumber<TInput>(this INumber<TInput> input)
		where TInput : INumber<TInput>
	{
		bool success = typeof(TInput) == typeof(SignificantNumber);

		return success
			? (SignificantNumber)(object)input
			: (SignificantNumber)input.ToPreciseNumber();
	}
}
