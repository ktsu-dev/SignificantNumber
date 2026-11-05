// Copyright (c) 2023-2026 ktsu-dev contributors

namespace SignificantNumber.Test;

using System.Globalization;
using ktsu.PreciseNumber;
using ktsu.SignificantNumber;

/// <summary>
/// Covers <see cref="SignificantNumber.To{TOutput}"/>, which must not send a conversion to its own type through
/// <see cref="PreciseNumber"/>'s fallback, where the value is rebuilt by multiplying with a power of ten.
/// </summary>
[TestClass]
public class SignificantNumberToTests
{
	private static SignificantNumber Parse(string text) =>
		SignificantNumber.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);

	[TestMethod]
	[DataRow("1.23")]
	[DataRow("1200")]
	[DataRow("1E400")]
	[DataRow("1E-400")]
	[DataRow("-4.567")]
	[DataRow("123")]
	public void ToSignificantNumberKeepsValueAndSignificantDigits(string text)
	{
		SignificantNumber value = Parse(text);

		SignificantNumber converted = value.To<SignificantNumber>();

		Assert.AreEqual(value, converted);
		Assert.AreEqual(value.SignificantDigits, converted.SignificantDigits);
	}

	[TestMethod]
	[DataRow("1.23")]
	[DataRow("1200")]
	[DataRow("1E400")]
	[DataRow("1E-400")]
	public void ToPreciseNumberReturnsTheHeldValue(string text)
	{
		SignificantNumber value = Parse(text);

		Assert.AreEqual(value.Value, value.To<PreciseNumber>());
	}

	[TestMethod]
	public void ToBuiltInTypesBehavesAsBefore()
	{
		SignificantNumber value = Parse("-4.5");

		Assert.AreEqual(-4.5, value.To<double>());
		Assert.AreEqual(-4.5m, value.To<decimal>());
		Assert.AreEqual(1200, Parse("1200").To<int>());
		Assert.ThrowsExactly<OverflowException>(() => Parse("1E400").To<int>());
	}
}
