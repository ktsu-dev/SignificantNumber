namespace ktsu.io.SignificantNumber;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

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
		// if TInput is already a SignificantNumber then just return it
		SignificantNumber significantNumber;
		bool success = typeof(TInput) == typeof(SignificantNumber);

		if (success)
		{
			significantNumber = (SignificantNumber)(object)input;
		}
		else
		{
			success = TryCreate((TInput)input, out significantNumber);
		}

		return success
			? significantNumber
			: throw new NotSupportedException();
	}

	/// <summary>
	/// Tries to create a <see cref="SignificantNumber"/> from the input.
	/// </summary>
	/// <typeparam name="TInput">The type of the input number.</typeparam>
	/// <param name="input">The input number to create a <see cref="SignificantNumber"/> from.</param>
	/// <param name="significantNumber">The created <see cref="SignificantNumber"/> if successful, otherwise null.</param>
	/// <returns>True if the creation was successful, otherwise false.</returns>
	internal static bool TryCreate<TInput>([NotNullWhen(true)] TInput input, [MaybeNullWhen(false)] out SignificantNumber significantNumber)
		where TInput : INumber<TInput>
	{
		var type = typeof(TInput);
		if (Array.Exists(type.GetInterfaces(), i => i.Name.StartsWith("IBinaryInteger", StringComparison.Ordinal)))
		{
			significantNumber = SignificantNumber.CreateFromInteger(input);
			return true;
		}

		if (Array.Exists(type.GetInterfaces(), i => i.Name.StartsWith("IFloatingPoint", StringComparison.Ordinal)))
		{
			significantNumber = SignificantNumber.CreateFromFloatingPoint(input);
			return true;
		}

		significantNumber = default;
		return false;
	}
}
