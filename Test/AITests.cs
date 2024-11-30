// Ignore Spelling: Commonized

namespace ktsu.SignificantNumber.Test;

using System.Globalization;
using System.Numerics;

[TestClass]
public class AITests
{
	[TestMethod]
	public void Test_Zero()
	{
		var zero = SignificantNumber.Zero;
		Assert.AreEqual(0, zero.Significand);
		Assert.AreEqual(0, zero.Exponent);
	}

	[TestMethod]
	public void Test_One()
	{
		var one = SignificantNumber.One;
		Assert.AreEqual(1, one.Significand);
		Assert.AreEqual(0, one.Exponent);
	}

	[TestMethod]
	public void Test_NegativeOne()
	{
		var negativeOne = SignificantNumber.NegativeOne;
		Assert.AreEqual(-1, negativeOne.Significand);
		Assert.AreEqual(0, negativeOne.Exponent);
	}

	[TestMethod]
	public void Test_Add()
	{
		var one = SignificantNumber.One;
		var result = one + one;
		Assert.AreEqual(2.ToSignificantNumber(), result);
	}

	[TestMethod]
	public void Test_Subtract()
	{
		var one = SignificantNumber.One;
		var result = one - one;
		Assert.AreEqual(SignificantNumber.Zero, result);
	}

	[TestMethod]
	public void Test_Multiply()
	{
		var one = SignificantNumber.One;
		var result = one * one;
		Assert.AreEqual(SignificantNumber.One, result);
	}

	[TestMethod]
	public void Test_Divide()
	{
		var one = SignificantNumber.One;
		var result = one / one;
		Assert.AreEqual(SignificantNumber.One, result);
	}

	[TestMethod]
	public void Test_Round()
	{
		var number = 1.2345.ToSignificantNumber();
		var rounded = number.Round(2);
		Assert.AreEqual(1.24.ToSignificantNumber(), rounded);
	}

	[TestMethod]
	public void Test_Abs()
	{
		var negative = SignificantNumber.NegativeOne;
		var positive = negative.Abs();
		Assert.AreEqual(SignificantNumber.One, positive);
	}

	[TestMethod]
	public void Test_Clamp()
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
	public void Test_ToString()
	{
		var number = 0.0123.ToSignificantNumber();
		string str = number.ToString();
		Assert.AreEqual("0.0123", str);
	}

	[TestMethod]
	public void Test_Equals()
	{
		var one = SignificantNumber.One;
		Assert.IsTrue(one.Equals(SignificantNumber.One));
		Assert.IsFalse(one.Equals(SignificantNumber.Zero));
	}

	[TestMethod]
	public void Test_CompareTo()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(one.CompareTo(zero) > 0);
		Assert.IsTrue(zero.CompareTo(one) < 0);
		Assert.IsTrue(one.CompareTo(SignificantNumber.One) == 0);
	}

	// Tests for comparison operators
	[TestMethod]
	public void Test_GreaterThan()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(one > zero);
		Assert.IsFalse(zero > one);
	}

	[TestMethod]
	public void Test_GreaterThanOrEqual()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(one >= zero);
		Assert.IsTrue(one >= SignificantNumber.One);
		Assert.IsFalse(zero >= one);
	}

	[TestMethod]
	public void Test_LessThan()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(zero < one);
		Assert.IsFalse(one < zero);
	}

	[TestMethod]
	public void Test_LessThanOrEqual()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(zero <= one);
		Assert.IsTrue(one <= SignificantNumber.One);
		Assert.IsFalse(one <= zero);
	}

	[TestMethod]
	public void Test_Equality()
	{
		var one = SignificantNumber.One;
		var anotherOne = 1.ToSignificantNumber();
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(one == anotherOne);
		Assert.IsFalse(one == zero);
	}

	[TestMethod]
	public void Test_Inequality()
	{
		var one = SignificantNumber.One;
		var anotherOne = 1.ToSignificantNumber();
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(one != zero);
		Assert.IsFalse(one != anotherOne);
	}

	// Tests for unsupported operators
	[TestMethod]
	public void Test_Modulus()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => one % one);
	}

	[TestMethod]
	public void Test_Decrement()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => --one);
	}

	[TestMethod]
	public void Test_Increment()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => ++one);
	}

	// Test for unary + operator
	[TestMethod]
	public void Test_UnaryPlus()
	{
		var one = SignificantNumber.One;
		var result = +one;
		Assert.AreEqual(SignificantNumber.One, result);
	}

	// Tests for static methods of unary operators
	[TestMethod]
	public void Test_StaticUnaryPlus()
	{
		var one = SignificantNumber.One;
		var result = SignificantNumber.Plus(one);
		Assert.AreEqual(SignificantNumber.One, result);
	}

	[TestMethod]
	public void Test_StaticUnaryNegate()
	{
		var one = SignificantNumber.One;
		var result = SignificantNumber.Negate(one);
		Assert.AreEqual(SignificantNumber.NegativeOne, result);
	}

	// Tests for static methods of binary operators
	[TestMethod]
	public void Test_StaticAdd()
	{
		var one = SignificantNumber.One;
		var result = SignificantNumber.Add(one, one);
		Assert.AreEqual(2.ToSignificantNumber(), result);
	}

	[TestMethod]
	public void Test_StaticSubtract()
	{
		var one = SignificantNumber.One;
		var result = SignificantNumber.Subtract(one, one);
		Assert.AreEqual(SignificantNumber.Zero, result);
	}

	[TestMethod]
	public void Test_StaticMultiply()
	{
		var one = SignificantNumber.One;
		var result = SignificantNumber.Multiply(one, one);
		Assert.AreEqual(SignificantNumber.One, result);
	}

	[TestMethod]
	public void Test_StaticDivide()
	{
		var one = SignificantNumber.One;
		var result = SignificantNumber.Divide(one, one);
		Assert.AreEqual(SignificantNumber.One, result);
	}

	[TestMethod]
	public void Test_StaticModulus()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.Mod(one, one));
	}

	// Test for static increment method
	[TestMethod]
	public void Test_StaticIncrement()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.Increment(one));
	}

	// Test for static decrement method
	[TestMethod]
	public void Test_StaticDecrement()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.Decrement(one));
	}

	[TestMethod]
	public void Test_StaticGreaterThan()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(SignificantNumber.GreaterThan(one, zero));
		Assert.IsFalse(SignificantNumber.GreaterThan(zero, one));
	}

	[TestMethod]
	public void Test_StaticGreaterThanOrEqual()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(SignificantNumber.GreaterThanOrEqual(one, zero));
		Assert.IsTrue(SignificantNumber.GreaterThanOrEqual(one, SignificantNumber.One));
		Assert.IsFalse(SignificantNumber.GreaterThanOrEqual(zero, one));
	}

	[TestMethod]
	public void Test_StaticLessThan()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(SignificantNumber.LessThan(zero, one));
		Assert.IsFalse(SignificantNumber.LessThan(one, zero));
	}

	[TestMethod]
	public void Test_StaticLessThanOrEqual()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(SignificantNumber.LessThanOrEqual(zero, one));
		Assert.IsTrue(SignificantNumber.LessThanOrEqual(one, SignificantNumber.One));
		Assert.IsFalse(SignificantNumber.LessThanOrEqual(one, zero));
	}

	[TestMethod]
	public void Test_StaticEqual()
	{
		var one = SignificantNumber.One;
		var anotherOne = 1.ToSignificantNumber();
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(SignificantNumber.Equal(one, anotherOne));
		Assert.IsFalse(SignificantNumber.Equal(one, zero));
	}

	[TestMethod]
	public void Test_StaticNotEqual()
	{
		var one = SignificantNumber.One;
		var anotherOne = 1.ToSignificantNumber();
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(SignificantNumber.NotEqual(one, zero));
		Assert.IsFalse(SignificantNumber.NotEqual(one, anotherOne));
	}

	// Test for static Max method
	[TestMethod]
	public void Test_StaticMax()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		var result = SignificantNumber.Max(one, zero);
		Assert.AreEqual(one, result);
	}

	// Test for static Min method
	[TestMethod]
	public void Test_StaticMin()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		var result = SignificantNumber.Min(one, zero);
		Assert.AreEqual(zero, result);
	}

	// Test for static Clamp method
	[TestMethod]
	public void Test_StaticClamp()
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
	public void Test_StaticRound()
	{
		var number = 1.2345.ToSignificantNumber();
		var result = SignificantNumber.Round(number, 2);
		Assert.AreEqual(1.24.ToSignificantNumber(), result);
	}

	[TestMethod]
	public void Test_TryConvertFromChecked()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryConvertFromChecked(one, out var result));
	}

	[TestMethod]
	public void Test_TryConvertFromSaturating()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryConvertFromSaturating(one, out var result));
	}

	[TestMethod]
	public void Test_TryConvertFromTruncating()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryConvertFromTruncating(one, out var result));
	}

	[TestMethod]
	public void Test_TryConvertToChecked()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryConvertToChecked(one, out SignificantNumber result));
	}

	[TestMethod]
	public void Test_TryConvertToSaturating()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryConvertToSaturating(one, out SignificantNumber result));
	}

	[TestMethod]
	public void Test_TryConvertToTruncating()
	{
		var one = SignificantNumber.One;
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryConvertToTruncating(one, out SignificantNumber result));
	}

	[TestMethod]
	public void Test_IsCanonical()
	{
		var one = SignificantNumber.One;
		Assert.IsTrue(SignificantNumber.IsCanonical(one));
	}

	[TestMethod]
	public void Test_IsComplexNumber()
	{
		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsComplexNumber(one));
	}

	[TestMethod]
	public void Test_IsEvenInteger()
	{
		var two = 2.ToSignificantNumber();
		Assert.IsTrue(SignificantNumber.IsEvenInteger(two));

		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsEvenInteger(one));
	}

	[TestMethod]
	public void Test_IsFinite()
	{
		var one = SignificantNumber.One;
		Assert.IsTrue(SignificantNumber.IsFinite(one));
	}

	[TestMethod]
	public void Test_IsImaginaryNumber()
	{
		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsImaginaryNumber(one));
	}

	[TestMethod]
	public void Test_IsInfinity()
	{
		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsInfinity(one));
	}

	[TestMethod]
	public void Test_IsInteger()
	{
		var one = SignificantNumber.One;
		Assert.IsTrue(SignificantNumber.IsInteger(one));
	}

	[TestMethod]
	public void Test_IsNaN()
	{
		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsNaN(one));
	}

	[TestMethod]
	public void Test_IsNegative()
	{
		var negativeOne = SignificantNumber.NegativeOne;
		Assert.IsTrue(SignificantNumber.IsNegative(negativeOne));

		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsNegative(one));
	}

	[TestMethod]
	public void Test_IsNegativeInfinity()
	{
		var negativeOne = SignificantNumber.NegativeOne;
		Assert.IsFalse(SignificantNumber.IsNegativeInfinity(negativeOne));
	}

	[TestMethod]
	public void Test_IsNormal()
	{
		var one = SignificantNumber.One;
		Assert.IsTrue(SignificantNumber.IsNormal(one));
	}

	[TestMethod]
	public void Test_IsOddInteger()
	{
		var one = SignificantNumber.One;
		Assert.IsTrue(SignificantNumber.IsOddInteger(one));

		var two = 2.ToSignificantNumber();
		Assert.IsFalse(SignificantNumber.IsOddInteger(two));
	}

	[TestMethod]
	public void Test_IsPositive()
	{
		var one = SignificantNumber.One;
		Assert.IsTrue(SignificantNumber.IsPositive(one));

		var negativeOne = SignificantNumber.NegativeOne;
		Assert.IsFalse(SignificantNumber.IsPositive(negativeOne));
	}

	[TestMethod]
	public void Test_IsPositiveInfinity()
	{
		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsPositiveInfinity(one));
	}

	[TestMethod]
	public void Test_IsRealNumber()
	{
		var one = SignificantNumber.One;
		Assert.IsTrue(SignificantNumber.IsRealNumber(one));
	}

	[TestMethod]
	public void Test_IsSubnormal()
	{
		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsSubnormal(one));
	}

	[TestMethod]
	public void Test_IsZero()
	{
		var zero = SignificantNumber.Zero;
		Assert.IsTrue(SignificantNumber.IsZero(zero));

		var one = SignificantNumber.One;
		Assert.IsFalse(SignificantNumber.IsZero(one));
	}

	[TestMethod]
	public void Test_TryParse_ReadOnlySpan()
	{
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryParse("1.23e2".AsSpan(), NumberStyles.Float, CultureInfo.InvariantCulture, out var result));
	}

	[TestMethod]
	public void Test_TryParse_String_Style_Provider()
	{
		string input = "1.23e2";
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out var result));
	}

	[TestMethod]
	public void Test_TryParse_String_Provider()
	{
		string input = "1.23e2";
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryParse(input, CultureInfo.InvariantCulture, out var result));
	}

	[TestMethod]
	public void Test_TryParse_ReadOnlySpan_Provider()
	{
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.TryParse("1.23e2".AsSpan(), CultureInfo.InvariantCulture, out var result));
	}

	[TestMethod]
	public void Test_Parse_ReadOnlySpan_Style_Provider()
	{
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.Parse("1.23e2".AsSpan(), NumberStyles.Float, CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void Test_Parse_String_Style_Provider()
	{
		string input = "1.23e2";
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.Parse(input, NumberStyles.Float, CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void Test_Parse_String_Provider()
	{
		string input = "1.23e2";
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.Parse(input, CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void Test_Parse_ReadOnlySpan_Provider()
	{
		Assert.ThrowsException<NotSupportedException>(() => SignificantNumber.Parse("1.23e2".AsSpan(), CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void Test_StaticMaxMagnitude()
	{
		var one = SignificantNumber.One;
		var negativeOne = SignificantNumber.NegativeOne;
		var result = SignificantNumber.MaxMagnitude(one, negativeOne);
		Assert.AreEqual(one, result);
	}

	[TestMethod]
	public void Test_StaticMaxMagnitudeNumber()
	{
		var one = SignificantNumber.One;
		var negativeOne = SignificantNumber.NegativeOne;
		var result = SignificantNumber.MaxMagnitudeNumber(one, negativeOne);
		Assert.AreEqual(one, result);
	}

	[TestMethod]
	public void Test_StaticMinMagnitude()
	{
		var one = SignificantNumber.One;
		var negativeOne = SignificantNumber.NegativeOne;
		var result = SignificantNumber.MinMagnitude(one, negativeOne);
		Assert.AreEqual(one, result);
	}

	[TestMethod]
	public void Test_StaticMinMagnitudeNumber()
	{
		var one = SignificantNumber.One;
		var negativeOne = SignificantNumber.NegativeOne;
		var result = SignificantNumber.MinMagnitudeNumber(one, negativeOne);
		Assert.AreEqual(one, result);
	}

	[TestMethod]
	public void Test_CompareTo_Object()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		object oneObject = SignificantNumber.One;
		object zeroObject = SignificantNumber.Zero;
		object intObject = 1;
		Assert.IsTrue(one.CompareTo(oneObject) == 0);
		Assert.IsTrue(one.CompareTo(zeroObject) > 0);
		Assert.IsTrue(zero.CompareTo(oneObject) < 0);
		Assert.ThrowsException<NotSupportedException>(() => one.CompareTo(intObject));
	}

	[TestMethod]
	public void Test_CompareTo_SignificantNumber()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		var anotherOne = SignificantNumber.One;

		Assert.IsTrue(one.CompareTo(zero) > 0);
		Assert.IsTrue(zero.CompareTo(one) < 0);
		Assert.IsTrue(one.CompareTo(anotherOne) == 0);
	}

	[TestMethod]
	public void Test_CompareTo_INumber()
	{
		var one = SignificantNumber.One;
		var zero = SignificantNumber.Zero;
		var anotherOne = SignificantNumber.One;

		Assert.IsTrue(one.CompareTo<SignificantNumber>(zero) > 0);
		Assert.IsTrue(zero.CompareTo<SignificantNumber>(one) < 0);
		Assert.IsTrue(one.CompareTo<SignificantNumber>(anotherOne) == 0);

		Assert.IsTrue(one.CompareTo(0) > 0);
		Assert.IsTrue(zero.CompareTo(1) < 0);
		Assert.IsTrue(one.CompareTo(1) == 0);

		Assert.IsTrue(one.CompareTo(0.0) > 0);
		Assert.IsTrue(zero.CompareTo(1.0) < 0);
		Assert.IsTrue(one.CompareTo(1.0) == 0);
	}

	[TestMethod]
	public void Test_Constructor_PositiveNumber()
	{
		var number = new SignificantNumber(2, 123);
		Assert.AreEqual(123, number.Significand);
		Assert.AreEqual(2, number.Exponent);
		Assert.AreEqual(3, number.SignificantDigits);
	}

	[TestMethod]
	public void Test_Constructor_NegativeNumber()
	{
		var number = new SignificantNumber(2, -123);
		Assert.AreEqual(-123, number.Significand);
		Assert.AreEqual(2, number.Exponent);
		Assert.AreEqual(3, number.SignificantDigits);
	}

	[TestMethod]
	public void Test_Constructor_Zero()
	{
		var number = new SignificantNumber(2, 0);
		Assert.AreEqual(0, number.Significand);
		Assert.AreEqual(0, number.Exponent);
		Assert.AreEqual(1, number.SignificantDigits);
	}

	[TestMethod]
	public void Test_Constructor_SanitizeTrue()
	{
		var number = new SignificantNumber(2, 12300, true);
		Assert.AreEqual(123, number.Significand);
		Assert.AreEqual(4, number.Exponent);
		Assert.AreEqual(3, number.SignificantDigits);
	}

	[TestMethod]
	public void Test_Constructor_SanitizeFalse()
	{
		var number = new SignificantNumber(2, 12300, false);
		Assert.AreEqual(12300, number.Significand);
		Assert.AreEqual(2, number.Exponent);
		Assert.AreEqual(5, number.SignificantDigits);
	}

	[TestMethod]
	public void Test_CreateFromFloatingPoint_PositiveNumber()
	{
		var number = SignificantNumber.CreateFromFloatingPoint(123000.45);
		Assert.AreEqual(12300045, number.Significand);
		Assert.AreEqual(-2, number.Exponent);
		Assert.AreEqual(8, number.SignificantDigits);
	}

	[TestMethod]
	public void Test_CreateFromFloatingPoint_NegativeNumber()
	{
		var number = SignificantNumber.CreateFromFloatingPoint(-123000.45);
		Assert.AreEqual(-12300045, number.Significand);
		Assert.AreEqual(-2, number.Exponent);
		Assert.AreEqual(8, number.SignificantDigits);
	}

	[TestMethod]
	public void Test_CreateFromFloatingPoint_One()
	{
		var number = SignificantNumber.CreateFromFloatingPoint(1.0);
		Assert.AreEqual(1, number.Significand);
		Assert.AreEqual(0, number.Exponent);
		Assert.AreEqual(1, number.SignificantDigits);
	}

	[TestMethod]
	public void Test_CreateFromFloatingPoint_NegativeOne()
	{
		var number = SignificantNumber.CreateFromFloatingPoint(-1.0);
		Assert.AreEqual(-1, number.Significand);
		Assert.AreEqual(0, number.Exponent);
		Assert.AreEqual(1, number.SignificantDigits);
	}

	[TestMethod]
	public void Test_CreateFromFloatingPoint_Zero()
	{
		var number = SignificantNumber.CreateFromFloatingPoint(0000.0);
		Assert.AreEqual(0, number.Significand);
		Assert.AreEqual(0, number.Exponent);
		Assert.AreEqual(1, number.SignificantDigits);
	}

	[TestMethod]
	public void Test_CreateFromInteger_PositiveNumber()
	{
		var number = SignificantNumber.CreateFromInteger(123000);
		Assert.AreEqual(123, number.Significand);
		Assert.AreEqual(3, number.Exponent);
		Assert.AreEqual(3, number.SignificantDigits);
	}

	[TestMethod]
	public void Test_CreateFromInteger_NegativeNumber()
	{
		var number = SignificantNumber.CreateFromInteger(-123000);
		Assert.AreEqual(-123, number.Significand);
		Assert.AreEqual(3, number.Exponent);
		Assert.AreEqual(3, number.SignificantDigits);
	}

	[TestMethod]
	public void Test_CreateFromInteger_One()
	{
		var number = SignificantNumber.CreateFromInteger(1);
		Assert.AreEqual(1, number.Significand);
		Assert.AreEqual(0, number.Exponent);
		Assert.AreEqual(1, number.SignificantDigits);
	}

	[TestMethod]
	public void Test_CreateFromInteger_NegativeOne()
	{
		var number = SignificantNumber.CreateFromInteger(-1);
		Assert.AreEqual(-1, number.Significand);
		Assert.AreEqual(0, number.Exponent);
		Assert.AreEqual(1, number.SignificantDigits);
	}

	[TestMethod]
	public void Test_CreateFromInteger_Zero()
	{
		var number = SignificantNumber.CreateFromInteger(0000);
		Assert.AreEqual(0, number.Significand);
		Assert.AreEqual(0, number.Exponent);
		Assert.AreEqual(1, number.SignificantDigits);
	}

	[TestMethod]
	public void Test_MaximumBigInteger()
	{
		var maxBigInt = BigInteger.Parse("79228162514264337593543950335"); // Decimal.MaxValue
		var number = new SignificantNumber(0, maxBigInt);
		Assert.AreEqual(maxBigInt, number.Significand);
	}

	[TestMethod]
	public void Test_MinimumBigInteger()
	{
		var minBigInt = BigInteger.Parse("-79228162514264337593543950335"); // Decimal.MinValue
		var number = new SignificantNumber(0, minBigInt);
		Assert.AreEqual(minBigInt, number.Significand);
	}

	[TestMethod]
	public void Test_NegativeExponent()
	{
		var number = new SignificantNumber(-5, 12345);
		Assert.AreEqual(12345, number.Significand);
		Assert.AreEqual(-5, number.Exponent);
	}

	[TestMethod]
	public void Test_TrailingZerosBoundary()
	{
		var number = new SignificantNumber(2, 123000, true);
		Assert.AreEqual(123, number.Significand);
		Assert.AreEqual(5, number.Exponent);
	}

	[TestMethod]
	public void Test_ToString_WithFormat()
	{
		var number = new SignificantNumber(2, 12345);
		string str = number.ToString("G");
		Assert.AreEqual("1234500", str);
	}

	[TestMethod]
	public void Test_Addition_WithLargeNumbers()
	{
		var largeNum1 = SignificantNumber.CreateFromInteger(BigInteger.Parse("79228162514264337593543950335"));
		var largeNum2 = SignificantNumber.CreateFromInteger(BigInteger.Parse("79228162514264337593543950335"));
		var result = largeNum1 + largeNum2;
		Assert.AreEqual(BigInteger.Parse("15845632502852867518708790067"), result.Significand);
		Assert.AreEqual(1, result.Exponent);
	}

	[TestMethod]
	public void Test_Subtraction_WithLargeNumbers()
	{
		var largeNum1 = SignificantNumber.CreateFromInteger(BigInteger.Parse("79228162514264337593543950335"));
		var largeNum2 = SignificantNumber.CreateFromInteger(BigInteger.Parse("39228162514264337593543950335"));
		var result = largeNum1 - largeNum2;
		Assert.AreEqual(4, result.Significand);
		Assert.AreEqual(28, result.Exponent);
	}

	[TestMethod]
	public void Test_Multiplication_WithSmallNumbers()
	{
		var smallNum1 = SignificantNumber.CreateFromFloatingPoint(0.00001);
		var smallNum2 = SignificantNumber.CreateFromFloatingPoint(0.00002);
		var result = smallNum1 * smallNum2;
		Assert.AreEqual(2, result.Significand);
		Assert.AreEqual(-10, result.Exponent);
	}

	[TestMethod]
	public void Test_Division_WithSmallNumbers()
	{
		var smallNum1 = SignificantNumber.CreateFromFloatingPoint(0.00002);
		var smallNum2 = SignificantNumber.CreateFromFloatingPoint(0.00001);
		var result = smallNum1 / smallNum2;
		Assert.AreEqual(2, result.Significand);
		Assert.AreEqual(0, result.Exponent);
	}

	[TestMethod]
	public void Test_Radix()
	{
		Assert.AreEqual(2, SignificantNumber.Radix);
	}

	[TestMethod]
	public void Test_AdditiveIdentity()
	{
		var additiveIdentity = SignificantNumber.AdditiveIdentity;
		Assert.AreEqual(SignificantNumber.Zero, additiveIdentity);
	}

	[TestMethod]
	public void Test_MultiplicativeIdentity()
	{
		var multiplicativeIdentity = SignificantNumber.MultiplicativeIdentity;
		Assert.AreEqual(SignificantNumber.One, multiplicativeIdentity);
	}

	[TestMethod]
	public void Test_CreateRepeatingDigits()
	{
		var result = SignificantNumber.CreateRepeatingDigits(5, 3);
		Assert.AreEqual(new BigInteger(555), result);

		result = SignificantNumber.CreateRepeatingDigits(7, 0);
		Assert.AreEqual(new BigInteger(0), result);
	}

	[TestMethod]
	public void Test_HasInfinitePrecision()
	{
		var number = SignificantNumber.One;
		Assert.IsTrue(number.HasInfinitePrecision);

		number = new SignificantNumber(0, new BigInteger(2));
		Assert.IsFalse(number.HasInfinitePrecision);
	}

	[TestMethod]
	public void Test_LowestDecimalDigits()
	{
		var number1 = new SignificantNumber(-2, 12345);
		var number2 = new SignificantNumber(-3, 678);
		int result = SignificantNumber.LowestDecimalDigits(number1, number2);
		Assert.AreEqual(2, result);
	}

	[TestMethod]
	public void Test_LowestSignificantDigits()
	{
		var number1 = new SignificantNumber(0, 12345);
		var number2 = new SignificantNumber(0, 678);
		int result = SignificantNumber.LowestSignificantDigits(number1, number2);
		Assert.AreEqual(3, result);
	}

	[TestMethod]
	public void Test_CountDecimalDigits()
	{
		var number = new SignificantNumber(-2, 12345);
		int result = number.CountDecimalDigits();
		Assert.AreEqual(2, result);
	}

	[TestMethod]
	public void Test_ReduceSignificance()
	{
		var number = new SignificantNumber(0, 12345);
		var result = number.ReduceSignificance(3);
		Assert.AreEqual(124, result.Significand);
		Assert.AreEqual(2, result.Exponent);
	}

	[TestMethod]
	public void Test_MakeCommonizedAndGetExponent()
	{
		var number1 = new SignificantNumber(1, 123);
		var number2 = new SignificantNumber(3, 456);
		int result = SignificantNumber.MakeCommonizedAndGetExponent(ref number1, ref number2);
		Assert.AreEqual(1, result);
		Assert.AreEqual(123, number1.Significand);
		Assert.AreEqual(45600, number2.Significand);
	}

	[TestMethod]
	public void Test_Abs_Static()
	{
		var negative = SignificantNumber.NegativeOne;
		var result = SignificantNumber.Abs(negative);
		Assert.AreEqual(SignificantNumber.One, result);
	}

	[TestMethod]
	public void Test_AssertExponentsMatch()
	{
		var number1 = new SignificantNumber(1, 123);
		var number2 = new SignificantNumber(1, 456);
		SignificantNumber.AssertExponentsMatch(number1, number2);
		// No assertion needed, just ensure no exception is thrown
	}

	[TestMethod]
	public void Test_OperatorNegate()
	{
		var number = SignificantNumber.One;
		var result = -number;
		Assert.AreEqual(SignificantNumber.NegativeOne, result);
	}

	[TestMethod]
	public void Test_OperatorAdd()
	{
		var number1 = new SignificantNumber(-2, 12345);
		var number2 = new SignificantNumber(-3, 678);
		var result = number1 + number2;
		Assert.AreEqual(12413, result.Significand);
		Assert.AreEqual(-2, result.Exponent);
	}

	[TestMethod]
	public void Test_OperatorSubtract()
	{
		var number1 = new SignificantNumber(-2, 12345);
		var number2 = new SignificantNumber(-3, 678);
		var result = number1 - number2;
		Assert.AreEqual(12277, result.Significand);
		Assert.AreEqual(-2, result.Exponent);
	}

	[TestMethod]
	public void Test_OperatorMultiply()
	{
		var number1 = new SignificantNumber(-2, 12345);
		var number2 = new SignificantNumber(-3, 678);
		var result = number1 * number2;
		Assert.AreEqual(837, result.Significand);
		Assert.AreEqual(-1, result.Exponent);
	}

	[TestMethod]
	public void Test_OperatorDivide()
	{
		var number1 = new SignificantNumber(-2, 12345);
		var number2 = new SignificantNumber(-3, 678);
		var result = number1 / number2;
		Assert.AreEqual(182, result.Significand);
		Assert.AreEqual(0, result.Exponent);
	}

	[TestMethod]
	public void Test_OperatorGreaterThan()
	{
		var number1 = new SignificantNumber(0, 12345);
		var number2 = new SignificantNumber(0, 678);
		Assert.IsTrue(number1 > number2);
	}

	[TestMethod]
	public void Test_OperatorLessThan()
	{
		var number1 = new SignificantNumber(0, 123);
		var number2 = new SignificantNumber(0, 678);
		Assert.IsTrue(number1 < number2);
	}

	[TestMethod]
	public void Test_OperatorGreaterThanOrEqual()
	{
		var number1 = new SignificantNumber(0, 12345);
		var number2 = new SignificantNumber(0, 12345);
		Assert.IsTrue(number1 >= number2);
	}

	[TestMethod]
	public void Test_OperatorLessThanOrEqual()
	{
		var number1 = new SignificantNumber(0, 123);
		var number2 = new SignificantNumber(0, 678);
		Assert.IsTrue(number1 <= number2);
	}

	[TestMethod]
	public void Test_OperatorEqual()
	{
		var number1 = new SignificantNumber(0, 12345);
		var number2 = new SignificantNumber(0, 12345);
		Assert.IsTrue(number1 == number2);
	}

	[TestMethod]
	public void Test_OperatorNotEqual()
	{
		var number1 = new SignificantNumber(0, 12345);
		var number2 = new SignificantNumber(0, 678);
		Assert.IsTrue(number1 != number2);
	}

	[TestMethod]
	public void Test_GetHashCode()
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
	public void Test_Equals_Object_SameInstance()
	{
		var number = SignificantNumber.One;
		Assert.IsTrue(number.Equals((object)number));
	}

	[TestMethod]
	public void Test_Equals_Object_EquivalentInstance()
	{
		var number1 = SignificantNumber.One;
		var number2 = new SignificantNumber(0, 1);
		Assert.IsTrue(number1.Equals((object)number2));
	}

	[TestMethod]
	public void Test_Equals_Object_DifferentInstance()
	{
		var number1 = SignificantNumber.One;
		var number2 = SignificantNumber.Zero;
		Assert.IsFalse(number1.Equals((object)number2));
	}

	[TestMethod]
	public void Test_Equals_Object_Null()
	{
		var number = SignificantNumber.One;
		Assert.IsFalse(number.Equals(null));
	}

	[TestMethod]
	public void Test_Equals_Object_DifferentType()
	{
		var number = SignificantNumber.One;
		string differentType = "1";
		Assert.IsFalse(number.Equals(differentType));
	}

	[TestMethod]
	public void Test_ToString_WithFormat_AndInvariantCulture()
	{
		var number = new SignificantNumber(-2, 12345);
		string result = number.ToString("G", CultureInfo.InvariantCulture);
		Assert.AreEqual("123.45", result);
	}

	[TestMethod]
	public void Test_ToString_WithNullFormat_AndInvariantCulture()
	{
		var number = new SignificantNumber(3, 12345);
		string result = number.ToString(null, CultureInfo.InvariantCulture);
		Assert.AreEqual("12345000", result);
	}

	[TestMethod]
	public void Test_ToString_WithEmptyFormat_AndInvariantCulture()
	{
		var number = new SignificantNumber(-2, 12345);
		string result = number.ToString("", CultureInfo.InvariantCulture);
		Assert.AreEqual("123.45", result);
	}

	[TestMethod]
	public void Test_TryFormat_SufficientBuffer()
	{
		var number = new SignificantNumber(-2, 12345);
		Span<char> buffer = stackalloc char[50];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

		Assert.IsTrue(result);
		Assert.AreEqual("123.45", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void Test_TryFormat_InsufficientBuffer()
	{
		var number = new SignificantNumber(-2, 12345);
		Span<char> buffer = stackalloc char[4];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

		Assert.IsFalse(result);
		Assert.AreEqual(0, charsWritten);
	}

	[TestMethod]
	public void Test_TryFormat_EmptyFormat()
	{
		var number = new SignificantNumber(-2, 12345);
		Span<char> buffer = stackalloc char[50];
		string format = string.Empty;
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

		Assert.IsTrue(result);
		Assert.AreEqual("123.45", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void Test_TryFormat_InvalidFormat()
	{
		var number = new SignificantNumber(-2, 12345);
		Assert.ThrowsException<FormatException>(() => number.TryFormat(stackalloc char[50], out int charsWritten, "e", CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void Test_TryFormat_NullFormatProvider()
	{
		var number = new SignificantNumber(-2, 12345);
		Span<char> buffer = stackalloc char[50];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), null);

		Assert.IsTrue(result);
		Assert.AreEqual("123.45", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void Test_TryFormat_Zero()
	{
		var number = SignificantNumber.Zero;
		Span<char> buffer = stackalloc char[50];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

		Assert.IsTrue(result);
		Assert.AreEqual("0", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void Test_TryFormat_One()
	{
		var number = SignificantNumber.One;
		Span<char> buffer = stackalloc char[50];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

		Assert.IsTrue(result);
		Assert.AreEqual("1", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void Test_TryFormat_NegativeOne()
	{
		var number = SignificantNumber.NegativeOne;
		Span<char> buffer = stackalloc char[50];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

		Assert.IsTrue(result);
		Assert.AreEqual("-1", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void Test_TryFormat_Integer()
	{
		var number = 3.ToSignificantNumber();
		Span<char> buffer = stackalloc char[50];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

		Assert.IsTrue(result);
		Assert.AreEqual("3", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void Test_TryFormat_Float()
	{
		var number = 3.0.ToSignificantNumber();
		Span<char> buffer = stackalloc char[50];
		string format = "G";
		bool result = number.TryFormat(buffer, out int charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

		Assert.IsTrue(result);
		Assert.AreEqual("3", buffer[..charsWritten].ToString());
	}

	[TestMethod]
	public void Test_Add_LargeNumbers()
	{
		var largeNumber1 = new SignificantNumber(100, BigInteger.Parse("79228162514264337593543950335"));
		var largeNumber2 = new SignificantNumber(100, BigInteger.Parse("79228162514264337593543950335"));
		var result = largeNumber1 + largeNumber2;
		Assert.AreEqual(BigInteger.Parse("15845632502852867518708790067"), result.Significand);
		Assert.AreEqual(101, result.Exponent);
	}

	[TestMethod]
	public void Test_Subtract_LargeNumbers()
	{
		var largeNumber1 = new SignificantNumber(100, BigInteger.Parse("79228162514264337593543950335"));
		var largeNumber2 = new SignificantNumber(100, BigInteger.Parse("39228162514264337593543950335"));
		var result = largeNumber1 - largeNumber2;
		Assert.AreEqual(BigInteger.Parse("4"), result.Significand);
		Assert.AreEqual(128, result.Exponent);
	}

	[TestMethod]
	public void Test_Multiply_LargeNumbers()
	{
		var largeNumber1 = new SignificantNumber(50, BigInteger.Parse("79228162514264337593543950335"));
		var largeNumber2 = new SignificantNumber(50, BigInteger.Parse("2"));
		var result = largeNumber1 * largeNumber2;
		Assert.AreEqual(BigInteger.Parse("2"), result.Significand);
		Assert.AreEqual(129, result.Exponent);
	}

	[TestMethod]
	public void Test_Divide_LargeNumbers()
	{
		var largeNumber1 = new SignificantNumber(100, BigInteger.Parse("79228162514264337593543950335"));
		var largeNumber2 = new SignificantNumber(1, BigInteger.Parse("2"));
		var result = largeNumber1 / largeNumber2;
		Assert.AreEqual(BigInteger.Parse("4"), result.Significand);
		Assert.AreEqual(127, result.Exponent);
	}

	[TestMethod]
	public void Test_Add_Zero()
	{
		var zero = SignificantNumber.Zero;
		var one = SignificantNumber.One;
		var result = zero + one;
		Assert.AreEqual(one, result);
	}

	[TestMethod]
	public void Test_Subtract_Zero()
	{
		var zero = SignificantNumber.Zero;
		var one = SignificantNumber.One;
		var result = one - zero;
		Assert.AreEqual(one, result);
	}

	[TestMethod]
	public void Test_Multiply_Zero()
	{
		var zero = SignificantNumber.Zero;
		var one = SignificantNumber.One;
		var result = one * zero;
		Assert.AreEqual(zero, result);
	}

	[TestMethod]
	public void Test_Divide_Zero()
	{
		var zero = SignificantNumber.Zero;
		Assert.ThrowsException<DivideByZeroException>(() => zero / zero);
	}

	[TestMethod]
	public void Test_CreateFromFloatingPoint_SpecialValues()
	{
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => SignificantNumber.CreateFromFloatingPoint(double.NaN));
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => SignificantNumber.CreateFromFloatingPoint(double.PositiveInfinity));
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => SignificantNumber.CreateFromFloatingPoint(double.NegativeInfinity));
	}

	[TestMethod]
	public void Test_CreateFromInteger_BoundaryValues()
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
	public void Test_NegativeExponentHandling()
	{
		var number = new SignificantNumber(-3, 12345);
		Assert.AreEqual(12345, number.Significand);
		Assert.AreEqual(-3, number.Exponent);

		var result = number.Round(2);
		Assert.AreEqual(1235, result.Significand); // After rounding, check if the exponent and significand are adjusted correctly
		Assert.AreEqual(-2, result.Exponent);
	}

	[TestMethod]
	public void Test_HandlingTrailingZeros()
	{
		var number = new SignificantNumber(2, 123000, true);
		Assert.AreEqual(123, number.Significand);
		Assert.AreEqual(5, number.Exponent); // Ensure trailing zeros are removed and exponent is adjusted correctly

		number = new SignificantNumber(-2, 123000, true);
		Assert.AreEqual(123, number.Significand);
		Assert.AreEqual(1, number.Exponent);
	}

	[TestMethod]
	public void Test_ToString_VariousFormats()
	{
		var number = new SignificantNumber(-2, 12345);
		Assert.ThrowsException<FormatException>(() => number.ToString("E2", CultureInfo.InvariantCulture));
		Assert.ThrowsException<FormatException>(() => number.ToString("F2", CultureInfo.InvariantCulture));
		Assert.ThrowsException<FormatException>(() => number.ToString("N2", CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void Test_TryFormat_VariousFormats()
	{
		var number = new SignificantNumber(-2, 12345);
		Assert.ThrowsException<FormatException>(() => number.TryFormat(stackalloc char[50], out int charsWritten, "E2".AsSpan(), CultureInfo.InvariantCulture));
		Assert.ThrowsException<FormatException>(() => number.TryFormat(stackalloc char[50], out int charsWritten, "F2".AsSpan(), CultureInfo.InvariantCulture));
		Assert.ThrowsException<FormatException>(() => number.TryFormat(stackalloc char[50], out int charsWritten, "N2".AsSpan(), CultureInfo.InvariantCulture));
	}

	[TestMethod]
	public void To_Double()
	{
		var significantNumber = new SignificantNumber(3, 12345); // 12345e3
		double result = significantNumber.To<double>();
		Assert.AreEqual(12345e3, result);
	}

	[TestMethod]
	public void To_Float()
	{
		var significantNumber = new SignificantNumber(2, 12345); // 12345e2
		float result = significantNumber.To<float>();
		Assert.AreEqual(12345e2f, result);
	}

	[TestMethod]
	public void To_Decimal()
	{
		var significantNumber = new SignificantNumber(1, 12345); // 12345e1
		decimal result = significantNumber.To<decimal>();
		Assert.AreEqual(12345e1m, result);
	}

	[TestMethod]
	public void To_Int()
	{
		var significantNumber = new SignificantNumber(0, 12345); // 12345e0
		int result = significantNumber.To<int>();
		Assert.AreEqual(12345, result);
	}

	[TestMethod]
	public void To_Long()
	{
		var significantNumber = new SignificantNumber(0, 123456789012345); // 123456789012345e0
		long result = significantNumber.To<long>();
		Assert.AreEqual(123456789012345L, result);
	}

	[TestMethod]
	public void To_BigInteger()
	{
		var significantNumber = new SignificantNumber(5, 12345); // 12345e5
		var result = significantNumber.To<BigInteger>();
		Assert.AreEqual(BigInteger.Parse("1234500000"), result);
	}

	[TestMethod]
	public void To_Overflow()
	{
		var significantNumber = new SignificantNumber(1000, 12345); // This is a very large number
		Assert.ThrowsException<OverflowException>(() => significantNumber.To<int>()); // This should throw an exception
	}

	[TestMethod]
	public void Squared_ShouldReturnCorrectValue()
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
	public void Cubed_ShouldReturnCorrectValue()
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
	public void Pow_ShouldReturnCorrectValue()
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
	public void Pow_ZeroPower_ShouldReturnOne()
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
	public void Pow_NegativePower_ShouldReturnCorrectValue()
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
		var expected = 148.413159.ToSignificantNumber();
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
