namespace ktsu.io.SignificantNumber;

using System.Numerics;

public static class SignificantNumberExtensions
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1199:Nested code blocks should not be used", Justification = "<Pending>")]
	public static SignificantNumber ToSignificantNumber<TInput>(this INumber<TInput> input)
		where TInput : INumber<TInput>
	{
		{
			if (input is sbyte value)
			{
				return SignificantNumber.CreateFromInteger(value);
			}
		}

		{
			if (input is byte value)
			{
				return SignificantNumber.CreateFromInteger(value);
			}
		}

		{
			if (input is short value)
			{
				return SignificantNumber.CreateFromInteger(value);
			}
		}

		{
			if (input is ushort value)
			{
				return SignificantNumber.CreateFromInteger(value);
			}
		}

		{
			if (input is int value)
			{
				return SignificantNumber.CreateFromInteger(value);
			}
		}

		{
			if (input is uint value)
			{
				return SignificantNumber.CreateFromInteger(value);
			}
		}

		{
			if (input is long value)
			{
				return SignificantNumber.CreateFromInteger(value);
			}
		}

		{
			if (input is ulong value)
			{
				return SignificantNumber.CreateFromInteger(value);
			}
		}

		{
			if (input is BigInteger value)
			{
				return SignificantNumber.CreateFromInteger(value);
			}
		}

		{
			if (input is float value)
			{
				return SignificantNumber.CreateFromFloatingPoint(value);
			}
		}

		{
			if (input is double value)
			{
				return SignificantNumber.CreateFromFloatingPoint(value);
			}
		}

		{
			if (input is decimal value)
			{
				return SignificantNumber.CreateFromFloatingPoint(value);
			}
		}

		throw new NotSupportedException();
	}
}
