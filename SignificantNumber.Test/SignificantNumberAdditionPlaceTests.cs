// Copyright (c) 2023-2026 ktsu-dev contributors

namespace SignificantNumber.Test;

using System.Globalization;
using ktsu.SignificantNumber;

/// <summary>
/// Covers the rounding of <see cref="SignificantNumber.Add"/> and <see cref="SignificantNumber.Subtract"/> to the
/// coarsest least significant place of their operands, including places left of the decimal point.
/// </summary>
[TestClass]
public class SignificantNumberAdditionPlaceTests
{
	private static SignificantNumber Parse(string text) =>
		SignificantNumber.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);

	[TestMethod]
	[DataRow("1.2E3", "34", "1.2E3")]
	[DataRow("1200", "34", "1.2E3")]
	[DataRow("34", "1.2E3", "1.2E3")]
	[DataRow("1.2E3", "56", "1.3E3")]
	[DataRow("1234", "5.6", "1240")]
	[DataRow("1.2E3", "1", "1.2E3")]
	public void Add_RoundsToTheCoarsestLeastSignificantPlace(string left, string right, string expected) =>
		Assert.AreEqual(Parse(expected), Parse(left) + Parse(right));

	[TestMethod]
	[DataRow("1.2E3", "34", "1.2E3")]
	[DataRow("1200", "34", "1.2E3")]
	[DataRow("1.2E3", "56", "1.1E3")]
	[DataRow("1234", "5.6", "1228")]
	public void Subtract_RoundsToTheCoarsestLeastSignificantPlace(string left, string right, string expected) =>
		Assert.AreEqual(Parse(expected), Parse(left) - Parse(right));
}
