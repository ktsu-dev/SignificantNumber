// Copyright (c) 2023-2026 ktsu-dev contributors

namespace SignificantNumber.Test;

using System.Globalization;
using ktsu.SignificantNumber;

/// <summary>
/// Covers <see cref="SignificantNumber.ToString(string?, IFormatProvider?)"/> and
/// <see cref="SignificantNumber.TryFormat"/>, which must write every format <see cref="decimal"/> writes.
/// </summary>
[TestClass]
public class SignificantNumberFormatTests
{
	private static readonly CultureInfo CommaDecimalCulture = CreateCommaDecimalCulture();

	/// <summary>
	/// Builds a culture that writes 1.234,5, like de-DE, without depending on the host's culture data.
	/// </summary>
	private static CultureInfo CreateCommaDecimalCulture()
	{
		CultureInfo culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
		NumberFormatInfo format = culture.NumberFormat;
		format.NumberDecimalSeparator = ",";
		format.NumberGroupSeparator = ".";
		format.CurrencyDecimalSeparator = ",";
		format.CurrencyGroupSeparator = ".";
		format.PercentDecimalSeparator = ",";
		format.PercentGroupSeparator = ".";
		return culture;
	}

	private static CultureInfo Culture(bool commaDecimal) => commaDecimal ? CommaDecimalCulture : CultureInfo.InvariantCulture;

	private static SignificantNumber Parse(string text) =>
		SignificantNumber.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);

	[TestMethod]
	[DataRow("1234.5", "F2", false)]
	[DataRow("1234.5", "N2", false)]
	[DataRow("1234.5", "E2", false)]
	[DataRow("1234.5", "C", false)]
	[DataRow("1234.5", "C0", false)]
	[DataRow("1234.5", "P", false)]
	[DataRow("1234.5", "P1", false)]
	[DataRow("1234.5", "0.00", false)]
	[DataRow("1234.5", "#,##0.0", false)]
	[DataRow("1234.5", "F2", true)]
	[DataRow("1234.5", "N2", true)]
	[DataRow("1234.5", "E2", true)]
	[DataRow("1234.5", "C", true)]
	[DataRow("1234.5", "P1", true)]
	[DataRow("1234.5", "0.00", true)]
	[DataRow("1234.5", "#,##0.0", true)]
	[DataRow("-0.00125", "C", false)]
	[DataRow("-0.00125", "P", false)]
	[DataRow("-0.00125", "0.0000", true)]
	[DataRow("0", "C", false)]
	[DataRow("0", "P", true)]
	[DataRow("100000000000000000000", "C", false)]
	[DataRow("100000000000000000000", "#,##0.0", true)]
	public void ToString_Format_MatchesDecimal(string value, string format, bool commaDecimal)
	{
		CultureInfo culture = Culture(commaDecimal);
		string expected = decimal.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture).ToString(format, culture);

		Assert.AreEqual(expected, Parse(value).ToString(format, culture));
	}

	[TestMethod]
	[DataRow("1234.5", "C", false)]
	[DataRow("1234.5", "P1", true)]
	[DataRow("1234.5", "0.00", true)]
	[DataRow("1234.5", "N2", false)]
	public void TryFormat_Format_MatchesDecimal(string value, string format, bool commaDecimal)
	{
		CultureInfo culture = Culture(commaDecimal);
		string expected = decimal.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture).ToString(format, culture);
		Span<char> destination = stackalloc char[64];

		Assert.IsTrue(Parse(value).TryFormat(destination, out int charsWritten, format, culture));
		Assert.AreEqual(expected, destination[..charsWritten].ToString());
	}

	[TestMethod]
	[DataRow("F2")]
	[DataRow("N2")]
	[DataRow("C")]
	[DataRow("0.00")]
	public void TryFormat_DestinationTooSmall_ReturnsFalse(string format)
	{
		Span<char> destination = stackalloc char[2];

		Assert.IsFalse(Parse("1234.5").TryFormat(destination, out _, format, CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void Interpolation_WithFormatSpecifier_UsesTheFormat()
	{
		SignificantNumber value = Parse("1234.5");

		Assert.AreEqual("1234.5", string.Create(CultureInfo.InvariantCulture, $"{value:F1}"));
		Assert.AreEqual("1,234.50", string.Format(CultureInfo.InvariantCulture, "{0:N2}", value));
		Assert.AreEqual("1234.50", string.Create(CultureInfo.InvariantCulture, $"{value:0.00}"));
		Assert.AreEqual("¤1,234.50", string.Format(CultureInfo.InvariantCulture, "{0:C}", value));
	}

	[TestMethod]
	public void ToString_CustomFormat_OutsideDecimalRange_FormatsLikeDouble()
	{
		SignificantNumber value = Parse("1E+30");

		Assert.AreEqual(1e30.ToString("0.###E+0", CultureInfo.InvariantCulture), value.ToString("0.###E+0", CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void ToString_CustomFormat_BeyondDecimalPrecision_FormatsLikeDouble()
	{
		SignificantNumber value = Parse("1E-40");

		Assert.AreEqual(1e-40.ToString("0.##E+0", CultureInfo.InvariantCulture), value.ToString("0.##E+0", CultureInfo.InvariantCulture));
	}

	[TestMethod]
	[DataRow("1E400", "C")]
	[DataRow("1E400", "P")]
	[DataRow("1E400", "0.##E+0")]
	[DataRow("1E400", "#,##0")]
	[DataRow("-1E400", "#,##0")]
	[DataRow("1E-400", "C")]
	[DataRow("1E-400", "P")]
	[DataRow("1E-400", "0.##E+0")]
	[DataRow("-1E-400", "#,##0")]
	public void ToString_FormatOutsideDoubleRange_Throws(string value, string format)
	{
		SignificantNumber number = Parse(value);

		Assert.ThrowsExactly<FormatException>(() => number.ToString(format, CultureInfo.InvariantCulture));
	}

	[TestMethod]
	[DataRow("1E400", "C")]
	[DataRow("1E400", "P")]
	[DataRow("1E400", "0.##E+0")]
	[DataRow("1E400", "#,##0")]
	[DataRow("1E-400", "0.##E+0")]
	[DataRow("-1E-400", "#,##0")]
	public void TryFormat_FormatOutsideDoubleRange_Throws(string value, string format)
	{
		SignificantNumber number = Parse(value);
		char[] destination = new char[512];

		Assert.ThrowsExactly<FormatException>(() => number.TryFormat(destination, out _, format, CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void ToString_ExactFormatOutsideDoubleRange_StillWritesTheValue()
	{
		Assert.AreEqual("1.00E+400", Parse("1E400").ToString("E2", CultureInfo.InvariantCulture));
	}

	[TestMethod]
	[DataRow("0.123456789012345678901234567891", "0.0000000000000000000000000")]
	[DataRow("123.456789012345678901234567891", "#,##0.00000000000000000000000")]
	[DataRow("-0.123456789012345678901234567891", "C25")]
	public void ToString_CustomFormat_MoreDigitsThanDecimal_MatchesDecimalRounding(string value, string format)
	{
		string expected = decimal.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture).ToString(format, CultureInfo.InvariantCulture);
		Span<char> destination = stackalloc char[128];

		Assert.AreEqual(expected, Parse(value).ToString(format, CultureInfo.InvariantCulture));
		Assert.IsTrue(Parse(value).TryFormat(destination, out int charsWritten, format, CultureInfo.InvariantCulture));
		Assert.AreEqual(expected, destination[..charsWritten].ToString());
	}

	[TestMethod]
	[DataRow("1.00499999999999999999999999999999", "C2", "¤1.00")]
	[DataRow("1.00499999999999999999999999999999", "0.00", "1.00")]
	[DataRow("-1.00499999999999999999999999999999", "C2", "(¤1.00)")]
	[DataRow("-1.00499999999999999999999999999999", "0.00", "-1.00")]
	[DataRow("0.00499999999999999999999999999999", "C2", "¤0.00")]
	[DataRow("0.00499999999999999999999999999999", "0.00", "0.00")]
	[DataRow("2.4999999999999999999999999999999", "0", "2")]
	[DataRow("2.4999999999999999999999999999999", "C0", "¤2")]
	[DataRow("0.0049999999999999999999999999999", "P0", "0 %")]
	[DataRow("1.00500000000000000000000000000001", "C2", "¤1.01")]
	[DataRow("1.00500000000000000000000000000001", "0.00", "1.01")]
	[DataRow("-1.00500000000000000000000000000001", "0.00", "-1.01")]
	[DataRow("2.5000000000000000000000000000001", "0", "3")]
	public void ToString_CustomFormat_MoreDigitsThanDecimal_RoundsOnlyOnce(string value, string format, string expected)
	{
		Span<char> destination = stackalloc char[128];

		Assert.AreEqual(expected, Parse(value).ToString(format, CultureInfo.InvariantCulture));
		Assert.IsTrue(Parse(value).TryFormat(destination, out int charsWritten, format, CultureInfo.InvariantCulture));
		Assert.AreEqual(expected, destination[..charsWritten].ToString());
	}

	[TestMethod]
	[DataRow("1.00499999999999999999999999999999")]
	[DataRow("-1.00499999999999999999999999999999")]
	[DataRow("0.00499999999999999999999999999999")]
	public void ToString_CustomFormat_MoreDigitsThanDecimal_AgreesWithFixedPoint(string value)
	{
		SignificantNumber number = Parse(value);

		Assert.AreEqual(number.ToString("F2", CultureInfo.InvariantCulture), number.ToString("0.00", CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void ToString_CustomFormat_TooSmallForDecimalToKeepDoublePrecision_FormatsLikeDouble()
	{
		SignificantNumber value = Parse("1.234567890123456789E-20");

		Assert.AreEqual(1.234567890123456789E-20.ToString("0.##############E+0", CultureInfo.InvariantCulture), value.ToString("0.##############E+0", CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void ToString_FormatWithoutProvider_UsesCurrentCulture()
	{
		CultureInfo original = CultureInfo.CurrentCulture;
		try
		{
			CultureInfo.CurrentCulture = CommaDecimalCulture;
			Assert.AreEqual("1234,50", Parse("1234.5").ToString("0.00"));
		}
		finally
		{
			CultureInfo.CurrentCulture = original;
		}
	}

	[TestMethod]
	[DataRow(null, "1234,5")]
	[DataRow("G", "1234,5")]
	[DataRow("F2", "1234,50")]
	[DataRow("N2", "1.234,50")]
	[DataRow("E2", "1,23E+003")]
	[DataRow("0.00", "1234,50")]
	[DataRow("#,##0.00", "1.234,50")]
	public void ToString_NullProvider_UsesCurrentCultureForEveryFormat(string? format, string expected)
	{
		SignificantNumber value = Parse("1234.5");
		char[] destination = new char[64];

		WithCurrentCulture(CommaDecimalCulture, () =>
		{
			Assert.AreEqual(expected, value.ToString(format, null));
			Assert.IsTrue(value.TryFormat(destination, out int charsWritten, format, null));
			Assert.AreEqual(expected, new string(destination, 0, charsWritten));
		});
	}

	[TestMethod]
	public void ToString_NoArguments_UsesCurrentCulture()
	{
		SignificantNumber value = Parse("1234.5");

		WithCurrentCulture(CommaDecimalCulture, () =>
		{
			Assert.AreEqual("1234,5", value.ToString());
			Assert.AreEqual("1234,5", value.ToString((IFormatProvider?)null));
			Assert.AreEqual("1234,50", value.ToString("F2"));
			Assert.AreEqual("1234,5 / 1234,50", $"{value} / {value:0.00}");
		});
	}

	[TestMethod]
	public void Parse_NullProvider_UsesCurrentCulture()
	{
		WithCurrentCulture(CommaDecimalCulture, () =>
		{
			Assert.AreEqual(Parse("1.5"), SignificantNumber.Parse("1,5", null));
			Assert.AreEqual(Parse("1.5"), SignificantNumber.Parse("1,5".AsSpan(), null));
			Assert.AreEqual(Parse("1.5"), SignificantNumber.Parse("1,5", NumberStyles.Float, null));
			Assert.AreEqual(Parse("1.5"), SignificantNumber.Parse("1,5".AsSpan(), NumberStyles.Float, null));

			Assert.IsTrue(SignificantNumber.TryParse("1,5", null, out SignificantNumber parsed));
			Assert.AreEqual(Parse("1.5"), parsed);
			Assert.IsTrue(SignificantNumber.TryParse("1,5".AsSpan(), null, out parsed));
			Assert.AreEqual(Parse("1.5"), parsed);
			Assert.IsTrue(SignificantNumber.TryParse("1,5", NumberStyles.Float, null, out parsed));
			Assert.AreEqual(Parse("1.5"), parsed);
			Assert.IsTrue(SignificantNumber.TryParse("1,5".AsSpan(), NumberStyles.Float, null, out parsed));
			Assert.AreEqual(Parse("1.5"), parsed);
		});
	}

	[TestMethod]
	[DataRow("F1")]
	[DataRow("F2")]
	[DataRow("0.00")]
	[DataRow("0.0#")]
	public void Parse_NullProvider_ReadsBackWhatToStringWrote(string format)
	{
		SignificantNumber value = Parse("1234.5");

		WithCurrentCulture(CommaDecimalCulture, () =>
			Assert.AreEqual(value, SignificantNumber.Parse(value.ToString(format), null)));
	}

	private static void WithCurrentCulture(CultureInfo culture, Action action)
	{
		CultureInfo original = CultureInfo.CurrentCulture;
		try
		{
			CultureInfo.CurrentCulture = culture;
			action();
		}
		finally
		{
			CultureInfo.CurrentCulture = original;
		}
	}
}
