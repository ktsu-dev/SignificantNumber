namespace ktsu.io.SignificantNumber;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

public static class SignificantNumberExtensions
{
	[SuppressMessage("Minor Code Smell", "S1199:Nested code blocks should not be used", Justification = "<Pending>")]
	public static SignificantNumber ToSignificantNumber<TInput>(this INumber<TInput> input)
		where TInput : INumber<TInput>
	{
		return TryCreate((TInput)input, out var significantNumber)
			? significantNumber
			: throw new NotSupportedException();
	}

	private static bool TryCreate<TInput>([NotNullWhen(true)] TInput input, [MaybeNullWhen(false)] out SignificantNumber significantNumber)
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
