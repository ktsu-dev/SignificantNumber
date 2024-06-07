namespace ktsu.io.SignificantNumber.Test;

using System.Numerics;
using ktsu.io.SignificantNumber;

[TestClass]
public class Tests
{
	[TestMethod]
	public void TestZero()
	{
		const int testValue = 0;
		IsValid(SignificantNumber.Zero);
		IsValid(byte.CreateChecked(testValue).ToSignificantNumber());
		IsValid(sbyte.CreateChecked(testValue).ToSignificantNumber());
		IsValid(short.CreateChecked(testValue).ToSignificantNumber());
		IsValid(ushort.CreateChecked(testValue).ToSignificantNumber());
		IsValid(int.CreateChecked(testValue).ToSignificantNumber());
		IsValid(uint.CreateChecked(testValue).ToSignificantNumber());
		IsValid(long.CreateChecked(testValue).ToSignificantNumber());
		IsValid(ulong.CreateChecked(testValue).ToSignificantNumber());
		IsValid(float.CreateChecked(testValue).ToSignificantNumber());
		IsValid(double.CreateChecked(testValue).ToSignificantNumber());
		IsValid(decimal.CreateChecked(testValue).ToSignificantNumber());
		IsValid(BigInteger.CreateChecked(testValue).ToSignificantNumber());
		IsValid(Half.CreateTruncating(testValue).ToSignificantNumber());

		static void IsValid(SignificantNumber a)
		{
			Assert.AreEqual(SignificantNumber.Zero, a);
			Assert.AreEqual(0, a.Significand);
			Assert.AreEqual(-SignificantNumber.MaxDecimalPlaces, a.Exponent);
			Assert.AreEqual(SignificantNumber.MaxDecimalPlaces + 1, a.SignificantDigits);
		}
	}

	[TestMethod]
	public void TestOne()
	{
		const int testValue = 1;
		IsValid(SignificantNumber.One);
		IsValid(byte.CreateChecked(testValue).ToSignificantNumber());
		IsValid(sbyte.CreateChecked(testValue).ToSignificantNumber());
		IsValid(short.CreateChecked(testValue).ToSignificantNumber());
		IsValid(ushort.CreateChecked(testValue).ToSignificantNumber());
		IsValid(int.CreateChecked(testValue).ToSignificantNumber());
		IsValid(uint.CreateChecked(testValue).ToSignificantNumber());
		IsValid(long.CreateChecked(testValue).ToSignificantNumber());
		IsValid(ulong.CreateChecked(testValue).ToSignificantNumber());
		IsValid(float.CreateChecked(testValue).ToSignificantNumber());
		IsValid(double.CreateChecked(testValue).ToSignificantNumber());
		IsValid(decimal.CreateChecked(testValue).ToSignificantNumber());
		IsValid(BigInteger.CreateChecked(testValue).ToSignificantNumber());
		IsValid(Half.CreateTruncating(testValue).ToSignificantNumber());

		static void IsValid(SignificantNumber a)
		{
			Assert.AreEqual(SignificantNumber.One, a);
			Assert.AreEqual(BigInteger.Pow(10, SignificantNumber.MaxDecimalPlaces), a.Significand);
			Assert.AreEqual(-SignificantNumber.MaxDecimalPlaces, a.Exponent);
			Assert.AreEqual(SignificantNumber.MaxDecimalPlaces + 1, a.SignificantDigits);
		}
	}

	[TestMethod]
	public void TestCreation()
	{
		var a = 1.1.ToSignificantNumber();
		Assert.AreEqual(11, a.Significand);
		Assert.AreEqual(-1, a.Exponent);
		Assert.AreEqual(2, a.SignificantDigits);

		a = 1.01000.ToSignificantNumber();
		Assert.AreEqual(101, a.Significand);
		Assert.AreEqual(-2, a.Exponent);
		Assert.AreEqual(3, a.SignificantDigits);

		a = 10.01000.ToSignificantNumber();
		Assert.AreEqual(1001, a.Significand);
		Assert.AreEqual(-2, a.Exponent);
		Assert.AreEqual(4, a.SignificantDigits);

		a = 1000.000.ToSignificantNumber();
		Assert.AreEqual(1, a.Significand);
		Assert.AreEqual(3, a.Exponent);
		Assert.AreEqual(1, a.SignificantDigits);
	}

	[TestMethod]
	public void TestAbs()
	{
		var a = 1.1.ToSignificantNumber();
		var b = a.Abs();
		Assert.AreEqual(a, b);

		a = -1.1.ToSignificantNumber();
		b = a.Abs();
		Assert.AreEqual(11, b.Significand);
		Assert.AreEqual(-1, b.Exponent);
		Assert.AreEqual(2, b.SignificantDigits);
	}

	[TestMethod]
	public void TestRound()
	{
		var a = 1.111111111111111.ToSignificantNumber();
		var b = a.Round(0);
		Assert.AreEqual(SignificantNumber.One, b);

		b = a.Round(1);
		Assert.AreEqual(11, b.Significand);
		Assert.AreEqual(-1, b.Exponent);
		Assert.AreEqual(2, b.SignificantDigits);

		b = a.Round(2);
		Assert.AreEqual(111, b.Significand);
		Assert.AreEqual(-2, b.Exponent);
		Assert.AreEqual(3, b.SignificantDigits);

		a = 1.14.ToSignificantNumber();
		b = a.Round(1);
		Assert.AreEqual(11, b.Significand);
		Assert.AreEqual(-1, b.Exponent);
		Assert.AreEqual(2, b.SignificantDigits);

		a = 1.15.ToSignificantNumber();
		b = a.Round(1);
		Assert.AreEqual(12, b.Significand);
		Assert.AreEqual(-1, b.Exponent);
		Assert.AreEqual(2, b.SignificantDigits);
	}

	[TestMethod]
	public void TestAddition()
	{
		var a = 1.1.ToSignificantNumber();
		var b = 1.1.ToSignificantNumber();
		var c = a + b;
		Assert.AreEqual(22, c.Significand);
		Assert.AreEqual(-1, c.Exponent);
		Assert.AreEqual(2, c.SignificantDigits);

		a = 1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		c = a + b;
		Assert.AreEqual(21, c.Significand);
		Assert.AreEqual(-1, c.Exponent);
		Assert.AreEqual(2, c.SignificantDigits);

		a = -1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		c = a + b;
		Assert.AreEqual(-1, c.Significand);
		Assert.AreEqual(-1, c.Exponent);
		Assert.AreEqual(1, c.SignificantDigits);
	}

	[TestMethod]
	public void TestSubtraction()
	{
		var a = 1.1.ToSignificantNumber();
		var b = 1.1.ToSignificantNumber();
		var c = a - b;
		Assert.AreEqual(SignificantNumber.Zero, c);

		a = 1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		c = a - b;
		Assert.AreEqual(1, c.Significand);
		Assert.AreEqual(-1, c.Exponent);
		Assert.AreEqual(1, c.SignificantDigits);

		a = -1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		c = a - b;
		Assert.AreEqual(-21, c.Significand);
		Assert.AreEqual(-1, c.Exponent);
		Assert.AreEqual(2, c.SignificantDigits);
	}

	[TestMethod]
	public void TestEquality()
	{
		var a = 1.1.ToSignificantNumber();
		var b = 1.1.ToSignificantNumber();
		Assert.AreEqual(a, b);

		a = 1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		Assert.AreNotEqual(a, b);

		a = 1.1.ToSignificantNumber();
		b = 1.11.ToSignificantNumber();
		Assert.AreEqual(a, b);

		a = 10.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		Assert.AreNotEqual(a, b);

		a = 100.1.ToSignificantNumber();
		b = 100.ToSignificantNumber();
		Assert.AreEqual(a, b);

		a = 100.1.ToSignificantNumber();
		b = 100.0.ToSignificantNumber();
		Assert.AreEqual(a, b);

		a = 1.ToSignificantNumber();
		Assert.IsFalse(a.Equals(1));
	}

	[TestMethod]
	public void TestUnequality()
	{
		var a = 1.1.ToSignificantNumber();
		var b = 1.1.ToSignificantNumber();
		Assert.IsFalse(a != b);

		a = 1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		Assert.IsTrue(a != b);

		a = 1.1.ToSignificantNumber();
		b = 1.11.ToSignificantNumber();
		Assert.IsFalse(a != b);

		a = 10.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		Assert.IsTrue(a != b);

		a = 100.1.ToSignificantNumber();
		b = 100.ToSignificantNumber();
		Assert.IsFalse(a != b);

		a = 100.1.ToSignificantNumber();
		b = 100.0.ToSignificantNumber();
		Assert.IsFalse(a != b);
	}

	[TestMethod]
	public void TestEquals()
	{
		var a = 1.1.ToSignificantNumber();
		var b = 1.1.ToSignificantNumber();
		Assert.IsTrue(a.Equals(b));

		a = 1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		Assert.IsFalse(a.Equals(b));

		a = 1.1.ToSignificantNumber();
		b = 1.11.ToSignificantNumber();
		Assert.IsTrue(a.Equals(b));

		a = 10.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		Assert.IsFalse(a.Equals(b));

		a = 100.1.ToSignificantNumber();
		b = 100.ToSignificantNumber();
		Assert.IsTrue(a.Equals(b));

		a = 100.1.ToSignificantNumber();
		b = 100.0.ToSignificantNumber();
		Assert.IsTrue(a.Equals(b));
	}

	[TestMethod]
	public void TestGetHashCode()
	{
		var a = 1.1.ToSignificantNumber();
		var b = 1.1.ToSignificantNumber();
		Assert.AreEqual(a.GetHashCode(), b.GetHashCode());

		a = 1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
	}

	[TestMethod]
	public void TestToString()
	{
		var a = 1.ToSignificantNumber();
		Assert.AreEqual("1", a.ToString());

		a = -1.ToSignificantNumber();
		Assert.AreEqual("-1", a.ToString());

		a = 1.1.ToSignificantNumber();
		Assert.AreEqual("1.1", a.ToString());

		a = 1.01.ToSignificantNumber();
		Assert.AreEqual("1.01", a.ToString());

		a = 1.001.ToSignificantNumber();
		Assert.AreEqual("1.001", a.ToString());

		a = 1.0001.ToSignificantNumber();
		Assert.AreEqual("1.0001", a.ToString());
		Assert.AreEqual("1.0001", a.ToString("G"));
		Assert.ThrowsException<FormatException>(() => a.ToString("F"));
		Assert.ThrowsException<FormatException>(() => a.ToString("N"));
		Assert.ThrowsException<FormatException>(() => a.ToString("C"));
		Assert.ThrowsException<FormatException>(() => a.ToString("P"));
		Assert.ThrowsException<FormatException>(() => a.ToString("E"));
		Assert.ThrowsException<FormatException>(() => a.ToString("X"));
		Assert.ThrowsException<FormatException>(() => a.ToString("D"));
		Assert.ThrowsException<FormatException>(() => a.ToString("R"));
		Assert.ThrowsException<FormatException>(() => a.ToString("X"));

		a = 2.ToSignificantNumber();
		Assert.AreEqual("2", a.ToString());

		a = 2000.ToSignificantNumber();
		Assert.AreEqual("2000", a.ToString());

		a = 0.001.ToSignificantNumber();
		Assert.AreEqual("0.001", a.ToString());

		a = -0.001.ToSignificantNumber();
		Assert.AreEqual("-0.001", a.ToString());
	}

	[TestMethod]
	public void TestUnary()
	{
		var a = 1.1.ToSignificantNumber();
		var b = -a;
		Assert.AreEqual(-11, b.Significand);
		Assert.AreEqual(-1, b.Exponent);
		Assert.AreEqual(2, b.SignificantDigits);

		a = -1.1.ToSignificantNumber();
		b = -a;
		Assert.AreEqual(11, b.Significand);
		Assert.AreEqual(-1, b.Exponent);
		Assert.AreEqual(2, b.SignificantDigits);

		a = 1.1.ToSignificantNumber();
		b = +a;
		Assert.AreEqual(a, b);

		a = -0.ToSignificantNumber();
		Assert.AreEqual(SignificantNumber.Zero, a);
	}

	[TestMethod]
	public void TestMultiplication()
	{
		var a = 1.1.ToSignificantNumber();
		var b = 1.1.ToSignificantNumber();
		var c = a * b;
		Assert.AreEqual(12, c.Significand);
		Assert.AreEqual(-1, c.Exponent);
		Assert.AreEqual(2, c.SignificantDigits);

		a = 1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		c = a * b;
		Assert.AreEqual(11, c.Significand);
		Assert.AreEqual(-1, c.Exponent);
		Assert.AreEqual(2, c.SignificantDigits);

		a = -1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		c = a * b;
		Assert.AreEqual(-11, c.Significand);
		Assert.AreEqual(-1, c.Exponent);
		Assert.AreEqual(2, c.SignificantDigits);

		a = 1234.5678.ToSignificantNumber();
		b = 987.ToSignificantNumber();
		c = a * b;
		Assert.AreEqual(122, c.Significand);
		Assert.AreEqual(4, c.Exponent);
		Assert.AreEqual(3, c.SignificantDigits);
	}

	[TestMethod]
	public void TestDivision()
	{
		var a = 1.1.ToSignificantNumber();
		var b = 1.1.ToSignificantNumber();
		var c = a / b;
		Assert.AreEqual(SignificantNumber.One, c);

		a = 1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		c = a / b;
		Assert.AreEqual(11, c.Significand);
		Assert.AreEqual(-1, c.Exponent);
		Assert.AreEqual(2, c.SignificantDigits);

		a = -1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		c = a / b;
		Assert.AreEqual(-11, c.Significand);
		Assert.AreEqual(-1, c.Exponent);
		Assert.AreEqual(2, c.SignificantDigits);
	}
}
