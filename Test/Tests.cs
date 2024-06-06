namespace ktsu.io.SignificantNumber.Test;

using ktsu.io.SignificantNumber;

[TestClass]
public class Tests
{
	[TestMethod]
	public void TestZero()
	{
		var a = 0.ToSignificantNumber();
		Assert.AreEqual(SignificantNumber.Zero, a);

		a = 0.0.ToSignificantNumber();
		Assert.AreEqual(SignificantNumber.Zero, a);

		a = 0.0f.ToSignificantNumber();
		Assert.AreEqual(SignificantNumber.Zero, a);

		a = 0.0m.ToSignificantNumber();
		Assert.AreEqual(SignificantNumber.Zero, a);
	}

	[TestMethod]
	public void TestOne()
	{
		var a = 1.ToSignificantNumber();
		Assert.AreEqual(SignificantNumber.One, a);

		a = 1.0.ToSignificantNumber();
		Assert.AreEqual(SignificantNumber.One, a);

		a = 1.0f.ToSignificantNumber();
		Assert.AreEqual(SignificantNumber.One, a);

		a = 1.0m.ToSignificantNumber();
		Assert.AreEqual(SignificantNumber.One, a);
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

		a = 1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		c = b + a;
		Assert.AreEqual(21, c.Significand);
		Assert.AreEqual(-1, c.Exponent);
		Assert.AreEqual(2, c.SignificantDigits);

		a = -1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		c = b + a;
		Assert.AreEqual(-1, c.Significand);
		Assert.AreEqual(-1, c.Exponent);
		Assert.AreEqual(2, c.SignificantDigits);
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
		Assert.AreEqual(2, c.SignificantDigits);

		a = 1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		c = b - a;
		Assert.AreEqual(-1, c.Significand);
		Assert.AreEqual(-1, c.Exponent);
		Assert.AreEqual(2, c.SignificantDigits);

		a = -1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		c = a - b;
		Assert.AreEqual(-21, c.Significand);
		Assert.AreEqual(-1, c.Exponent);
		Assert.AreEqual(2, c.SignificantDigits);

		a = -1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		c = b - a;
		Assert.AreEqual(21, c.Significand);
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
		Assert.AreEqual(111, c.Significand);
		Assert.AreEqual(-2, c.Exponent);
		Assert.AreEqual(3, c.SignificantDigits);

		a = 1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		c = b * a;
		Assert.AreEqual(111, c.Significand);
		Assert.AreEqual(-2, c.Exponent);
		Assert.AreEqual(3, c.SignificantDigits);

		a = -1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		c = b * a;
		Assert.AreEqual(-11, c.Significand);
		Assert.AreEqual(-1, c.Exponent);
		Assert.AreEqual(2, c.SignificantDigits);
	}
}
