namespace ktsu.io.SignificantNumber.Test;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using ktsu.io.SignificantNumber;

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
	[SuppressMessage("Style", "IDE0008:Use explicit type", Justification = "<Pending>")]
	public void TestRoundTripWithRandomValues()
	{
		const int numIterations = 10000;
		const int maxFailuresToReport = 10;

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

		static void TestType<TInput>()
			where TInput : INumber<TInput>
		{
			var failureReason = string.Empty;
			string failures = string.Empty;
			int numFailures = 0;
			var typename = typeof(TInput).Name;

			try
			{
				failureReason = $"{nameof(Helpers.GetMaxValue)}<{typename}>()";
				var testValue = Helpers.GetMaxValue<TInput>();
				TestNumber(testValue, ref failureReason, $"{nameof(Helpers.GetMaxValue)}");
			}
			catch (Exception ex)
			{
				AddFailureMessage(ref numFailures, ref failures, failureReason, ex);
			}

			try
			{
				failureReason = $"{nameof(Helpers.GetMinValue)}<{typename}>()";
				var testValue = Helpers.GetMinValue<TInput>();
				TestNumber(testValue, ref failureReason, $"{nameof(Helpers.GetMinValue)}");
			}
			catch (Exception ex)
			{
				AddFailureMessage(ref numFailures, ref failures, failureReason, ex);
			}

			for (int i = 0; i < numIterations; i++)
			{
				try
				{
					failureReason = $"{nameof(Helpers.RandomNumber)}<{typename}>()";
					var testValue = Helpers.RandomNumber<TInput>();
					TestNumber(testValue, ref failureReason, $"random[{i}]");
				}
				catch (Exception ex)
				{
					AddFailureMessage(ref numFailures, ref failures, failureReason, ex);
				}
			}

			if (!string.IsNullOrEmpty(failures))
			{
				string msg = $"{Environment.NewLine}Round Trip failures:";
				msg += $"{Environment.NewLine}First {Math.Min(maxFailuresToReport, numFailures)} failures:{Environment.NewLine}{failures}";
				if (numFailures > maxFailuresToReport)
				{
					msg += $"{Environment.NewLine}Plus {numFailures - maxFailuresToReport} more failures.";
				}

				Assert.Fail(msg);
			}
		}

		static void TestNumber<TInput>(TInput testValue, ref string failureReason, string id)
			where TInput : INumber<TInput>
		{
			var typename = typeof(TInput).Name;
			var abs = $"{typename}.{nameof(INumber<TInput>.Abs)}";
			var max = $"{typename}.{nameof(INumber<TInput>.Max)}";
			var create = $"{typename}.{nameof(INumber<TInput>.CreateTruncating)}";
			failureReason = $"{id}: {testValue}.{nameof(SignificantNumberExtensions.ToSignificantNumber)}()";
			var sig = testValue.ToSignificantNumber();
			failureReason = $"{id}: {sig}.{nameof(sig.ToString)}()";
			var str = sig.ToString();
			failureReason = $"{id}: {typename}.{nameof(INumber<TInput>.Parse)}({str})";
			var roundtrip = TInput.Parse(str, CultureInfo.InvariantCulture);
			failureReason = $"{id}: {max}({abs}({testValue}), {abs}({roundtrip})) * {create}(1e-15)";
			var epsilon = TInput.Max(TInput.Abs(testValue), TInput.Abs(roundtrip)) * TInput.CreateTruncating(1e-15);
			failureReason = $"{id}: {abs}({testValue} - {roundtrip}) <= {epsilon}";
			Assert.IsTrue(TInput.Abs(testValue - roundtrip) <= epsilon);
		}

		static void AddFailureMessage(ref int numFailures, ref string failures, string failureReason, Exception ex)
		{
			if (numFailures < maxFailuresToReport)
			{
				failures += $"{ex.GetType().Name} | {ex.Message} | {failureReason}{Environment.NewLine}";
			}

			++numFailures;
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
		var rng = new Random((int)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds);

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
	[SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
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
		var rng = new Random((int)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds);

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
	public void TestMaxDecimalPlaces() => Assert.AreEqual(15, SignificantNumber.MaxDecimalPlaces);

	[TestMethod]
	public void TestIdentity()
	{
		Assert.AreEqual(SignificantNumber.Zero, SignificantNumber.AdditiveIdentity);
		Assert.AreEqual(SignificantNumber.One, SignificantNumber.MultiplicativeIdentity);
	}

	// AI generated tests

	[TestMethod]
	public void DoesImplementGenericInterfaceNonGenericInterfaceThrowsArgumentException()
	{
		// Arrange
		var type = typeof(List<int>);
		var nonGenericInterface = typeof(IDisposable);

		// Act & Assert
		Assert.ThrowsException<ArgumentException>(() => SignificantNumber.DoesImplementGenericInterface(type, nonGenericInterface));
	}

	[TestMethod]
	public void DoesImplementGenericInterfaceGenericInterfaceNotImplementedReturnsFalse()
	{
		// Arrange
		var type = typeof(List<int>);
		var genericInterface = typeof(IDictionary<,>);

		// Act
		bool result = SignificantNumber.DoesImplementGenericInterface(type, genericInterface);

		// Assert
		Assert.IsFalse(result);
	}

	[TestMethod]
	public void DoesImplementGenericInterfaceGenericInterfaceImplementedReturnsTrue()
	{
		// Arrange
		var type = typeof(List<int>);
		var genericInterface = typeof(IEnumerable<>);

		// Act
		bool result = SignificantNumber.DoesImplementGenericInterface(type, genericInterface);

		// Assert
		Assert.IsTrue(result);
	}

	[TestMethod]
	public void ToSignificantNumberValidInputReturnsSignificantNumber()
	{
		// Arrange
		int input = 5;

		// Act
		var result = input.ToSignificantNumber();

		// Assert
		Assert.IsNotNull(result);
	}

	[TestMethod]
	public void ToSignificantNumberInvalidInputThrowsNotSupportedException()
	{
		// Arrange
		var input = new InvalidNumber();

		// Act & Assert
		Assert.ThrowsException<NotSupportedException>(() => input.ToSignificantNumber());
	}

	[TestMethod]
	public void TryCreateValidInputReturnsTrueAndSetsOutParameter()
	{
		// Arrange
		int input = 5;

		// Act
		bool result = SignificantNumberExtensions.TryCreate(input, out var significantNumber);

		// Assert
		Assert.IsTrue(result);
		Assert.IsNotNull(significantNumber);
	}

	[TestMethod]
	public void TryCreateInvalidInputReturnsFalseAndSetsOutParameterToDefault()
	{
		// Arrange
		var input = new InvalidNumber();

		// Act
		bool result = SignificantNumberExtensions.TryCreate(input, out var significantNumber);

		// Assert
		Assert.IsFalse(result);
		Assert.AreEqual(default, significantNumber);
	}

	[TestMethod]
	public void IsEvenIntegerEvenIntegerReturnsTrue()
	{
		// Arrange
		var evenInteger = 2.ToSignificantNumber();

		// Act
		bool result = SignificantNumber.IsEvenInteger(evenInteger);

		// Assert
		Assert.IsTrue(result);
	}

	[TestMethod]
	public void IsEvenIntegerOddIntegerReturnsFalse()
	{
		// Arrange
		var oddInteger = 3.ToSignificantNumber();

		// Act
		bool result = SignificantNumber.IsEvenInteger(oddInteger);

		// Assert
		Assert.IsFalse(result);
	}

	[TestMethod]
	public void IsOddIntegerOddIntegerReturnsTrue()
	{
		// Arrange
		var oddInteger = 3.ToSignificantNumber();

		// Act
		bool result = SignificantNumber.IsOddInteger(oddInteger);

		// Assert
		Assert.IsTrue(result);
	}

	[TestMethod]
	public void IsOddIntegerEvenIntegerReturnsFalse()
	{
		// Arrange
		var evenInteger = 2.ToSignificantNumber();

		// Act
		bool result = SignificantNumber.IsOddInteger(evenInteger);

		// Assert
		Assert.IsFalse(result);
	}

	[TestMethod]
	public void IsZeroZeroReturnsTrue()
	{
		// Arrange
		var zero = 0.ToSignificantNumber();

		// Act
		bool result = SignificantNumber.IsZero(zero);

		// Assert
		Assert.IsTrue(result);
	}

	[TestMethod]
	public void IsZeroNonZeroReturnsFalse()
	{
		// Arrange
		var nonZero = 1.ToSignificantNumber();

		// Act
		bool result = SignificantNumber.IsZero(nonZero);

		// Assert
		Assert.IsFalse(result);
	}

	[TestMethod]
	public void DoesImplementINumberTypeImplementsINumberReturnsTrue()
	{
		// Arrange
		var type = typeof(int); // Replace with actual type that implements INumber<T>

		// Act
		bool result = SignificantNumber.DoesImplementINumber(type);

		// Assert
		Assert.IsTrue(result);
	}

	[TestMethod]
	public void DoesImplementINumberTypeDoesNotImplementINumberReturnsFalse()
	{
		// Arrange
		var type = typeof(string); // Replace with actual type that does not implement INumber<T>

		// Act
		bool result = SignificantNumber.DoesImplementINumber(type);

		// Assert
		Assert.IsFalse(result);
	}

	[TestMethod]
	public void AddTwoSignificantNumbersReturnsCorrectResult()
	{
		// Arrange
		var left = 2.ToSignificantNumber();
		var right = 3.ToSignificantNumber();
		var expected = 5.ToSignificantNumber();
		// Act
		var result = SignificantNumber.Add(left, right);

		// Assert
		Assert.AreEqual(expected, result);
	}

	[TestMethod]
	public void SubtractTwoSignificantNumbersReturnsCorrectResult()
	{
		// Arrange
		var left = 5.ToSignificantNumber();
		var right = 3.ToSignificantNumber();
		var expected = 2.ToSignificantNumber();

		// Act
		var result = SignificantNumber.Subtract(left, right);

		// Assert
		Assert.AreEqual(expected, result);
	}
}
