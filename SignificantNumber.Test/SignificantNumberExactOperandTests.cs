// Copyright (c) 2023-2026 ktsu-dev contributors

namespace SignificantNumber.Test;

using System.Globalization;
using System.Numerics;
using ktsu.SignificantNumber;

/// <summary>
/// Covers arithmetic and comparison where both operands are -1, 0, or 1, which have unlimited precision.
/// </summary>
[TestClass]
public class SignificantNumberExactOperandTests
{
	private static SignificantNumber Parse(string text) =>
		SignificantNumber.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);

	private static T Square<T>(T value)
		where T : INumber<T> =>
		value * value;

	[TestMethod]
	[DataRow("1", "0")]
	[DataRow("-1", "0")]
	[DataRow("0", "0")]
	[DataRow("0", "1")]
	[DataRow("0", "-1")]
	public void Multiply_ByZero_ReturnsZero(string left, string right) =>
		Assert.AreEqual(SignificantNumber.Zero, Parse(left) * Parse(right));

	[TestMethod]
	public void Multiply_DefaultByDefault_ReturnsZero() =>
		Assert.AreEqual(SignificantNumber.Zero, default(SignificantNumber) * default(SignificantNumber));

	[TestMethod]
	public void Square_OfZeroThroughGenericMath_ReturnsZero() =>
		Assert.AreEqual(SignificantNumber.Zero, Square(SignificantNumber.Zero));

	[TestMethod]
	public void Multiply_ExactOperands_IsCommutativeAndExact()
	{
		string[] values = ["-1", "0", "1"];
		foreach (string left in values)
		{
			foreach (string right in values)
			{
				SignificantNumber expected = Parse((int.Parse(left, CultureInfo.InvariantCulture) * int.Parse(right, CultureInfo.InvariantCulture)).ToString(CultureInfo.InvariantCulture));
				Assert.AreEqual(expected, Parse(left) * Parse(right), $"{left} * {right}");
				Assert.AreEqual(Parse(right) * Parse(left), Parse(left) * Parse(right), $"{left} * {right} vs {right} * {left}");
			}
		}
	}

	[TestMethod]
	[DataRow("0", "1", "0")]
	[DataRow("0", "-1", "0")]
	[DataRow("1", "-1", "-1")]
	[DataRow("-1", "-1", "1")]
	public void Divide_ExactOperands_ReturnsExactQuotient(string left, string right, string expected) =>
		Assert.AreEqual(Parse(expected), Parse(left) / Parse(right));

	[TestMethod]
	[DataRow("0", "1")]
	[DataRow("1", "1")]
	[DataRow("-1", "1")]
	[DataRow("0", "-1")]
	public void Mod_ExactOperands_ReturnsZero(string left, string right) =>
		Assert.AreEqual(SignificantNumber.Zero, Parse(left) % Parse(right));

	[TestMethod]
	public void CompareTo_ExactOperands_IsAntisymmetric()
	{
		string[] values = ["-1", "0", "1"];
		foreach (string left in values)
		{
			foreach (string right in values)
			{
				int expected = int.Parse(left, CultureInfo.InvariantCulture).CompareTo(int.Parse(right, CultureInfo.InvariantCulture));
				Assert.AreEqual(expected, int.Sign(Parse(left).CompareTo(Parse(right))), $"{left}.CompareTo({right})");
			}
		}
	}
}
