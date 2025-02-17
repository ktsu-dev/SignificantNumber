// Ignore Spelling: Commonized

namespace ktsu.SignificantNumber.Test;

using System.Globalization;
using System.Numerics;

[TestClass]
public class AITests
{
	[TestMethod]
	public void TestZero()
	{
		var zero = SignificantNumber.Zero;
		Assert.AreEqual(0, zero.Significand);
		Assert.AreEqual(0, zero.Exponent);
	}

	[TestMethod]
	public void TestOne()
	{
		var one = SignificantNumber.One;
		Assert.AreEqual(1, one.Significand);
		Assert.AreEqual(0, one.Exponent);
	}

	[TestMethod]
	public void TestNegativeOne()
	{
		var negativeOne = SignificantNumber.NegativeOne;
		Assert.AreEqual(-1, negativeOne.Significand);
		Assert.AreEqual(0, negativeOne.Exponent);
	}

	[TestMethod]
	public void TestAdd()
	{
		var one = SignificantNumber.One;
		var result = one + one;
		Assert.AreEqual(2.ToSignificantNumber(), result);
	}

	[TestMethod]
	public void TestSubtract()
	{
		var one = SignificantNumber.One;
		var result = one - one;
		Assert.AreEqual(SignificantNumber.Zero, result);
	}

	[TestMethod]
	public void TestMultiply()
	{
		var one = SignificantNumber.One;
		var result = one * one;
		Assert.AreEqual(SignificantNumber.One, result);
	}

	[TestMethod]
	public void TestDivide()
	{
		var one = SignificantNumber.One;
		var result = one / one;
		Assert.AreEqual(SignificantNumber.One, result);
	}

	[TestMethod]
	public void TestRound()
	{
		var number = 1.2345.ToSignificantNumber();
		var rounded = number.Round(2);
		Assert.AreEqual(1.24.ToSignificantNumber(), rounded);
	}

	[TestMethod]
	public void TestAbs()
	{
		var negative = SignificantNumber.NegativeOne;
		var positive = negative.Abs();
		Assert.AreEqual(SignificantNumber.One, positive);
	}

	[TestMethod]
	public void TestClamp()
	{
		var value = 5.ToSignificantNumber();
		var min = 3.ToSignificantNumber();
		var max = 7.ToSignificantNumber();
		var clamped = value.Clamp(min, max);
		Assert.AreEqual(value, clamped);

		clamped = 2.ToSignificantNumber().Clamp(min, max);
		Assert.AreEqual(min, clamped);

		clamped = 8.ToSignificantNumber().Clamp(min, max);
		Assert.AreEqual(max, clamped);
	}

	[TestMethod]
	public void TestToString()
	{
		var number = 0.0123.ToSignificantNumber();
		string str = number.ToString();
		Assert.AreEqual("0.0123", str);
	}

	[TestMethod]
	public void TestEquals()
	{
		var one = SignificantNumber.One;
		Assert.IsTrue(one.Equals(SignificantNumber.One));
		Assert.IsFalse(one.Equals(SignificantNumber.Zero));
	}

	[TestMethod]
	public void TestCompareTo()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(one.CompareTo(zero) > 0);
		Assert.IsTrue(zero.CompareTo(one) < 0);
		Assert.AreEqual(0, one.CompareTo(SignificantNumber.One));
	}

	// Tests for comparison operators
	[TestMethod]
	public void TestGreaterThan()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(one > zero);
		Assert.IsFalse(zero > one);
	}

	[TestMethod]
	public void TestGreaterThanOrEqual()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(one >= zero);
		Assert.IsTrue(one >= SignificantNumber.One);
		Assert.IsFalse(zero >= one);
	}

	[TestMethod]
	public void TestLessThan()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(zero < one);
		Assert.IsFalse(one < zero);
	}

	[TestMethod]
	public void TestLessThanOrEqual()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(zero <= one);
		Assert.IsTrue(one <= SignificantNumber.One);
		Assert.IsFalse(one <= zero);
	}

	[TestMethod]
	public void TestEquality()
	{
		var one = SignificantNumber.One;
		var anotherOne = 1.ToSignificantNumber();
		var zero = SignificantNumber.Zero;
		Assert.AreEqual(anotherOne, one);
		Assert.AreNotEqual(zero, one);
	}

	[TestMethod]
	public void TestInequality()
	{
		var one = SignificantNumber.One;
		var anotherOne = 1.ToSignificantNumber();
		var zero = SignificantNumber.Zero;
		Assert.AreNotEqual(zero, one);
		Assert.AreEqual(anotherOne, one);
	}

	// Tests for unsupported operators
	[TestMethod]
	public void TestModulus()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => one % one);
	}

	[TestMethod]
	public void TestDecrement()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => --one);
	}

	[TestMethod]
	public void TestIncrement()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => ++one);
	}

	// Test for unary + operator
	[TestMethod]
	public void TestUnaryPlus()
	{
		var one = SignificantNumber.One;
		var result = +one;
		Assert.AreEqual(SignificantNumber.One, result);
	}

	// Tests for static methods of unary operators
	[TestMethod]
	public void TestStaticUnaryPlus()
	{
		var one = SignificantNumber.One;
		var result = SignificantNumber.Plus(one);
		Assert.AreEqual(SignificantNumber.One, result);
	}

	[TestMethod]
	public void TestStaticUnaryNegate()
	{
		var one = SignificantNumber.One;
		var result = SignificantNumber.Negate(one);
		Assert.AreEqual(SignificantNumber.NegativeOne, result);
	}

	// Tests for static methods of binary operators
	[TestMethod]
	public void TestStaticAdd()
	{
		var one = SignificantNumber.One;
		var result = SignificantNumber.Add(one, one);
		Assert.AreEqual(2.ToSignificantNumber(), result);
	}

	[TestMethod]
	public void TestStaticSubtract()
	{
		var one = SignificantNumber.One;
		var result = SignificantNumber.Subtract(one, one);
		Assert.AreEqual(SignificantNumber.Zero, result);
	}

	[TestMethod]
	public void TestStaticMultiply()
	{
		var one = SignificantNumber.One;
		var result = SignificantNumber.Multiply(one, one);
		Assert.AreEqual(SignificantNumber.One, result);
	}

	[TestMethod]
	public void TestStaticDivide()
	{
		var one = SignificantNumber.One;
		var result = SignificantNumber.Divide(one, one);
		Assert.AreEqual(SignificantNumber.One, result);
	}

	[TestMethod]
	public void TestStaticModulus()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.Mod(one, one));
	}

	// Test for static increment method
	[TestMethod]
	public void TestStaticIncrement()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.Increment(one));
	}

	// Test for static decrement method
	[TestMethod]
	public void TestStaticDecrement()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.Decrement(one));
	}

	[TestMethod]
	public void TestStaticGreaterThan()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(SignificantNumber.GreaterThan(one, zero));
		Assert.IsFalse(SignificantNumber.GreaterThan(zero, one));
	}

	[TestMethod]
	public void TestStaticGreaterThanOrEqual()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(SignificantNumber.GreaterThanOrEqual(one, zero));
		Assert.IsTrue(SignificantNumber.GreaterThanOrEqual(one, SignificantNumber.One));
		Assert.IsFalse(SignificantNumber.GreaterThanOrEqual(zero, one));
	}

	[TestMethod]
	public void TestStaticLessThan()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(SignificantNumber.LessThan(zero, one));
		Assert.IsFalse(SignificantNumber.LessThan(one, zero));
	}

	[TestMethod]
	public void TestStaticLessThanOrEqual()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(SignificantNumber.LessThanOrEqual(zero, one));
		Assert.IsTrue(SignificantNumber.LessThanOrEqual(one, SignificantNumber.One));
		Assert.IsFalse(SignificantNumber.LessThanOrEqual(one, zero));
	}

	[TestMethod]
	public void TestStaticEqual()
	{
		var one = SignificantNumber.One;
		var anotherOne = 1.ToSignificantNumber();
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(SignificantNumber.Equal(one, anotherOne));
		Assert.IsFalse(SignificantNumber.Equal(one, zero));
	}

	[TestMethod]
	public void TestStaticNotEqual()
	{
		var one = SignificantNumber.One;
		var anotherOne = 1.ToSignificantNumber();
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(SignificantNumber.NotEqual(one, zero));
		Assert.IsFalse(SignificantNumber.NotEqual(one, anotherOne));
	}

	// Test for static Max method
	[TestMethod]
	public void TestStaticMax()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		var result = SignificantNumber.Max(one, zero);
		Assert.AreEqual(one, result);
	}

	// Test for static Min method
	[TestMethod]
	public void TestStaticMin()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		var result = SignificantNumber.Min(one, zero);
		Assert.AreEqual(zero, result);
	}

	// Test for static Clamp method
	[TestMethod]
	public void TestStaticClamp()
	{
		var value = 5.ToSignificantNumber();
		var min = 3.ToSignificantNumber();
		var max = 7.ToSignificantNumber();

		var result = SignificantNumber.Clamp(value, min, max);
		Assert.AreEqual(value, result);

		result = SignificantNumber.Clamp(2.ToSignificantNumber(), min, max);
		Assert.AreEqual(min, result);

		result = SignificantNumber.Clamp(8.ToSignificantNumber(), min, max);
		Assert.AreEqual(max, result);
	}

	// Test for static Round method
	[TestMethod]
	public void TestStaticRound()
	{
		var number = 1.2345.ToSignificantNumber();
		var result = SignificantNumber.Round(number, 2);
		Assert.AreEqual(1.24.ToSignificantNumber(), result);
	}

	[TestMethod]
	public void TestTryConvertFromChecked()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryConvertFromChecked(one, out var result));
	}

	[TestMethod]
	public void TestTryConvertFromSaturating()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryConvertFromSaturating(one, out var result));
	}

	[TestMethod]
	public void TestTryConvertFromTruncating()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryConvertFromTruncating(one, out var result));
	}

	[TestMethod]
	public void TestTryConvertToChecked()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryConvertToChecked(one, out SignificantNumber result));
	}

	[TestMethod]
	public void TestTryConvertToSaturating()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryConvertToSaturating(one, out SignificantNumber result));
	}

	[TestMethod]
	public void TestTryConvertToTruncating()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryConvertToTruncating(one, out SignificantNumber result));
	}

	[TestMethod]
	public void TestIsCanonical()
	{
		var one = SignificantNumber.One;
		Assert.IsTrue(SignificantNumber.IsCanonical(one));
	}

	[TestMethod]
	public void TestIsComplexNumber()
	{
		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsComplexNumber(one));
	}

	[TestMethod]
	public void TestIsEvenInteger()
	{
		var two = 2.ToSignificantNumber();
		Assert.IsTrue(SignificantNumber.IsEvenInteger(two));

		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsEvenInteger(one));
	}

	[TestMethod]
	public void TestIsFinite()
	{
		var one = SignificantNumber.One;
		Assert.IsTrue(SignificantNumber.IsFinite(one));
	}

	[TestMethod]
	public void TestIsImaginaryNumber()
	{
		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsImaginaryNumber(one));
	}

	[TestMethod]
	public void TestIsInfinity()
	{
		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsInfinity(one));
	}

	[TestMethod]
	public void TestIsInteger()
	{
		var one = SignificantNumber.One;
		Assert.IsTrue(SignificantNumber.IsInteger(one));
	}

	[TestMethod]
	public void TestIsNaN()
	{
		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsNaN(one));
	}

	[TestMethod]
	public void TestIsNegative()
	{
		var negativeOne = SignificantNumber.NegativeOne;
		Assert.IsTrue(SignificantNumber.IsNegative(negativeOne));

		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsNegative(one));
	}

	[TestMethod]
	public void TestIsNegativeInfinity()
	{
		var negativeOne = SignificantNumber.NegativeOne;
		Assert.IsFalse(SignificantNumber.IsNegativeInfinity(negativeOne));
	}

	[TestMethod]
	public void TestIsNormal()
	{
		var one = SignificantNumber.One;
		Assert.IsTrue(SignificantNumber.IsNormal(one));
	}

	[TestMethod]
	public void TestIsOddInteger()
	{
		var one = SignificantNumber.One;
		Assert.IsTrue(SignificantNumber.IsOddInteger(one));

		var two = 2.ToSignificantNumber();
		Assert.IsFalse(SignificantNumber.IsOddInteger(two));
	}

	[TestMethod]
	public void TestIsPositive()
	{
		var one = SignificantNumber.One;
		Assert.IsTrue(SignificantNumber.IsPositive(one));

		var negativeOne = SignificantNumber.NegativeOne;
		Assert.IsFalse(SignificantNumber.IsPositive(negativeOne));
	}

	[TestMethod]
	public void TestIsPositiveInfinity()
	{
		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsPositiveInfinity(one));
	}

	[TestMethod]
	public void TestIsRealNumber()
	{
		var one = SignificantNumber.One;
		Assert.IsTrue(SignificantNumber.IsRealNumber(one));
	}

	[TestMethod]
	public void TestIsSubnormal()
	{
		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsSubnormal(one));
	}

	[TestMethod]
	public void TestIsZero()
	{
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(SignificantNumber.IsZero(zero));

		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsZero(one));
	}

	[TestMethod]
	public void TestTryParseReadOnlySpan()
	{
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryParse("1.23e2".AsSpan(), NumberStyles.Float, CultureInfo.InvariantCulture, out var result));
	}

	[TestMethod]
	public void TestTryParseStringStyleProvider()
	{
		string input = "1.23e2";
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out var result));
	}

	[TestMethod]
	public void TestTryParseStringProvider()
	{
		string input = "1.23e2";
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryParse(input, CultureInfo.InvariantCulture, out var result));
	}

	[TestMethod]
	public void TestTryParseReadOnlySpanProvider()
	{
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryParse("1.23e2".AsSpan(), CultureInfo.InvariantCulture, out var result));
	}

	[TestMethod]
	public void TestParseReadOnlySpanStyleProvider()
	{
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.Parse("1.23e2".AsSpan(), NumberStyles.Float, CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void TestParseStringStyleProvider()
	{
		string input = "1.23e2";
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.Parse(input, NumberStyles.Float, CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void TestParseStringProvider()
	{
		string input = "1.23e2";
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.Parse(input, CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void TestParseReadOnlySpanProvider()
	{
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.Parse("1.23e2".AsSpan(), CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void TestStaticMaxMagnitude()
	{
		var one = SignificantNumber.One;
		var negativeOne = SignificantNumber.NegativeOne;
		var result = SignificantNumber.MaxMagnitude(one, negativeOne);
		Assert.AreEqual(one, result);
	}

	[TestMethod]
	public void TestStaticMaxMagnitudeNumber()
	{
		var one = SignificantNumber.One;
		var negativeOne = SignificantNumber.NegativeOne;
		var result = SignificantNumber.MaxMagnitudeNumber(one, negativeOne);
		Assert.AreEqual(one, result);
	}

	[TestMethod]
	public void TestStaticMinMagnitude()
	{
		var one = SignificantNumber.One;
		var negativeOne = SignificantNumber.NegativeOne;
		var result = SignificantNumber.MinMagnitude(one, negativeOne);
		Assert.AreEqual(one, result);
	}

	[TestMethod]
	public void TestStaticMinMagnitudeNumber()
	{
		var one = SignificantNumber.One;
		var negativeOne = SignificantNumber.NegativeOne;
		var result = SignificantNumber.MinMagnitudeNumber(one, negativeOne);
		Assert.AreEqual(one, result);
	}

	[TestMethod]
	public void TestCompareToObject()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		object oneObject = SignificantNumber.One;
		object zeroObject = SignificantNumber.Zero;
		object intObject = 1;
		Assert.AreEqual(0, one.CompareTo(oneObject));
		Assert.IsTrue(one.CompareTo(zeroObject) > 0);
		Assert.IsTrue(zero.CompareTo(oneObject) < 0);
		Assert.ThrowsException<NotSupportedException>(() => one.CompareTo(intObject));
	}

	[TestMethod]
	public void TestCompareToSignificantNumber()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		var anotherOne = SignificantNumber.One;

		Assert.IsTrue(one.CompareTo(zero) > 0);
		Assert.IsTrue(zero.CompareTo(one) < 0);
		Assert.AreEqual(0, one.CompareTo(anotherOne));
	}

	[TestMethod]
	public void TestCompareToINumber()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		var anotherOne = SignificantNumber.One;

		Assert.IsTrue(one.CompareTo<SignificantNumber>(zero) > 0);
		Assert.IsTrue(zero.CompareTo<SignificantNumber>(one) < 0);
		Assert.AreEqual(0, one.CompareTo<SignificantNumber>(anotherOne));

		Assert.IsTrue(one.CompareTo(0) > 0);
		Assert.IsTrue(zero.CompareTo(1) < 0);
		Assert.AreEqual(0, one.CompareTo(1));

		Assert.IsTrue(one.CompareTo(0.0) > 0);
		Assert.IsTrue(zero.CompareTo(1.0) < 0);
		Assert.AreEqual(0, one.CompareTo(1.0));
	}

	[TestMethod]
	public void TestConstructorPositiveNumber()
	{
		var number = new SignificantNumber(2, 123);
		Assert.AreEqual(123, number.Significand);
		Assert.AreEqual(2, number.Exponent);
		Assert.AreEqual(3, number.SignificantDigits);
	}

	[TestMethod]
	public void TestConstructorNegativeNumber()
	{
		var number = new SignificantNumber(2, -123);
		Assert.AreEqual(-123, number.Significand);
		Assert.AreEqual(2, number.Exponent);
		Assert.AreEqual(3, number.SignificantDigits);
	}

	[TestMethod]
	public void TestConstructorZero()
	{
		var number = new SignificantNumber(2, 0);
		Assert.AreEqual(0, number.Significand);
		Assert.AreEqual(0, number.Exponent);
		Assert.AreEqual(0, number.SignificantDigits);
	}

	[TestMethod]
	public void TestConstructorSanitizeTrue()
	{
		var number = new SignificantNumber(2, 12300, true);
		Assert.AreEqual(123, number.Significand);
		Assert.AreEqual(4, number.Exponent);
		Assert.AreEqual(3, number.SignificantDigits);
	}

	[TestMethod]
	public void TestConstructorSanitizeFalse()
	{
		var number = new SignificantNumber(2, 12300, false);
		Assert.AreEqual(12300, number.Significand);
		Assert.AreEqual(2, number.Exponent);
		Assert.AreEqual(5, number.SignificantDigits);
	}

	[TestMethod]
	public void TestCreateFromFloatingPointPositiveNumber()
	{
		var number = SignificantNumber.CreateFromFloatingPoint(123000.45);
		Assert.AreEqual(12300045, number.Significand);
		Assert.AreEqual(-2, number.Exponent);
		Assert.AreEqual(8, number.SignificantDigits);
	}

	[TestMethod]
	public void TestCreateFromFloatingPointNegativeNumber()
	{
		var number = SignificantNumber.CreateFromFloatingPoint(-123000.45);
		Assert.AreEqual(-12300045, number.Significand);
		Assert.AreEqual(-2, number.Exponent);
		Assert.AreEqual(8, number.SignificantDigits);
	}

	[TestMethod]
	public void TestCreateFromFloatingPointOne()
	{
		var number = SignificantNumber.CreateFromFloatingPoint(1.0);
		Assert.AreEqual(1, number.Significand);
		Assert.AreEqual(0, number.Exponent);
		Assert.AreEqual(1, number.SignificantDigits);
	}

	[TestMethod]
	public void TestCreateFromFloatingPointNegativeOne()
	{
		var number = SignificantNumber.CreateFromFloatingPoint(-1.0);
		Assert.AreEqual(-1, number.Significand);
		Assert.AreEqual(0, number.Exponent);
		Assert.AreEqual(1, number.SignificantDigits);
	}

	[TestMethod]
	public void TestCreateFromFloatingPointZero()
	{
		var number = SignificantNumber.CreateFromFloatingPoint(0000.0);
		Assert.AreEqual(0, number.Significand);
		Assert.AreEqual(0, number.Exponent);
		Assert.AreEqual(0, number.SignificantDigits);
	}

	[TestMethod]
	public void TestCreateFromIntegerPositiveNumber()
	{
		var number = SignificantNumber.CreateFromInteger(123000);
		Assert.AreEqual(123, number.Significand);
		Assert.AreEqual(3, number.Exponent);
		Assert.AreEqual(3, number.SignificantDigits);
	}

	[TestMethod]
	public void TestCreateFromIntegerNegativeNumber()
	{
		var number = SignificantNumber.CreateFromInteger(-123000);
		Assert.AreEqual(-123, number.Significand);
		Assert.AreEqual(3, number.Exponent);
		Assert.AreEqual(3, number.SignificantDigits);
	}

	[TestMethod]
	public void TestCreateFromIntegerOne()
	{
		var number = SignificantNumber.CreateFromInteger(1);
		Assert.AreEqual(1, number.Significand);
		Assert.AreEqual(0, number.Exponent);
		Assert.AreEqual(1, number.SignificantDigits);
	}

	[TestMethod]
	public void TestCreateFromIntegerNegativeOne()
	{
		var number = SignificantNumber.CreateFromInteger(-1);
		Assert.AreEqual(-1, number.Significand);
		Assert.AreEqual(0, number.Exponent);
		Assert.AreEqual(1, number.SignificantDigits);
	}

	[TestMethod]
	public void TestCreateFromIntegerZero()
	{
		var number = SignificantNumber.CreateFromInteger(0000);
		Assert.AreEqual(0, number.Significand);
		Assert.AreEqual(0, number.Exponent);
		Assert.AreEqual(0, number.SignificantDigits);
	}

	[TestMethod]
	public void TestMaximumBigInteger()
	{
		var maxBigInt = BigInteger.Parse("79228162514264337593543950335"); // Decimal.MaxValue
		var number = new SignificantNumber(0, maxBigInt);
		Assert.AreEqual(maxBigInt, number.Significand);
	}

	[TestMethod]
	public void TestMinimumBigInteger()
	{
		var minBigInt = BigInteger.Parse("-79228162514264337593543950335"); // Decimal.MinValue
		var number = new SignificantNumber(0, minBigInt);
		Assert.AreEqual(minBigInt, number.Significand);
	}

	[TestMethod]
	public void TestNegativeExponent()
	{
		var number = new SignificantNumber(-5, 12345);
		Assert.AreEqual(12345, number.Significand);
		Assert.AreEqual(-5, number.Exponent);
	}

	[TestMethod]
	public void TestTrailingZerosBoundary()
	{
		var number = new SignificantNumber(2, 123000, true);
		Assert.AreEqual(123, number.Significand);
		Assert.AreEqual(5, number.Exponent);
	}

	[TestMethod]
	public void TestToStringWithFormat()
	{
		var number = new SignificantNumber(2, 12345);
		string str = number.ToString("G");
		Assert.AreEqual("1234500", str);
	}

	[TestMethod]
	public void TestToStringWithDifferentCulture()
	{
		var number = new SignificantNumber(-2, 12345);
		string str = number.ToString(CultureInfo.GetCultureInfo("fr-FR"));
		Assert.AreEqual("123,45", str);
	}

	[TestMethod]
	public void TestParseWithDifferentCulture()
	{
		string str = "123,45";
		var culture = CultureInfo.GetCultureInfo("fr-FR");
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.Parse(str.AsSpan(), culture));
	}

	[TestMethod]
	public void TestAdditionWithLargeNumbers()
	{
		var largeNum1 = SignificantNumber.CreateFromInteger(BigInteger.Parse("79228162514264337593543950335"));
		var largeNum2 = SignificantNumber.CreateFromInteger(BigInteger.Parse("79228162514264337593543950335"));
		var result = largeNum1 + largeNum2;
		Assert.AreEqual(BigInteger.Parse("15845632502852867518708790067"), result.Significand);
		Assert.AreEqual(1, result.Exponent);
	}

	[TestMethod]
	public void TestSubtractionWithLargeNumbers()
	{
		var largeNum1 = SignificantNumber.CreateFromInteger(BigInteger.Parse("79228162514264337593543950335"));
		var largeNum2 = SignificantNumber.CreateFromInteger(BigInteger.Parse("39228162514264337593543950335"));
		var result = largeNum1 - largeNum2;
		Assert.AreEqual(4, result.Significand);
		Assert.AreEqual(28, result.Exponent);
	}

	[TestMethod]
	public void TestMultiplicationWithSmallNumbers()
	{
		var smallNum1 = SignificantNumber.CreateFromFloatingPoint(0.00001);
		var smallNum2 = SignificantNumber.CreateFromFloatingPoint(0.00002);
		var result = smallNum1 * smallNum2;
		Assert.AreEqual(2, result.Significand);
		Assert.AreEqual(-10, result.Exponent);
	}

	[TestMethod]
	public void TestDivisionWithSmallNumbers()
	{
		var smallNum1 = SignificantNumber.CreateFromFloatingPoint(0.00002);
		var smallNum2 = SignificantNumber.CreateFromFloatingPoint(0.00001);
		var result = smallNum1 / smallNum2;
		Assert.AreEqual(2, result.Significand);
		Assert.AreEqual(0, result.Exponent);
	}

	[TestMethod]
	public void TestRadix()
	{
		Assert.AreEqual(2, SignificantNumber.Radix);
	}

	[TestMethod]
	public void TestAdditiveIdentity()
	{
		var additiveIdentity = SignificantNumber.AdditiveIdentity;
		Assert.AreEqual(SignificantNumber.Zero, additiveIdentity);
	}

	[TestMethod]
	public void TestMultiplicativeIdentity()
	{
		var multiplicativeIdentity = SignificantNumber.MultiplicativeIdentity;
		Assert.AreEqual(SignificantNumber.One, multiplicativeIdentity);
	}

	[TestMethod]
	public void TestCreateRepeatingDigits()
	{
		var result = SignificantNumber.CreateRepeatingDigits(5, 3);
		Assert.AreEqual(new BigInteger(555), result);

		result = SignificantNumber.CreateRepeatingDigits(7, 0);
		Assert.AreEqual(new BigInteger(0), result);
	}

	[TestMethod]
	public void TestHasInfinitePrecision()
	{
		var number = SignificantNumber.One;
		Assert.IsTrue(number.HasInfinitePrecision);

		number = new SignificantNumber(0, new BigInteger(2));
		Assert.IsFalse(number.HasInfinitePrecision);
	}

	[TestMethod]
	public void TestLowestDecimalDigits()
	{
		var number1 = new SignificantNumber(-2, 12345);
		var number2 = new SignificantNumber(-3, 678);
		int result = SignificantNumber.LowestDecimalDigits(number1, number2);
		Assert.AreEqual(2, result);
	}

	[TestMethod]
	public void TestLowestSignificantDigits()
	{
		var number1 = new SignificantNumber(0, 12345);
		var number2 = new SignificantNumber(0, 678);
		int result = SignificantNumber.LowestSignificantDigits(number1, number2);
		Assert.AreEqual(3, result);
	}

	[TestMethod]
	public void TestCountDecimalDigits()
	{
		var number = new SignificantNumber(-2, 12345);
		int result = number.CountDecimalDigits();
		Assert.AreEqual(2, result);
	}

	[TestMethod]
	public void TestReduceSignificance()
	{
		var number = new SignificantNumber(0, 12345);
		var result = number.ReduceSignificance(3);
		Assert.AreEqual(124, result.Significand);
		Assert.AreEqual(2, result.Exponent);
	}

	[TestMethod]
	public void TestMakeCommonizedAndGetExponent()
	{
		var number1 = new SignificantNumber(1, 123);
		var number2 = new SignificantNumber(3, 456);
		int result = SignificantNumber.MakeCommonizedAndGetExponent(ref number1, ref number2);
		Assert.AreEqual(1, result);
		Assert.AreEqual(123, number1.Significand);
		Assert.AreEqual(45600, number2.Significand);
	}

	[TestMethod]
	public void TestAbsStatic()
	{
		var negative = SignificantNumber.NegativeOne;
		var result = SignificantNumber.Abs(negative);
		Assert.AreEqual(SignificantNumber.One, result);
	}

	[TestMethod]
	public void TestAssertExponentsMatch()
	{
		var number1 = new SignificantNumber(1, 123);
		var number2 = new SignificantNumber(1, 456);
		SignificantNumber.AssertExponentsMatch(number1, number2);
		// No assertion needed, just ensure no exception is thrown
	}

	[TestMethod]
	public void TestOperatorNegate()
	{
		var number = SignificantNumber.One;
		var result = -number;
		Assert.AreEqual(SignificantNumber.NegativeOne, result);
	}

	[TestMethod]
	public void TestOperatorAdd()
	{
		var number1 = new SignificantNumber(-2, 12345);
		var number2 = new SignificantNumber(-3, 678);
		var result = number1 + number2;
		Assert.AreEqual(12413, result.Significand);
		Assert.AreEqual(-2, result.Exponent);
	}

	[TestMethod]
	public void TestOperatorSubtract()
	{
		var number1 = new SignificantNumber(-2, 12345);
		var number2 = new SignificantNumber(-3, 678);
		var result = number1 - number2;
		Assert.AreEqual(12277, result.Significand);
		Assert.AreEqual(-2, result.Exponent);
	}

	[TestMethod]
	public void TestOperatorMultiply()
	{
		var number1 = new SignificantNumber(-2, 12345);
		var number2 = new SignificantNumber(-3, 678);
		var result = number1 * number2;
		Assert.AreEqual(837, result.Significand);
		Assert.AreEqual(-1, result.Exponent);
	}

	[TestMethod]
	public void TestOperatorDivide()
	{
		var number1 = new SignificantNumber(-2, 12345);
		var number2 = new SignificantNumber(-3, 678);
		var result = number1 / number2;
		Assert.AreEqual(182, result.Significand);
		Assert.AreEqual(0, result.Exponent);
	}

	[TestMethod]
	public void TestOperatorGreaterThan()
	{
		var number1 = new SignificantNumber(0, 12345);
		var number2 = new SignificantNumber(0, 678);
		Assert.IsTrue(number1 > number2);
	}

	[TestMethod]
	public void TestOperatorLessThan()
	{
		var number1 = new SignificantNumber(0, 123);
		var number2 = new SignificantNumber(0, 678);
		Assert.IsTrue(number1 < number2);
	}

	[TestMethod]
	public void TestOperatorGreaterThanOrEqual()
	{
		var number1 = new SignificantNumber(0, 12345);
		var number2 = new SignificantNumber(0, 12345);
		Assert.IsTrue(number1 >= number2);
	}

	[TestMethod]
	public void TestOperatorLessThanOrEqual()
	{
		var number1 = new SignificantNumber(0, 123);
		var number2 = new SignificantNumber(0, 678);
		Assert.IsTrue(number1 <= number2);
	}

	[TestMethod]
	public void TestOperatorEqual()
	{
		var number1 = new SignificantNumber(0, 12345);
		var number2 = new SignificantNumber(0, 12345);
		Assert.AreEqual(number2, number1);
	}

	[TestMethod]
	public void TestOperatorNotEqual()
	{
		var number1 = new SignificantNumber(0, 12345);
		var number2 = new SignificantNumber(0, 678);
		Assert.AreNotEqual(number2, number1);
	}

	[TestMethod]
	public void TestGetHashCode()
	{
		var number1 = new SignificantNumber(2, 12345);
		var number2 = new SignificantNumber(2, 12345);
		var number3 = new SignificantNumber(3, 12345);

		// Test if the same values produce the same hash code
		Assert.AreEqual(number1.GetHashCode(), number2.GetHashCode());

		// Test if different values produce different hash codes
		Assert.AreNotEqual(number1.GetHashCode(), number3.GetHashCode());

		// Additional edge cases
		var zero = SignificantNumber.Zero;
		var one = SignificantNumber.One;
		var negativeOne = SignificantNumber.NegativeOne;

		Assert.AreEqual(zero.GetHashCode(), SignificantNumber.Zero.GetHashCode());
		Assert.AreEqual(one.GetHashCode(), SignificantNumber.One.GetHashCode());
		Assert.AreEqual(negativeOne.GetHashCode(), SignificantNumber.NegativeOne.GetHashCode());
	}

	[TestMethod]
	public void TestEqualsObjectSameInstance()
	{
		var number = SignificantNumber.One;
		Assert.IsTrue(number.Equals((object)number));
	}

	[TestMethod]
	public void TestEqualsObjectEquivalentInstance()
	{
		var number1 = SignificantNumber.One;
		var number2 = new SignificantNumber(0, 1);
		Assert.IsTrue(number1.Equals((object)number2));
	}

	[TestMethod]
	public void TestEqualsObjectDifferentInstance()
	{
		var number1 = SignificantNumber.One;
		var number2 = SignificantNumber.Zero;
		Assert.IsFalse(number1.Equals((object)number2));
	}

	[TestMethod]
	public void TestEqualsObjectNull()
	{
		var number = SignificantNumber.One;
		Assert.IsFalse(number.Equals(null));
	}

	[TestMethod]
	public void TestEqualsObjectDifferentType()
	{
		var number = SignificantNumber.One;
		string differentType = "1";
		Assert.IsFalse(number.Equals(differentType));
	}

	[TestMethod]
	public void TestToStringWithFormatAndInvariantCulture()
	{
		var number = new SignificantNumber(-2, 12345);
		string result = number.ToString("G", CultureInfo.InvariantCulture);
		Assert.AreEqual("123.45", result);
	}

	[TestMethod]
	public void TestToStringWithFormatAndSpecificCulture()
	{
		var number = new SignificantNumber(-2, 12345);
		string result = number.ToString("G", CultureInfo.GetCultureInfo("fr-FR"));
		Assert.AreEqual("123,45", result);
	}

	[TestMethod]
	public void TestToStringWithNullFormatAndInvariantCulture()
	{
		var number = new SignificantNumber(3, 12345);
		string result = number.ToString(null, CultureInfo.InvariantCulture);
		Assert.AreEqual("12345000", result);
	}

	[TestMethod]
	public void TestToStringWithNullFormatAndSpecificCulture()
	{
		var number = new SignificantNumber(3, 12345);
		string result = number.ToString(null, CultureInfo.GetCultureInfo("fr-FR"));
		Assert.AreEqual("12345000", result);
	}

	[TestMethod]
	public void TestToStringWithEmptyFormatAndInvariantCulture()
	{
		var number = new SignificantNumber(-2, 12345);
		string result = number.ToString("", CultureInfo.InvariantCulture);
		Assert.AreEqual("123.45", result);
	}

	[TestMethod]
	public void TestToStringWithEmptyFormatAndSpecificCulture()
	{
		var number = new SignificantNumber(-2, 12345);
		string result = number.ToString("", CultureInfo.GetCultureInfo("fr-FR"));
		Assert.AreEqual("123,45", result);
	}

	[TestMethod]
	public void TestTryFormatSufficientBuffer()
	{
		var number = new SignificantNumber(-2, 12345);
		Span<char> buffer = stackalloc char[50];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

		Assert.IsTrue(result);
		Assert.AreEqual("123.45", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void TestTryFormatInsufficientBuffer()
	{
		var number = new SignificantNumber(-2, 12345);
		Span<char> buffer = stackalloc char[4];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

		Assert.IsFalse(result);
		Assert.AreEqual(0, charsWritten);
	}

	[TestMethod]
	public void TestTryFormatEmptyFormat()
	{
		var number = new SignificantNumber(-2, 12345);
		Span<char> buffer = stackalloc char[50];
		string format = string.Empty;
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

		Assert.IsTrue(result);
		Assert.AreEqual("123.45", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void TestTryFormatInvalidFormat()
	{
		var number = new SignificantNumber(-2, 12345);
		Assert.ThrowsException<FormatException>(() => number.TryFormat(stackalloc char[50], out int charsWritten, "e", CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void TestTryFormatNullFormatProvider()
	{
		var number = new SignificantNumber(-2, 12345);
		Span<char> buffer = stackalloc char[50];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), null);

		Assert.IsTrue(result);
		Assert.AreEqual("123.45", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void TestTryFormatSpecificCulture()
	{
		var number = new SignificantNumber(-2, 12345);
		Span<char> buffer = stackalloc char[50];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.GetCultureInfo("fr-FR"));

		Assert.IsTrue(result);
		Assert.AreEqual("123,45", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void TestTryFormatZero()
	{
		var number = SignificantNumber.Zero;
		Span<char> buffer = stackalloc char[50];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

		Assert.IsTrue(result);
		Assert.AreEqual("0", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void TestTryFormatOne()
	{
		var number = SignificantNumber.One;
		Span<char> buffer = stackalloc char[50];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

		Assert.IsTrue(result);
		Assert.AreEqual("1", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void TestTryFormatNegativeOne()
	{
		var number = SignificantNumber.NegativeOne;
		Span<char> buffer = stackalloc char[50];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

		Assert.IsTrue(result);
		Assert.AreEqual("-1", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void TestTryFormatInteger()
	{
		var number = 3.ToSignificantNumber();
		Span<char> buffer = stackalloc char[50];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

		Assert.IsTrue(result);
		Assert.AreEqual("3", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void TestTryFormatFloat()
	{
		var number = 3.0.ToSignificantNumber();
		Span<char> buffer = stackalloc char[50];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

		Assert.IsTrue(result);
		Assert.AreEqual("3", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void TestAddLargeNumbers()
	{
		var largeNumber1 = new SignificantNumber(100, BigInteger.Parse("79228162514264337593543950335"));
		var largeNumber2 = new SignificantNumber(100, BigInteger.Parse("79228162514264337593543950335"));
		var result = largeNumber1 + largeNumber2;
		Assert.AreEqual(BigInteger.Parse("15845632502852867518708790067"), result.Significand);
		Assert.AreEqual(101, result.Exponent);
	}

	[TestMethod]
	public void TestSubtractLargeNumbers()
	{
		var largeNumber1 = new SignificantNumber(100, BigInteger.Parse("79228162514264337593543950335"));
		var largeNumber2 = new SignificantNumber(100, BigInteger.Parse("39228162514264337593543950335"));
		var result = largeNumber1 - largeNumber2;
		Assert.AreEqual(BigInteger.Parse("4"), result.Significand);
		Assert.AreEqual(128, result.Exponent);
	}

	[TestMethod]
	public void TestMultiplyLargeNumbers()
	{
		var largeNumber1 = new SignificantNumber(50, BigInteger.Parse("79228162514264337593543950335"));
		var largeNumber2 = new SignificantNumber(50, BigInteger.Parse("2"));
		var result = largeNumber1 * largeNumber2;
		Assert.AreEqual(BigInteger.Parse("2"), result.Significand);
		Assert.AreEqual(129, result.Exponent);
	}

	[TestMethod]
	public void TestDivideLargeNumbers()
	{
		var largeNumber1 = new SignificantNumber(100, BigInteger.Parse("79228162514264337593543950335"));
		var largeNumber2 = new SignificantNumber(1, BigInteger.Parse("2"));
		var result = largeNumber1 / largeNumber2;
		Assert.AreEqual(BigInteger.Parse("4"), result.Significand);
		Assert.AreEqual(127, result.Exponent);
	}

	[TestMethod]
	public void TestAddZero()
	{
		var zero = SignificantNumber.Zero;
		var one = SignificantNumber.One;
		var result = zero + one;
		Assert.AreEqual(one, result);
	}

	[TestMethod]
	public void TestSubtractZero()
	{
		var zero = SignificantNumber.Zero;
		var one = SignificantNumber.One;
		var result = one - zero;
		Assert.AreEqual(one, result);
	}

	[TestMethod]
	public void TestMultiplyZero()
	{
		var zero = SignificantNumber.Zero;
		var one = SignificantNumber.One;
		var result = one * zero;
		Assert.AreEqual(zero, result);
	}

	[TestMethod]
	public void TestDivideZero()
	{
		var zero = SignificantNumber.Zero;
		Assert.ThrowsException<DivideByZeroException>(() => zero / zero);
	}

	[TestMethod]
	public void TestCreateFromFloatingPointSpecialValues()
	{
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => SignificantNumber.CreateFromFloatingPoint(double.NaN));
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => SignificantNumber.CreateFromFloatingPoint(double.PositiveInfinity));
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => SignificantNumber.CreateFromFloatingPoint(double.NegativeInfinity));
	}

	[TestMethod]
	public void TestCreateFromIntegerBoundaryValues()
	{
		var intMax = SignificantNumber.CreateFromInteger(int.MaxValue);
		Assert.AreEqual(BigInteger.Parse(int.MaxValue.ToString()), intMax.Significand);

		var intMin = SignificantNumber.CreateFromInteger(int.MinValue);
		Assert.AreEqual(BigInteger.Parse(int.MinValue.ToString()), intMin.Significand);

		var longMax = SignificantNumber.CreateFromInteger(long.MaxValue);
		Assert.AreEqual(BigInteger.Parse(long.MaxValue.ToString()), longMax.Significand);

		var longMin = SignificantNumber.CreateFromInteger(long.MinValue);
		Assert.AreEqual(BigInteger.Parse(long.MinValue.ToString()), longMin.Significand);
	}

	[TestMethod]
	public void TestNegativeExponentHandling()
	{
		var number = new SignificantNumber(-3, 12345);
		Assert.AreEqual(12345, number.Significand);
		Assert.AreEqual(-3, number.Exponent);

		var result = number.Round(2);
		Assert.AreEqual(1235, result.Significand); // After rounding, check if the exponent and significand are adjusted correctly
		Assert.AreEqual(-2, result.Exponent);
	}

	[TestMethod]
	public void TestHandlingTrailingZeros()
	{
		var number = new SignificantNumber(2, 123000, true);
		Assert.AreEqual(123, number.Significand);
		Assert.AreEqual(5, number.Exponent); // Ensure trailing zeros are removed and exponent is adjusted correctly

		number = new SignificantNumber(-2, 123000, true);
		Assert.AreEqual(123, number.Significand);
		Assert.AreEqual(1, number.Exponent);
	}

	[TestMethod]
	public void TestToStringVariousFormats()
	{
		var number = new SignificantNumber(-2, 12345);
		Assert.ThrowsException<FormatException>(() => number.ToString("E2", CultureInfo.InvariantCulture));
		Assert.ThrowsException<FormatException>(() => number.ToString("F2", CultureInfo.InvariantCulture));
		Assert.ThrowsException<FormatException>(() => number.ToString("N2", CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void TestTryFormatVariousFormats()
	{
		var number = new SignificantNumber(-2, 12345);
		Assert.ThrowsException<FormatException>(() => number.TryFormat(stackalloc char[50], out int charsWritten, "E2".AsSpan(), CultureInfo.InvariantCulture));
		Assert.ThrowsException<FormatException>(() => number.TryFormat(stackalloc char[50], out int charsWritten, "F2".AsSpan(), CultureInfo.InvariantCulture));
		Assert.ThrowsException<FormatException>(() => number.TryFormat(stackalloc char[50], out int charsWritten, "N2".AsSpan(), CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void ToDouble()
	{
		var significantNumber = new SignificantNumber(3, 12345); // 12345e3
		double result = significantNumber.To<double>();
		Assert.AreEqual(12345e3, result);
	}

	[TestMethod]
	public void ToFloat()
	{
		var significantNumber = new SignificantNumber(2, 12345); // 12345e2
		float result = significantNumber.To<float>();
		Assert.AreEqual(12345e2f, result);
	}

	[TestMethod]
	public void ToDecimal()
	{
		var significantNumber = new SignificantNumber(1, 12345); // 12345e1
		decimal result = significantNumber.To<decimal>();
		Assert.AreEqual(12345e1m, result);
	}

	[TestMethod]
	public void ToInt()
	{
		var significantNumber = new SignificantNumber(0, 12345); // 12345e0
		int result = significantNumber.To<int>();
		Assert.AreEqual(12345, result);
	}

	[TestMethod]
	public void ToLong()
	{
		var significantNumber = new SignificantNumber(0, 123456789012345); // 123456789012345e0
		long result = significantNumber.To<long>();
		Assert.AreEqual(123456789012345L, result);
	}

	[TestMethod]
	public void ToBigInteger()
	{
		var significantNumber = new SignificantNumber(5, 12345); // 12345e5
		var result = significantNumber.To<BigInteger>();
		Assert.AreEqual(BigInteger.Parse("1234500000"), result);
	}

	[TestMethod]
	public void ToOverflow()
	{
		var significantNumber = new SignificantNumber(1000, 12345); // This is a very large number
		Assert.ThrowsException<OverflowException>(() => significantNumber.To<int>()); // This should throw an exception
	}

	[TestMethod]
	public void SquaredShouldReturnCorrectValue()
	{
		// Arrange
		var number = 3.ToSignificantNumber();
		var expected = 9.ToSignificantNumber();

		// Act
		var result = number.Squared();

		// Assert
		Assert.AreEqual(expected, result);
	}

	[TestMethod]
	public void CubedShouldReturnCorrectValue()
	{
		// Arrange
		var number = 3.ToSignificantNumber();
		var expected = 27.ToSignificantNumber();

		// Act
		var result = number.Cubed();

		// Assert
		Assert.AreEqual(expected, result);
	}

	[TestMethod]
	public void PowShouldReturnCorrectValue()
	{
		// Arrange
		var number = 2.ToSignificantNumber();
		var expected = 8.ToSignificantNumber();

		// Act
		var result = number.Pow(3.ToSignificantNumber());

		// Assert
		Assert.AreEqual(expected, result);
	}

	[TestMethod]
	public void PowZeroPowerShouldReturnOne()
	{
		// Arrange
		var number = 5.ToSignificantNumber();
		var expected = SignificantNumber.One;

		// Act
		var result = number.Pow(0.ToSignificantNumber());

		// Assert
		Assert.AreEqual(expected, result);
	}

	[TestMethod]
	public void PowNegativePowerShouldReturnCorrectValue()
	{
		// Arrange
		var number = 2.ToSignificantNumber();
		var expected = 0.125.ToSignificantNumber();

		// Act
		var result = number.Pow(-3.ToSignificantNumber());

		// Assert
		Assert.AreEqual(expected, result);
	}

	[TestMethod]
	public void TestExpWithZeroPower()
	{
		var result = SignificantNumber.Exp(0.ToSignificantNumber());
		var expected = SignificantNumber.One; // e^0 = 1
		Assert.AreEqual(expected, result);
	}

	[TestMethod]
	public void TestExpWithPositivePower()
	{
		var result = SignificantNumber.Exp(1.ToSignificantNumber());
		var expected = SignificantNumber.E; // e^1 = e
		Assert.AreEqual(expected, result);
	}

	[TestMethod]
	public void TestExpWithNegativePower()
	{
		var result = SignificantNumber.Exp(-1.ToSignificantNumber());
		var expected = SignificantNumber.One / SignificantNumber.E; // e^-1 = 1/e
		Assert.AreEqual(expected, result);
	}

	[TestMethod]
	public void TestExpWithLargePositivePower()
	{
		var result = SignificantNumber.Exp(5.ToSignificantNumber());
		var expected = 200.ToSignificantNumber();
		Assert.AreEqual(expected, result);
	}

	[TestMethod]
	public void TestExpWithLargeNegativePower()
	{
		var result = SignificantNumber.Exp(-5.ToSignificantNumber());
		var expected = 0.006737947.ToSignificantNumber();
		Assert.AreEqual(expected, result);
	}
}
