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
}
