namespace ktsu.SignificantNumber.Test;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

[TestClass]
public class Tests
{

	[TestMethod]
	public void TestZero()
	{
		static void IsValid(SignificantNumber a) => Assert.AreEqual(SignificantNumber.Zero, a);

		double testValue = 0;
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
		IsValid(Half.CreateChecked(testValue).ToSignificantNumber());

		double testValue2 = 0.1;
		Assert.AreNotEqual(SignificantNumber.Zero, testValue2.ToSignificantNumber());
	}

	[TestMethod]
	public void TestOne()
	{
		static void IsValid(SignificantNumber a) => Assert.AreEqual(SignificantNumber.One, a);

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
		IsValid(Half.CreateChecked(testValue).ToSignificantNumber());

		double testValue2 = 1.1;
		Assert.AreNotEqual(SignificantNumber.One, testValue2.ToSignificantNumber());
	}

	[TestMethod]
	public void TestNegativeOne()
	{
		static void IsValid(SignificantNumber a) => Assert.AreEqual(SignificantNumber.NegativeOne, a);

		const int testValue = -1;
		IsValid(SignificantNumber.NegativeOne);
		IsValid(sbyte.CreateChecked(testValue).ToSignificantNumber());
		IsValid(short.CreateChecked(testValue).ToSignificantNumber());
		IsValid(int.CreateChecked(testValue).ToSignificantNumber());
		IsValid(long.CreateChecked(testValue).ToSignificantNumber());
		IsValid(float.CreateChecked(testValue).ToSignificantNumber());
		IsValid(double.CreateChecked(testValue).ToSignificantNumber());
		IsValid(decimal.CreateChecked(testValue).ToSignificantNumber());
		IsValid(BigInteger.CreateChecked(testValue).ToSignificantNumber());
		IsValid(Half.CreateChecked(testValue).ToSignificantNumber());

		double testValue2 = -1.1;
		Assert.AreNotEqual(SignificantNumber.NegativeOne, testValue2.ToSignificantNumber());
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

		a = 1000.ToSignificantNumber();
		Assert.AreEqual(1, a.Significand);
		Assert.AreEqual(3, a.Exponent);
		Assert.AreEqual(1, a.SignificantDigits);

		var b = a.ToSignificantNumber();
		Assert.AreEqual(a, b);
	}

	[TestMethod]
	public void TestRoundTripWithRandomValues()
	{
		const int numIterations = 10000;

		TestType<sbyte>();
		TestType<byte>();
		TestType<short>();
		TestType<ushort>();
		TestType<int>();
		TestType<uint>();
		TestType<long>();
		TestType<ulong>();
		TestType<float>();
		TestType<double>();
		TestType<decimal>();
		TestType<BigInteger>();
		TestType<Half>();

		static void TestType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TInput>()
			where TInput : INumber<TInput>
		{
			var testValue = Helpers.GetMaxValue<TInput>();
			TestNumber(testValue, $"{nameof(Helpers.GetMaxValue)}");

			testValue = Helpers.GetMinValue<TInput>();
			TestNumber(testValue, $"{nameof(Helpers.GetMinValue)}");

			for (int i = 0; i < numIterations; i++)
			{
				testValue = Helpers.RandomNumber<TInput>();
				TestNumber(testValue, $"random[{i}]");
			}
		}

		static void TestNumber<TInput>(TInput testValue, string id)
			where TInput : INumber<TInput>
		{
			string typename = typeof(TInput).Name;
			string abs = $"{typename}.{nameof(INumber<TInput>.Abs)}";
			var sig = testValue.ToSignificantNumber();
			string str = sig.ToString();
			var roundtrip = TInput.Parse(str, CultureInfo.InvariantCulture);
			string format = SignificantNumber.GetStringFormatForFloatType<TInput>();
			string precisionSpecifier =
				format == "R"
				? ""
				: "e-" + format.Replace("E", "", StringComparison.OrdinalIgnoreCase);
			var epsilon = TInput.Max(TInput.Abs(testValue), TInput.Abs(roundtrip)) * TInput.Parse($"1{precisionSpecifier}", CultureInfo.InvariantCulture);
			Assert.IsTrue(TInput.Abs(testValue - roundtrip) <= epsilon, $"{id}: {abs}({testValue} - {roundtrip}) <= {epsilon}");
		}
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
	public void TestComparison()
	{
		var a = 1.1.ToSignificantNumber();
		var b = 1.1.ToSignificantNumber();
		Assert.IsTrue(a == b);
		Assert.IsFalse(a != b);
		Assert.IsFalse(a < b);
		Assert.IsTrue(a <= b);
		Assert.IsFalse(a > b);
		Assert.IsTrue(a >= b);
		Assert.IsTrue(a.Equals(b));

		a = 1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		Assert.IsFalse(a == b);
		Assert.IsTrue(a != b);
		Assert.IsFalse(a < b);
		Assert.IsFalse(a <= b);
		Assert.IsTrue(a > b);
		Assert.IsTrue(a >= b);
		Assert.IsFalse(a.Equals(b));

		a = 1.1.ToSignificantNumber();
		b = 1.11.ToSignificantNumber();
		Assert.IsTrue(a == b);
		Assert.IsFalse(a != b);
		Assert.IsFalse(a < b);
		Assert.IsTrue(a <= b);
		Assert.IsFalse(a > b);
		Assert.IsTrue(a >= b);
		Assert.IsTrue(a.Equals(b));

		a = 10.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		Assert.IsFalse(a == b);
		Assert.IsTrue(a != b);
		Assert.IsTrue(a > b);
		Assert.IsTrue(a >= b);
		Assert.IsFalse(a < b);
		Assert.IsFalse(a <= b);
		Assert.IsFalse(a.Equals(b));

		a = 100.1.ToSignificantNumber();
		b = 100.ToSignificantNumber();
		Assert.IsTrue(a == b);
		Assert.IsFalse(a != b);
		Assert.IsFalse(a < b);
		Assert.IsTrue(a <= b);
		Assert.IsFalse(a > b);
		Assert.IsTrue(a >= b);
		Assert.IsTrue(a.Equals(b));

		a = 100.1.ToSignificantNumber();
		b = 100.0.ToSignificantNumber();
		Assert.IsTrue(a == b);
		Assert.IsFalse(a != b);
		Assert.IsFalse(a < b);
		Assert.IsTrue(a <= b);
		Assert.IsFalse(a > b);
		Assert.IsTrue(a >= b);
		Assert.IsTrue(a.Equals(b));
	}

	[TestMethod]
	public void TestComparisonWithRandomValues()
	{
		var rng = new Random((int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());

		const double reallyLow = -10.0;
		const double reallyHigh = 10.0;

		for (int i = 0; i < 1000; ++i)
		{
			double a = (rng.NextDouble() * 2.0) - 1.0;
			Assert.IsTrue(a.ToSignificantNumber() > reallyLow.ToSignificantNumber(), $"{a} > {reallyLow} should be true but was false");
			Assert.IsTrue(a.ToSignificantNumber() < reallyHigh.ToSignificantNumber(), $"{a} < {reallyHigh} should be true but was false");
			Assert.IsFalse(a.ToSignificantNumber() < reallyLow.ToSignificantNumber(), $"{a} < {reallyLow} should be false but was true");
			Assert.IsFalse(a.ToSignificantNumber() > reallyHigh.ToSignificantNumber(), $"{a} > {reallyHigh} should be false but was true");
		}
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
		var a = 0.ToSignificantNumber();
		Assert.AreEqual("0", a.ToString());

		a = 1.ToSignificantNumber();
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
		Assert.AreEqual("2", a.ToString("g", CultureInfo.InvariantCulture));
		Assert.AreEqual("2", a.ToString("g", CultureInfo.CurrentCulture));

		a = 2000.ToSignificantNumber();
		Assert.AreEqual("2000", a.ToString());

		a = 0.001.ToSignificantNumber();
		Assert.AreEqual("0.001", a.ToString());

		a = -0.001.ToSignificantNumber();
		Assert.AreEqual("-0.001", a.ToString());

		a = 10.ToSignificantNumber();
		Assert.AreEqual("10", a.ToString());

		a = 100.ToSignificantNumber();
		Assert.AreEqual("100", a.ToString());
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

	[TestMethod]
	public void TestCompareTo()
	{
		var a = 1.1.ToSignificantNumber();
		var b = 1.1.ToSignificantNumber();
		Assert.AreEqual(0, a.CompareTo(b));

		a = 1.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		Assert.AreEqual(1, a.CompareTo(b));

		a = 1.1.ToSignificantNumber();
		b = 1.11.ToSignificantNumber();
		Assert.AreEqual(0, a.CompareTo(b));

		a = 10.1.ToSignificantNumber();
		b = 1.01.ToSignificantNumber();
		Assert.AreEqual(1, a.CompareTo(b));

		a = 100.1.ToSignificantNumber();
		b = 100.ToSignificantNumber();
		Assert.AreEqual(0, a.CompareTo(b));

		a = 100.1.ToSignificantNumber();
		b = 100.0.ToSignificantNumber();
		Assert.AreEqual(0, a.CompareTo(b));
	}

	[TestMethod]
	public void TestClamp()
	{
		var rng = new Random((int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());

		for (int i = 0; i < 1000; ++i)
		{
			double a = rng.NextDouble();
			double b = rng.NextDouble();
			double c = rng.NextDouble();
			if (b > c)
			{
				(c, b) = (b, c);
			}

			var d = Math.Clamp(a, b, c).ToSignificantNumber();
			var e = a.ToSignificantNumber().Clamp(b, c);
			Assert.AreEqual(d, e, $"iteration {i}: {a}, {b}, {c}, {d}, {e}");
		}
	}

	[TestMethod]
	public void TestRadix() => Assert.AreEqual(2, SignificantNumber.Radix);

	[TestMethod]
	public void TestIdentity()
	{
		Assert.AreEqual(SignificantNumber.Zero, SignificantNumber.AdditiveIdentity);
		Assert.AreEqual(SignificantNumber.One, SignificantNumber.MultiplicativeIdentity);
	}

	[TestMethod]
	public void TestTryCreateWithInvalidType()
	{
		Assert.IsFalse(SignificantNumberExtensions.TryCreate(3.ToSignificantNumber(), out var _));
	}

	[TestMethod]
	public void TestDecimalInteger()
	{
		var result = 3600m.ToSignificantNumber();
		Assert.AreEqual(36, result.Significand);
		Assert.AreEqual(2, result.Exponent);
		Assert.AreEqual(2, result.SignificantDigits);
	}

	[TestMethod]
	public void TestInteger()
	{
		var result = 3600.ToSignificantNumber();
		Assert.AreEqual(36, result.Significand);
		Assert.AreEqual(2, result.Exponent);
		Assert.AreEqual(2, result.SignificantDigits);
	}

	[TestMethod]
	public void TestLargeIntegerDivision()
	{
		var largeValue = 1e18.ToSignificantNumber();
		var result = largeValue / 1000.ToSignificantNumber();
		Assert.AreEqual(1e15.ToSignificantNumber(), result);
	}

	[TestMethod]
	public void TestSmallDecimalDivision()
	{
		var largeValue = 1e-18.ToSignificantNumber();
		var result = largeValue / 1000.ToSignificantNumber();
		Assert.AreEqual(1e-21.ToSignificantNumber(), result);
	}

	[TestMethod]
	public void TestLargeIntegerMultiplication()
	{
		var largeValue = 1e18.ToSignificantNumber();
		var result = largeValue * 1000.ToSignificantNumber();
		Assert.AreEqual(1e21.ToSignificantNumber(), result);
	}

	[TestMethod]
	public void TestSmallDecimalMultiplication()
	{
		var largeValue = 1e-18.ToSignificantNumber();
		var result = largeValue * 1000.ToSignificantNumber();
		Assert.AreEqual(1e-15.ToSignificantNumber(), result);
	}

	[TestMethod]
	public void TestRounding()
	{
		var a = 300.ToSignificantNumber();
		var b = 300.2.ToSignificantNumber();
		var c = 300.22.ToSignificantNumber();
		var d = 300.222.ToSignificantNumber();

		Assert.AreEqual(300.ToSignificantNumber(), a.Round(0));
		Assert.AreEqual(300.ToSignificantNumber(), a.Round(2));

		Assert.AreEqual(300.ToSignificantNumber(), b.Round(0));
		Assert.AreEqual(300.2.ToSignificantNumber(), b.Round(2));

		Assert.AreEqual(300.ToSignificantNumber(), c.Round(0));
		Assert.AreEqual(300.22.ToSignificantNumber(), c.Round(2));

		Assert.AreEqual(300.ToSignificantNumber(), d.Round(0));
		Assert.AreEqual(300.22.ToSignificantNumber(), d.Round(2));
	}
}
