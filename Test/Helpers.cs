// Ignore Spelling: RNG

namespace ktsu.io.SignificantNumber.Test;
using System.Numerics;

internal static class Helpers
{
	public static Random RNG { get; set; } = new((int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());

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

	public static TNumber GetMaxValue<TNumber>()
		where TNumber : INumber<TNumber>
	{
		var type = typeof(TNumber);
		return type switch
		{
			_ when type == typeof(byte) => TNumber.CreateChecked(byte.MaxValue),
			_ when type == typeof(sbyte) => TNumber.CreateChecked(sbyte.MaxValue),
			_ when type == typeof(short) => TNumber.CreateChecked(short.MaxValue),
			_ when type == typeof(ushort) => TNumber.CreateChecked(ushort.MaxValue),
			_ when type == typeof(int) => TNumber.CreateChecked(int.MaxValue),
			_ when type == typeof(uint) => TNumber.CreateChecked(uint.MaxValue),
			_ when type == typeof(long) => TNumber.CreateChecked(long.MaxValue),
			_ when type == typeof(ulong) => TNumber.CreateChecked(long.MaxValue), // special case because random returns a signed int64
			_ when type == typeof(float) => TNumber.CreateChecked(float.MaxValue),
			_ when type == typeof(double) => TNumber.CreateChecked(double.MaxValue),
			_ when type == typeof(decimal) => TNumber.CreateChecked(79228162514264300000000000000m), // special case because we have 15 digits of precision
			_ when type == typeof(BigInteger) => TNumber.CreateChecked(long.MaxValue),
			_ when type == typeof(Half) => TNumber.CreateChecked(Half.MaxValue),
			_ => throw new NotSupportedException(),
		};
	}

	public static TNumber GetMinValue<TNumber>()
		where TNumber : INumber<TNumber>
	{
		var type = typeof(TNumber);
		return type switch
		{
			_ when type == typeof(byte) => TNumber.CreateChecked(byte.MinValue),
			_ when type == typeof(sbyte) => TNumber.CreateChecked(sbyte.MinValue + 1), // you cant use Abs on a signed number that is the minimum value
			_ when type == typeof(short) => TNumber.CreateChecked(short.MinValue + 1), // you cant use Abs on a signed number that is the minimum value
			_ when type == typeof(ushort) => TNumber.CreateChecked(ushort.MinValue),
			_ when type == typeof(int) => TNumber.CreateChecked(int.MinValue + 1), // you cant use Abs on a signed number that is the minimum value
			_ when type == typeof(uint) => TNumber.CreateChecked(uint.MinValue),
			_ when type == typeof(long) => TNumber.CreateChecked(long.MinValue + 1), // you cant use Abs on a signed number that is the minimum value
			_ when type == typeof(ulong) => TNumber.CreateChecked(ulong.MinValue),
			_ when type == typeof(float) => TNumber.CreateChecked(float.MinValue),
			_ when type == typeof(double) => TNumber.CreateChecked(double.MinValue),
			_ when type == typeof(decimal) => TNumber.CreateChecked(-79228162514264300000000000000m), // special case because we have 15 digits of precision
			_ when type == typeof(BigInteger) => TNumber.CreateChecked(long.MinValue),
			_ when type == typeof(Half) => TNumber.CreateChecked(Half.MinValue),
			_ => throw new NotSupportedException(),
		};
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
