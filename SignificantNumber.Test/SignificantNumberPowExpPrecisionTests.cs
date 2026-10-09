// Copyright (c) 2023-2026 ktsu-dev contributors

namespace SignificantNumber.Test;

using System.Globalization;
using ktsu.PreciseNumber;
using ktsu.SignificantNumber;

/// <summary>
/// Covers the significant digits <see cref="SignificantNumber.Pow"/> and <see cref="SignificantNumber.Exp"/> report
/// when the result is computed as e^y in a <see cref="double"/> and |y| is large enough to cost digits.
/// </summary>
[TestClass]
public class SignificantNumberPowExpPrecisionTests
{
	private static SignificantNumber Parse(string text) =>
		SignificantNumber.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);

	private static PreciseNumber Precise(string text) =>
		PreciseNumber.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);

	/// <summary>
	/// Asserts that every digit <paramref name="result"/> reports as significant matches <paramref name="trueValue"/>.
	/// </summary>
	private static void AssertReportedDigitsAreCorrect(string trueValue, SignificantNumber result) =>
		Assert.AreEqual(
			Precise(trueValue).ToSignificantNumber(result.SignificantDigits),
			result,
			$"{result} reports {result.SignificantDigits} significant digits of {trueValue}");

	// Reference values from Python decimal at 50 digits.
	[TestMethod]
	[DataRow("700.12345678901234567890", "1.1475032771153835242760515951099859734230596995731E+304")]
	[DataRow("-700.12345678901234567890", "8.7145720621715330952462542732455202987551213258088E-305")]
	public void Exp_LargeNonIntegerPower_ReportsOnlyCorrectDigits(string power, string trueValue) =>
		AssertReportedDigitsAreCorrect(trueValue, SignificantNumber.Exp(Precise(power)));

	[TestMethod]
	[DataRow("3.0000000000000000001", "200.50000000000000000001", "4.6005692393404968725867994056284596535772404109326E+95")]
	[DataRow("3.0000000000000000001", "2.5000000000000000001", "15.588457268119895644758622250842932011355302787297")]
	[DataRow("1.2345678901234567890", "2000", "1.0714068665496473862896950588742712767980475529713E+183")]
	public void Pow_LargeLogarithmOfResult_ReportsOnlyCorrectDigits(string baseNumber, string power, string trueValue) =>
		AssertReportedDigitsAreCorrect(trueValue, Parse(baseNumber).Pow(Precise(power)));

	[TestMethod]
	public void Exp_PowerNearSevenHundred_ReportsTwelveSignificantDigits() =>
		Assert.AreEqual(12, SignificantNumber.Exp(Precise("700.12345678901234567890")).SignificantDigits);

	[TestMethod]
	public void Pow_SmallLogarithmOfResult_KeepsTheDoublesPrecision() =>
		Assert.AreEqual(15, Parse("1.2345678901234567890").Pow(Precise("0.5000000000000000001")).SignificantDigits);
}
