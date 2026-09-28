// Copyright (c) 2023-2026 ktsu-dev contributors

namespace SignificantNumber.Test;

using System.Globalization;
using ktsu.SignificantNumber;

/// <summary>
/// Covers the rounding of <see cref="SignificantNumber.Mod"/>, whose result must stay smaller than the divisor.
/// </summary>
[TestClass]
public class SignificantNumberModTests
{
	private static SignificantNumber Parse(string text) =>
		SignificantNumber.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);

	[TestMethod]
	[DataRow("19.9", "2", "0")]
	[DataRow("9.96", "5", "0")]
	[DataRow("-19.9", "2", "0")]
	[DataRow("6.8", "2.27", "0")]
	public void Mod_RemainderRoundingUpToDivisor_ReturnsZero(string left, string right, string expected) =>
		Assert.AreEqual(Parse(expected), Parse(left) % Parse(right));

	[TestMethod]
	[DataRow("19.9", "1.5", "0.4")]
	[DataRow("-19.9", "1.5", "-0.4")]
	[DataRow("7.25", "2", "1")]
	[DataRow("250", "70", "40")]
	[DataRow("10.123", "0.5", "0.1")]
	public void Mod_RoundsToFewestDecimalPlaces(string left, string right, string expected) =>
		Assert.AreEqual(Parse(expected), Parse(left) % Parse(right));

	[TestMethod]
	[DataRow("19.9", "2")]
	[DataRow("9.96", "5")]
	[DataRow("-19.9", "2")]
	[DataRow("19.9", "-2")]
	[DataRow("6.8", "2.27")]
	[DataRow("123.456", "0.7")]
	[DataRow("1.99", "0.2")]
	public void Mod_ResultMagnitude_IsLessThanDivisor(string left, string right)
	{
		SignificantNumber divisor = Parse(right);
		SignificantNumber result = Parse(left) % divisor;

		Assert.IsLessThan(SignificantNumber.Abs(divisor).Value, SignificantNumber.Abs(result).Value, $"{left} % {right} = {result}");
	}
}
