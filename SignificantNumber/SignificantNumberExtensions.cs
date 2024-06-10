namespace ktsu.io.SignificantNumber;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

public static class SignificantNumberExtensions
{
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
