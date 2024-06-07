namespace ktsu.io.SignificantNumber.Test;
using System.Numerics;

internal static class Helpers
{
	public static Random RNG { get; set; } = new(int.CreateTruncating(DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds));

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>")]
	public static TNumber RandomNumber<TNumber>()
		where TNumber : INumber<TNumber>
	{
		if (Array.Exists(typeof(TNumber).GetInterfaces(), i => i.Name.StartsWith("IBinaryInteger", StringComparison.Ordinal)))
		{
			long min = GetMinValueAsLong<TNumber>();
			long max = GetMaxValueAsLong<TNumber>();
			return TNumber.CreateChecked(RNG.NextInt64(min, max));
		}

		if (Array.Exists(typeof(TNumber).GetInterfaces(), i => i.Name.StartsWith("IFloatingPoint", StringComparison.Ordinal)))
		{
			bool isSigned = TNumber.IsNegative(GetMinValue<TNumber>());
			double multiplier = RNG.NextDouble();
			bool negative = RNG.NextDouble() > 0.5;
			double min = GetMinValueAsDouble<TNumber>();
			double max = GetMaxValueAsDouble<TNumber>();
			double value = (negative && isSigned ? min : max) * multiplier;
			return TNumber.CreateChecked(value);
		}

		throw new NotSupportedException();
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>")]
	public static TNumber GetMaxValue<TNumber>()
		where TNumber : INumber<TNumber>
	{
		if (typeof(TNumber) == typeof(byte))
		{
			return TNumber.CreateChecked(byte.MaxValue);
		}

		if (typeof(TNumber) == typeof(sbyte))
		{
			return TNumber.CreateChecked(sbyte.MaxValue);
		}

		if (typeof(TNumber) == typeof(short))
		{
			return TNumber.CreateChecked(short.MaxValue);
		}

		if (typeof(TNumber) == typeof(ushort))
		{
			return TNumber.CreateChecked(ushort.MaxValue);
		}

		if (typeof(TNumber) == typeof(int))
		{
			return TNumber.CreateChecked(int.MaxValue);
		}

		if (typeof(TNumber) == typeof(uint))
		{
			return TNumber.CreateChecked(uint.MaxValue);
		}

		if (typeof(TNumber) == typeof(long))
		{
			return TNumber.CreateChecked(long.MaxValue);
		}

		if (typeof(TNumber) == typeof(ulong))
		{
			return TNumber.CreateChecked(long.MaxValue); // special case because random returns a signed int64
		}

		if (typeof(TNumber) == typeof(float))
		{
			return TNumber.CreateChecked(float.MaxValue);
		}

		if (typeof(TNumber) == typeof(double))
		{
			return TNumber.CreateChecked(double.MaxValue);
		}

		if (typeof(TNumber) == typeof(decimal))
		{
			return TNumber.CreateChecked(79228162514264300000000000000m); // special case because we have 15 digits of precision
		}

		if (typeof(TNumber) == typeof(BigInteger))
		{
			return TNumber.CreateChecked(long.MaxValue);
		}

		throw new NotSupportedException();
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>")]
	public static TNumber GetMinValue<TNumber>()
		where TNumber : INumber<TNumber>
	{
		if (typeof(TNumber) == typeof(byte))
		{
			return TNumber.CreateChecked(byte.MinValue);
		}

		if (typeof(TNumber) == typeof(sbyte))
		{
			return TNumber.CreateChecked(sbyte.MinValue + 1); // you cant use Abs on a signed number that is the minimum value
		}

		if (typeof(TNumber) == typeof(short))
		{
			return TNumber.CreateChecked(short.MinValue + 1); // you cant use Abs on a signed number that is the minimum value
		}

		if (typeof(TNumber) == typeof(ushort))
		{
			return TNumber.CreateChecked(ushort.MinValue);
		}

		if (typeof(TNumber) == typeof(int))
		{
			return TNumber.CreateChecked(int.MinValue + 1); // you cant use Abs on a signed number that is the minimum value
		}

		if (typeof(TNumber) == typeof(uint))
		{
			return TNumber.CreateChecked(uint.MinValue);
		}

		if (typeof(TNumber) == typeof(long))
		{
			return TNumber.CreateChecked(long.MinValue + 1); // you cant use Abs on a signed number that is the minimum value
		}

		if (typeof(TNumber) == typeof(ulong))
		{
			return TNumber.CreateChecked(ulong.MinValue);
		}

		if (typeof(TNumber) == typeof(float))
		{
			return TNumber.CreateChecked(float.MinValue);
		}

		if (typeof(TNumber) == typeof(double))
		{
			return TNumber.CreateChecked(double.MinValue);
		}

		if (typeof(TNumber) == typeof(decimal))
		{
			return TNumber.CreateChecked(-79228162514264300000000000000m); // special case because we have 15 digits of precision
		}

		if (typeof(TNumber) == typeof(BigInteger))
		{
			return TNumber.CreateChecked(long.MinValue);
		}

		throw new NotSupportedException();
	}

	public static long GetMinValueAsLong<TNumber>()
		where TNumber : INumber<TNumber> => long.CreateChecked(GetMinValue<TNumber>());

	public static long GetMaxValueAsLong<TNumber>()
		where TNumber : INumber<TNumber> => long.CreateChecked(GetMaxValue<TNumber>());

	public static double GetMaxValueAsDouble<TNumber>()
		where TNumber : INumber<TNumber> => double.CreateChecked(GetMaxValue<TNumber>());

	public static double GetMinValueAsDouble<TNumber>()
		where TNumber : INumber<TNumber> => double.CreateChecked(GetMinValue<TNumber>());
}
