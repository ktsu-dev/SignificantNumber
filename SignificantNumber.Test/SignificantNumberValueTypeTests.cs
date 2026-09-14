// Copyright (c) 2023-2026 ktsu-dev contributors

namespace SignificantNumber.Test;

using System.Globalization;
using System.Numerics;
using System.Text;
using ktsu.PreciseNumber;
using ktsu.SignificantNumber;

/// <summary>
/// Covers what changed when <see cref="SignificantNumber"/> became a value type that holds a <see cref="PreciseNumber"/>.
/// </summary>
[TestClass]
public class SignificantNumberValueTypeTests
{
	private static SignificantNumber Parse(string text) =>
		SignificantNumber.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);

	// The generic helpers call conversions the way generic numeric code does, through the static interface members.
	private static TTo ConvertChecked<TFrom, TTo>(TFrom value)
		where TFrom : INumberBase<TFrom>
		where TTo : INumberBase<TTo> =>
		TTo.CreateChecked(value);

	private static TTo ConvertSaturating<TFrom, TTo>(TFrom value)
		where TFrom : INumberBase<TFrom>
		where TTo : INumberBase<TTo> =>
		TTo.CreateSaturating(value);

	private static TTo ConvertTruncating<TFrom, TTo>(TFrom value)
		where TFrom : INumberBase<TFrom>
		where TTo : INumberBase<TTo> =>
		TTo.CreateTruncating(value);

	private static T Sum<T>(params T[] values)
		where T : INumber<T>
	{
		T total = T.Zero;
		foreach (T value in values)
		{
			total += value;
		}

		return total;
	}

	[TestMethod]
	public void Default_EqualsZero()
	{
		SignificantNumber value = default;

		Assert.AreEqual(SignificantNumber.Zero, value);
		Assert.IsTrue(SignificantNumber.IsZero(value), "default should be zero");
		Assert.AreEqual(0, value.Exponent);
		Assert.AreEqual(BigInteger.Zero, value.Significand);
		Assert.AreEqual(0, value.SignificantDigits);
		Assert.AreEqual(Parse("0").GetHashCode(), value.GetHashCode());
	}

	[TestMethod]
	public void Default_ArrayElementIsUsableZero()
	{
		SignificantNumber[] values = new SignificantNumber[3];

		Assert.AreEqual(SignificantNumber.Zero, values[1]);
		Assert.AreEqual(Parse("2.5"), values[1] + Parse("2.5"));
	}

	[TestMethod]
	public void ImplicitConversion_ToPreciseNumber_KeepsValue()
	{
		SignificantNumber significant = Parse("123.45");
		PreciseNumber precise = significant;

		Assert.AreEqual(significant.Value, precise);
		Assert.AreEqual(PreciseNumber.Parse("123.45", CultureInfo.InvariantCulture), precise);
	}

	[TestMethod]
	public void ExplicitConversion_FromPreciseNumber_HoldsValue()
	{
		PreciseNumber precise = PreciseNumber.Parse("123.45", CultureInfo.InvariantCulture);
		SignificantNumber significant = (SignificantNumber)precise;

		Assert.AreEqual(precise, significant.Value);
		Assert.AreEqual(significant, SignificantNumber.FromPreciseNumber(precise));
	}

	[TestMethod]
	public void ToPreciseNumber_ReturnsHeldValue()
	{
		SignificantNumber significant = Parse("0.001");

		Assert.AreEqual(significant.Value, significant.ToPreciseNumber());
	}

	[TestMethod]
	public void CreateChecked_FromDouble_IsExact()
	{
		SignificantNumber result = ConvertChecked<double, SignificantNumber>(0.3048);

		Assert.AreEqual(Parse("0.3048"), result);
	}

	[TestMethod]
	public void CreateChecked_FromInt_IsExact()
	{
		SignificantNumber result = ConvertChecked<int, SignificantNumber>(42);

		Assert.AreEqual(Parse("42"), result);
	}

	[TestMethod]
	public void CreateChecked_ToDouble_IsExact()
	{
		double result = ConvertChecked<SignificantNumber, double>(Parse("0.3048"));

		Assert.AreEqual(0.3048, result);
	}

	[TestMethod]
	public void CreateChecked_ToAndFromPreciseNumber_KeepsValue()
	{
		SignificantNumber significant = Parse("98.6");

		PreciseNumber precise = ConvertChecked<SignificantNumber, PreciseNumber>(significant);
		SignificantNumber roundTripped = ConvertChecked<PreciseNumber, SignificantNumber>(precise);

		Assert.AreEqual(significant.Value, precise);
		Assert.AreEqual(significant, roundTripped);
	}

	[TestMethod]
	public void CreateChecked_ToInt_OutOfRange_Throws()
	{
		SignificantNumber tooLarge = Parse("1e20");

		Assert.ThrowsExactly<OverflowException>(() => ConvertChecked<SignificantNumber, int>(tooLarge));
	}

	[TestMethod]
	public void CreateSaturating_ToByte_Clamps()
	{
		byte result = ConvertSaturating<SignificantNumber, byte>(Parse("300"));

		Assert.AreEqual(byte.MaxValue, result);
	}

	[TestMethod]
	public void CreateTruncating_ToInt_TruncatesTowardZero()
	{
		int result = ConvertTruncating<SignificantNumber, int>(Parse("12.9"));

		Assert.AreEqual(12, result);
	}

	[TestMethod]
	public void GenericSum_AppliesDecimalPlaceRule()
	{
		// 1.25 + 2.1 is 3.35, which the decimal place rule rounds to one decimal place. Zero, the starting total,
		// has unlimited precision, so it doesn't limit the first addition.
		SignificantNumber result = Sum(Parse("1.25"), Parse("2.1"));

		Assert.AreEqual(Parse("3.4"), result);
	}

	[TestMethod]
	public void StaticMaxMinClamp_CompareByValue()
	{
		SignificantNumber low = Parse("1.5");
		SignificantNumber high = Parse("2.5");

		Assert.AreEqual(high, SignificantNumber.Max(low, high));
		Assert.AreEqual(low, SignificantNumber.Min(low, high));
		Assert.AreEqual(high, SignificantNumber.Clamp(Parse("9"), low, high));
		Assert.AreEqual(Parse("1.2"), SignificantNumber.Round(Parse("1.23"), 1));
	}

	[TestMethod]
	public void TryParse_Invalid_YieldsZero()
	{
		bool parsed = SignificantNumber.TryParse("not a number", NumberStyles.Float, CultureInfo.InvariantCulture, out SignificantNumber result);

		Assert.IsFalse(parsed, "TryParse should fail for text that isn't a number");
		Assert.AreEqual(SignificantNumber.Zero, result);
	}

	[TestMethod]
	public void CompareToObject_Null_SortsFirst()
	{
		object? nothing = null;
		int result = Parse("5").CompareTo(nothing);

		Assert.AreEqual(1, result);
	}

	[TestMethod]
	public void CompareToObject_BoxedPreciseNumber_ComparesValue()
	{
		object precise = PreciseNumber.Parse("5", CultureInfo.InvariantCulture);

		Assert.AreEqual(0, Parse("5").CompareTo(precise));
	}

	[TestMethod]
	public void ToString_MatchesHeldValue()
	{
		SignificantNumber significant = Parse("123.45");

		Assert.AreEqual("123.45", significant.ToString(CultureInfo.InvariantCulture));
		Assert.AreEqual(significant.Value.ToString(CultureInfo.InvariantCulture), significant.ToString(CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void Utf8TryFormat_WritesSameTextAsToString()
	{
		SignificantNumber significant = Parse("123.45");
		Span<byte> buffer = stackalloc byte[64];

		bool formatted = ((IUtf8SpanFormattable)significant).TryFormat(buffer, out int bytesWritten, default, CultureInfo.InvariantCulture);

		Assert.IsTrue(formatted, "Formatting a short number into a 64 byte buffer should succeed");
		Assert.AreEqual(significant.ToString(CultureInfo.InvariantCulture), Encoding.UTF8.GetString(buffer[..bytesWritten]));
	}

	[TestMethod]
	public void ToSignificantNumberWithDigits_FromSignificantNumber_ReducesSignificance()
	{
		SignificantNumber result = Parse("123.456").ToSignificantNumber(3);

		Assert.AreEqual(Parse("123"), result);
	}
}
