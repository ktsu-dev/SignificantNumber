// Copyright (c) 2023-2026 ktsu-dev contributors

namespace SignificantNumber.Test;

using System.Globalization;
using ktsu.PreciseNumber;
using ktsu.SignificantNumber;

/// <summary>
/// Covers <see cref="SignificantNumber.Pow"/> and <see cref="SignificantNumber.Exp"/> for results outside the range
/// of a <see cref="double"/>.
/// </summary>
[TestClass]
public class SignificantNumberRangeTests
{
	private static SignificantNumber Parse(string text) =>
		SignificantNumber.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);

	private static PreciseNumber Precise(string text) =>
		PreciseNumber.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);

	[TestMethod]
	[DataRow("10", "2000", "1E2000")]
	[DataRow("1.5", "2000", "1.5E352")]
	[DataRow("1E400", "0.5", "1E200")]
	[DataRow("-10", "2001", "-1E2001")]
	public void Pow_ResultAboveDoubleRange_ReturnsMagnitude(string baseNumber, string power, string expected) =>
		Assert.AreEqual(Parse(expected), Parse(baseNumber).Pow(Precise(power)));

	[TestMethod]
	[DataRow("2", "-1100", "7E-332")]
	[DataRow("1E-400", "0.5", "1E-200")]
	[DataRow("1E400", "-0.5", "1E-200")]
	[DataRow("-2", "-1101", "-4E-332")]
	public void Pow_ResultBelowDoubleRange_ReturnsNonzeroMagnitude(string baseNumber, string power, string expected) =>
		Assert.AreEqual(Parse(expected), Parse(baseNumber).Pow(Precise(power)));

	[TestMethod]
	public void Exp_ResultAboveDoubleRange_ReturnsMagnitude()
	{
		// e^710 = 2.2339...E308 and e^1000 = 1.9700711140170469...E434
		SignificantNumber exp710 = SignificantNumber.Exp(Precise("710"));
		SignificantNumber exp1000 = SignificantNumber.Exp(Precise("1000"));

		Assert.AreEqual(308, exp710.Exponent + exp710.SignificantDigits - 1);
		Assert.AreEqual(434, exp1000.Exponent + exp1000.SignificantDigits - 1);
		Assert.AreEqual(Parse("1.97007111402E434"), exp1000);
	}

	[TestMethod]
	public void Exp_ResultBelowDoubleRange_ReturnsNonzeroMagnitude()
	{
		// e^-1000 = 5.0759588975494567...E-435
		SignificantNumber result = SignificantNumber.Exp(Precise("-1000"));

		Assert.AreEqual(Parse("5.07595889755E-435"), result);
	}

	[TestMethod]
	public void Pow_PowerOfTenBeyondExponentRange_ThrowsOverflowException()
	{
		Assert.ThrowsExactly<OverflowException>(() => Parse("10").Pow(Precise("1E300")));
		Assert.ThrowsExactly<OverflowException>(() => Parse("10").Pow(Precise("-1E300")));
	}

	[TestMethod]
	public void Exp_PowerBeyondExponentRange_ThrowsOverflowException()
	{
		Assert.ThrowsExactly<OverflowException>(() => SignificantNumber.Exp(Precise("1E300")));
		Assert.ThrowsExactly<OverflowException>(() => SignificantNumber.Exp(Precise("-1E300")));
	}
}
