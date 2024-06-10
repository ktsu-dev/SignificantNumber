namespace ktsu.io.SignificantNumber.Test;

using System.Globalization;
using ktsu.io.SignificantNumber;

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
		Assert.AreEqual(2, number.Exponent);
		Assert.AreEqual(0, number.SignificantDigits);
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
	public void Test_CreateFromFloatingPoint_Zero()
	{
		var number = SignificantNumber.CreateFromFloatingPoint(0000.0);
		Assert.AreEqual(0, number.Significand);
		Assert.AreEqual(0, number.Exponent);
		Assert.AreEqual(0, number.SignificantDigits);
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
	public void Test_CreateFromInteger_Zero()
	{
		var number = SignificantNumber.CreateFromInteger(0000);
		Assert.AreEqual(0, number.Significand);
		Assert.AreEqual(0, number.Exponent);
		Assert.AreEqual(0, number.SignificantDigits);
	}
}
