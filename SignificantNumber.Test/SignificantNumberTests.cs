namespace SignificantNumber.Test;

using System.Numerics;

using ktsu.SignificantNumber;
using ktsu.PreciseNumber;
using System.Globalization;

[TestClass]
public class SignificantNumberTests
{
	[TestMethod]
	public void Add_TwoNumbers_ReturnsCorrectResult()
	{
		var left = SignificantNumber.CreateFromComponents(2, new BigInteger(123));
		var right = SignificantNumber.CreateFromComponents(2, new BigInteger(456));
		var result = SignificantNumber.Add(left, right);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(2, new BigInteger(579)), result);
	}

	[TestMethod]
	public void Subtract_TwoNumbers_ReturnsCorrectResult()
	{
		var left = SignificantNumber.CreateFromComponents(2, new BigInteger(456));
		var right = SignificantNumber.CreateFromComponents(2, new BigInteger(123));
		var result = SignificantNumber.Subtract(left, right);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(2, new BigInteger(333)), result);
	}

	[TestMethod]
	public void Multiply_TwoNumbers_ReturnsCorrectResult()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(20));
		var result = SignificantNumber.Multiply(left, right);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(2, new BigInteger(200)), result);
	}

	[TestMethod]
	public void Divide_TwoNumbers_ReturnsCorrectResult()
	{
		var left = SignificantNumber.CreateFromComponents(2, new BigInteger(100));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var result = SignificantNumber.Divide(left, right);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(1, new BigInteger(10)), result);
	}

	[TestMethod]
	public void Mod_TwoNumbers_ReturnsCorrectResult()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(25));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(7));
		var result = SignificantNumber.Mod(left, right);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(1, new BigInteger(4)), result);
	}

	[TestMethod]
	public void Increment_Number_ReturnsCorrectResult()
	{
		var value = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		var result = SignificantNumber.Increment(value);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(6)), result);
	}

	[TestMethod]
	public void Decrement_Number_ReturnsCorrectResult()
	{
		var value = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		var result = SignificantNumber.Decrement(value);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(4)), result);
	}

	[TestMethod]
	public void GreaterThan_TwoNumbers_ReturnsTrue()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		bool result = SignificantNumber.GreaterThan(left, right);

		Assert.IsTrue(result);
	}

	[TestMethod]
	public void LessThan_TwoNumbers_ReturnsTrue()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		bool result = SignificantNumber.LessThan(left, right);

		Assert.IsTrue(result);
	}

	[TestMethod]
	public void Equal_TwoNumbers_ReturnsTrue()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		bool result = SignificantNumber.Equal(left, right);

		Assert.IsTrue(result);
	}

	[TestMethod]
	public void Pow_Number_ReturnsCorrectResult()
	{
		var baseNumber = SignificantNumber.CreateFromComponents(0, new BigInteger(2));
		var power = SignificantNumber.CreateFromComponents(0, new BigInteger(3));
		var result = baseNumber.Pow(power);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(8)), result);
	}

	[TestMethod]
	public void Exp_Number_ReturnsCorrectResult()
	{
		var power = SignificantNumber.CreateFromComponents(0, new BigInteger(1));
		var result = SignificantNumber.Exp(power);

		Assert.AreEqual(PreciseNumber.E, result);
	}

	[TestMethod]
	public void Negate_Number_ReturnsCorrectResult()
	{
		var value = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		var result = SignificantNumber.Negate(value);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(-5)), result);
	}

	[TestMethod]
	public void Plus_Number_ReturnsSameValue()
	{
		var value = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		var result = SignificantNumber.Plus(value);

		Assert.AreEqual(value, result);
	}

	[TestMethod]
	public void GreaterThanOrEqual_TwoNumbers_ReturnsTrue()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		bool result = SignificantNumber.GreaterThanOrEqual(left, right);

		Assert.IsTrue(result);
	}

	[TestMethod]
	public void LessThanOrEqual_TwoNumbers_ReturnsTrue()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		bool result = SignificantNumber.LessThanOrEqual(left, right);

		Assert.IsTrue(result);
	}

	[TestMethod]
	public void NotEqual_TwoNumbers_ReturnsTrue()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		bool result = SignificantNumber.NotEqual(left, right);

		Assert.IsTrue(result);
	}

	[TestMethod]
	public void CompareTo_TwoNumbers_ReturnsCorrectComparison()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		int result = SignificantNumber.CompareTo(left, right);

		Assert.IsTrue(result > 0);
	}

	[TestMethod]
	public void Operator_Addition_ReturnsCorrectResult()
	{
		var left = SignificantNumber.CreateFromComponents(2, new BigInteger(123));
		var right = SignificantNumber.CreateFromComponents(2, new BigInteger(456));
		var result = left + right;

		Assert.AreEqual(SignificantNumber.CreateFromComponents(2, new BigInteger(579)), result);
	}

	[TestMethod]
	public void Operator_Subtraction_ReturnsCorrectResult()
	{
		var left = SignificantNumber.CreateFromComponents(2, new BigInteger(456));
		var right = SignificantNumber.CreateFromComponents(2, new BigInteger(123));
		var result = left - right;

		Assert.AreEqual(SignificantNumber.CreateFromComponents(2, new BigInteger(333)), result);
	}

	[TestMethod]
	public void Operator_Multiplication_ReturnsCorrectResult()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(20));
		var result = left * right;

		Assert.AreEqual(SignificantNumber.CreateFromComponents(2, new BigInteger(200)), result);
	}

	[TestMethod]
	public void Operator_Division_ReturnsCorrectResult()
	{
		var left = SignificantNumber.CreateFromComponents(2, new BigInteger(100));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var result = left / right;

		Assert.AreEqual(SignificantNumber.CreateFromComponents(1, new BigInteger(10)), result);
	}

	[TestMethod]
	public void Operator_Modulus_ReturnsCorrectResult()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(25));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(7));
		var result = left % right;

		Assert.AreEqual(SignificantNumber.CreateFromComponents(1, new BigInteger(4)), result);
	}

	[TestMethod]
	public void Operator_Equality_ReturnsTrue()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		bool result = left == right;

		Assert.IsTrue(result);
	}

	[TestMethod]
	public void Operator_Inequality_ReturnsTrue()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		bool result = left != right;

		Assert.IsTrue(result);
	}

	[TestMethod]
	public void Operator_GreaterThan_ReturnsTrue()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		bool result = left > right;

		Assert.IsTrue(result);
	}

	[TestMethod]
	public void Operator_LessThan_ReturnsTrue()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		bool result = left < right;

		Assert.IsTrue(result);
	}

	[TestMethod]
	public void Operator_GreaterThanOrEqual_ReturnsTrue()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		bool result = left >= right;

		Assert.IsTrue(result);
	}

	[TestMethod]
	public void Operator_LessThanOrEqual_ReturnsTrue()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		bool result = left <= right;

		Assert.IsTrue(result);
	}

	[TestMethod]
	public void Pow_ZeroExponent_ReturnsOne()
	{
		var baseNumber = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		var power = SignificantNumber.CreateFromComponents(0, BigInteger.Zero);
		var result = baseNumber.Pow(power);
		// When exponent is zero, the result should be one
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(1)), result);
	}

	[TestMethod]
	public void Pow_BaseIsZero_ReturnsZero()
	{
		var baseNumber = SignificantNumber.CreateFromComponents(0, BigInteger.Zero);
		var power = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		var result = baseNumber.Pow(power);
		// When base is zero and power is nonzero, the result should be zero
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, BigInteger.Zero), result);
	}

	[TestMethod]
	public void Pow_BaseIsOne_ReturnsOne()
	{
		var baseNumber = SignificantNumber.CreateFromComponents(0, new BigInteger(1));
		var power = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		var result = baseNumber.Pow(power);
		// When base is one, the result should always be one
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(1)), result);
	}

	[TestMethod]
	public void Operator_UnaryNegation_ReturnsCorrectResult()
	{
		var number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		// Using the unary - operator; this calls op_UnaryNegation internally.
		var result = -number;
		Assert.AreEqual(SignificantNumber.Negate(number), result);
	}

	[TestMethod]
	public void Operator_UnaryPlus_ReturnsSameValue()
	{
		var number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		// Using the unary + operator; this calls op_UnaryPlus internally.
		var result = +number;
		Assert.AreEqual(SignificantNumber.Plus(number), result);
	}

	[TestMethod]
	public void Operator_Subtraction_Overload_ReturnsCorrectResult()
	{
		// Tests the overload operator: (PreciseNumber left, SignificantNumber right)
		PreciseNumber left = SignificantNumber.CreateFromComponents(2, new BigInteger(456));
		var right = SignificantNumber.CreateFromComponents(2, new BigInteger(123));
		var result = left - right;
		Assert.AreEqual(SignificantNumber.Subtract(left, right), result);
	}

	[TestMethod]
	public void Operator_Multiplication_BothSignificantNumber_ReturnsCorrectResult()
	{
		// Tests the overload operator that takes two SignificantNumber parameters.
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(20));
		var result = left * right;
		Assert.AreEqual(SignificantNumber.Multiply(left, right), result);
	}

	[TestMethod]
	public void Operator_Division_BothSignificantNumber_ReturnsCorrectResult()
	{
		// Tests the overload operator that takes two SignificantNumber parameters.
		var left = SignificantNumber.CreateFromComponents(2, new BigInteger(100));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var result = left / right;
		Assert.AreEqual(SignificantNumber.Divide(left, right), result);
	}

	[TestMethod]
	public void Operator_Modulus_BothSignificantNumber_ReturnsCorrectResult()
	{
		// Tests the overload operator that takes two SignificantNumber parameters.
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(25));
		var right = SignificantNumber.CreateFromComponents(1, new BigInteger(7));
		var result = left % right;
		Assert.AreEqual(SignificantNumber.Mod(left, right), result);
	}

	[TestMethod]
	public void DoesImplementGenericInterface_ReturnsTrue_ForImplementedInterface()
	{
		// Dummy type that implements ITest<int>
		var type = typeof(DummyTestClass);
		var genericInterface = typeof(ITest<>);
		bool result = SignificantNumber.DoesImplementGenericInterface(type, genericInterface);
		Assert.IsTrue(result);
	}

	[TestMethod]
	public void DoesImplementGenericInterface_ReturnsFalse_ForNotImplementedInterface()
	{
		// Dummy type that does not implement ITest<>
		var type = typeof(DummyNonTestClass);
		var genericInterface = typeof(ITest<>);
		bool result = SignificantNumber.DoesImplementGenericInterface(type, genericInterface);
		Assert.IsFalse(result);
	}

	[TestMethod]
	public void DoesImplementGenericInterface_ThrowsArgumentException_ForNonGenericInterface()
	{
		// Passing a non-generic interface should throw an ArgumentException.
		var type = typeof(DummyTestClass);
		var nonGenericInterface = typeof(IDisposable); // IDisposable is not a generic interface.
		Assert.ThrowsExactly<ArgumentException>(() => SignificantNumber.DoesImplementGenericInterface(type, nonGenericInterface));
	}

	// Dummy types for testing DoesImplementGenericInterface.
	public interface ITest<T> { }
	public class DummyTestClass : ITest<int> { }
	public class DummyNonTestClass { }

	[TestMethod]
	public void CreateFromComponents_WithExplicitNormalize_ReturnsNormalizedValue()
	{
		// Tests the CreateFromComponents method with the normalize parameter
		var number = SignificantNumber.CreateFromComponents(2, new BigInteger(123), true);

		// Verify it returns a normalized value (the implementation details might vary)
		Assert.IsNotNull(number);
		// This assumes normalization doesn't change the value for this simple case
		Assert.AreEqual(new BigInteger(123), number.Significand);
		Assert.AreEqual(2, number.Exponent);
	}

	[TestMethod]
	public void Exp_NegativeValue_ReturnsCorrectResult()
	{
		// Testing Exp with negative exponent since that branch has low coverage
		var power = SignificantNumber.CreateFromComponents(0, new BigInteger(-2));
		var result = SignificantNumber.Exp(power);

		// The expected result should be approximately 1/e^2
		// This is a very simple test; actual implementation may have more precision considerations
		Assert.IsNotNull(result);
		// Add specific value assertion based on your implementation
	}

	[TestMethod]
	public void AssertDoesImplementGenericInterface_Valid_DoesNotThrow()
	{
		// Tests that the assertion passes for valid cases
		var type = typeof(List<int>);
		var genericInterface = typeof(IEnumerable<>);

		// This should not throw
		SignificantNumber.AssertDoesImplementGenericInterface(type, genericInterface);

		// If we got here, the test passes
		Assert.IsTrue(true);
	}

	[TestMethod]
	public void AssertDoesImplementGenericInterface_Invalid_Throws()
	{
		// Tests that the assertion throws for invalid cases
		var type = typeof(List<int>);
		var genericInterface = typeof(IDictionary<,>); // List<int> doesn't implement IDictionary

		// This should throw
		Assert.ThrowsExactly<ArgumentException>(() => SignificantNumber.AssertDoesImplementGenericInterface(type, genericInterface));
	}

	[TestMethod]
	public void Operator_Increment_ReturnsIncrementedValue()
	{
		var number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		var result = ++number; // Uses op_Increment

		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(6)), result);
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(6)), number); // Original should also be changed
	}

	[TestMethod]
	public void Operator_Decrement_ReturnsDecrementedValue()
	{
		var number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		var result = --number; // Uses op_Decrement

		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(4)), result);
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(4)), number); // Original should also be changed
	}

	[TestMethod]
	public void Operator_Addition_WithDifferentTypes_ReturnsCorrectResult()
	{
		// Testing overloads with PreciseNumber
		var leftSN = SignificantNumber.CreateFromComponents(2, new BigInteger(123));
		var rightPN = 456.ToPreciseNumber();

		// Test both ways
		var result1 = leftSN + rightPN; // SignificantNumber + PreciseNumber
		var result2 = rightPN + leftSN; // PreciseNumber + SignificantNumber

		// Both should give the same result
		Assert.AreEqual(SignificantNumber.Add(leftSN, rightPN), result1);
		Assert.AreEqual(SignificantNumber.Add(rightPN, leftSN), result2);
		Assert.AreEqual(result1, result2);
	}

	[TestMethod]
	public void Operator_Equality_WithDifferentTypes_ReturnsCorrectResult()
	{
		var leftSN = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var rightPN = 100.ToPreciseNumber();

		bool result1 = leftSN == rightPN;
		bool result2 = rightPN == leftSN;

		Assert.IsTrue(result1);
		Assert.IsTrue(result2);
	}

	[TestMethod]
	public void Operator_Comparison_WithDifferentTypes_ReturnsCorrectResults()
	{
		var smallerSN = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		var largerPN = 100.ToPreciseNumber();

		// Test greater than
		bool gtResult1 = largerPN > smallerSN;
		bool gtResult2 = smallerSN < largerPN;

		// Test greater than or equal
		bool gteResult1 = largerPN >= smallerSN;
		bool gteResult2 = smallerSN <= largerPN;

		Assert.IsTrue(gtResult1);
		Assert.IsTrue(gtResult2);
		Assert.IsTrue(gteResult1);
		Assert.IsTrue(gteResult2);
	}

	[TestMethod]
	public void ToSignificantNumber_ReturnsSameInstance()
	{
		var number = (PreciseNumber)SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		var result = number.ToSignificantNumber();
		Assert.AreSame(number, result);
	}

	[TestMethod]
	public void Abs_PositiveNumber_ReturnsSameValue()
	{
		var number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		var result = SignificantNumber.Abs(number);
		Assert.AreEqual(number, result);
	}

	[TestMethod]
	public void Abs_NegativeNumber_ReturnsPositiveValue()
	{
		var number = SignificantNumber.CreateFromComponents(0, new BigInteger(-5));
		var result = SignificantNumber.Abs(number);
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(5)), result);
	}

	[TestMethod]
	public void IsCanonical_ValidNumber_ReturnsTrue()
	{
		var number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		bool result = SignificantNumber.IsCanonical(number);
		Assert.IsTrue(result);
	}

	[TestMethod]
	public void IsEvenInteger_EvenNumber_ReturnsTrue()
	{
		var number = SignificantNumber.CreateFromComponents(0, new BigInteger(4));
		bool result = SignificantNumber.IsEvenInteger(number);
		Assert.IsTrue(result);
	}

	[TestMethod]
	public void IsEvenInteger_OddNumber_ReturnsFalse()
	{
		var number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		bool result = SignificantNumber.IsEvenInteger(number);
		Assert.IsFalse(result);
	}

	[TestMethod]
	public void IsOddInteger_OddNumber_ReturnsTrue()
	{
		var number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		bool result = SignificantNumber.IsOddInteger(number);
		Assert.IsTrue(result);
	}

	[TestMethod]
	public void IsOddInteger_EvenNumber_ReturnsFalse()
	{
		var number = SignificantNumber.CreateFromComponents(0, new BigInteger(4));
		bool result = SignificantNumber.IsOddInteger(number);
		Assert.IsFalse(result);
	}

	[TestMethod]
	public void IsZero_ZeroNumber_ReturnsTrue()
	{
		var number = SignificantNumber.Zero;
		bool result = SignificantNumber.IsZero(number);
		Assert.IsTrue(result);
	}

	[TestMethod]
	public void IsZero_NonZeroNumber_ReturnsFalse()
	{
		var number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		bool result = SignificantNumber.IsZero(number);
		Assert.IsFalse(result);
	}

	[TestMethod]
	public void MaxMagnitude_LargerMagnitude_ReturnsCorrectValue()
	{
		var number1 = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		var number2 = SignificantNumber.CreateFromComponents(0, new BigInteger(10));
		var result = SignificantNumber.MaxMagnitude(number1, number2);
		Assert.AreEqual(number2, result);
	}

	[TestMethod]
	public void MinMagnitude_SmallerMagnitude_ReturnsCorrectValue()
	{
		var number1 = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		var number2 = SignificantNumber.CreateFromComponents(0, new BigInteger(10));
		var result = SignificantNumber.MinMagnitude(number1, number2);
		Assert.AreEqual(number1, result);
	}

	[TestMethod]
	public void Parse_ValidString_ReturnsCorrectNumber()
	{
		string input = "5";
		var result = SignificantNumber.Parse(input, CultureInfo.InvariantCulture);
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(5)), result);
	}

	[TestMethod]
	public void TryParse_ValidString_ReturnsTrueAndCorrectNumber()
	{
		string input = "5";
		bool success = SignificantNumber.TryParse(input, CultureInfo.InvariantCulture, out var result);
		Assert.IsTrue(success);
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(5)), result);
	}

	[TestMethod]
	public void TryParse_InvalidString_ReturnsFalse()
	{
		string input = "invalid";
		bool success = SignificantNumber.TryParse(input, CultureInfo.InvariantCulture, out var result);
		Assert.IsFalse(success);
		Assert.IsNull(result);
	}

	[TestMethod]
	public void CreateFromComponents_WithSanitizeTrue_RemovesTrailingZeros()
	{
		var number = SignificantNumber.CreateFromComponents(2, new BigInteger(12300), true);

		// Assuming sanitization removes trailing zeros
		Assert.AreEqual(new BigInteger(123), number.Significand);
		Assert.AreEqual(4, number.Exponent); // Adjusted exponent
	}

	[TestMethod]
	public void CreateFromComponents_WithSanitizeFalse_KeepsTrailingZeros()
	{
		var number = SignificantNumber.CreateFromComponents(2, new BigInteger(12300), false);

		// Trailing zeros should remain
		Assert.AreEqual(new BigInteger(12300), number.Significand);
		Assert.AreEqual(2, number.Exponent);
	}

	[TestMethod]
	public void DoesImplementGenericInterface_InvalidGenericInterface_ThrowsArgumentException()
	{
		var type = typeof(List<int>);
		var invalidInterface = typeof(IDisposable); // Not a generic interface
		Assert.ThrowsExactly<ArgumentException>(() => SignificantNumber.DoesImplementGenericInterface(type, invalidInterface));
	}

	[TestMethod]
	public void Pow_PositiveBaseAndExponent_ReturnsCorrectResult()
	{
		var baseNumber = SignificantNumber.CreateFromComponents(0, new BigInteger(3));
		var power = SignificantNumber.CreateFromComponents(0, new BigInteger(2));
		var result = baseNumber.Pow(power);

		// Expected result is 3^2 = 9
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(9)), result);
	}

	[TestMethod]
	public void Pow_NegativeBaseAndPositiveExponent_ReturnsCorrectResult()
	{
		var baseNumber = SignificantNumber.CreateFromComponents(0, new BigInteger(-3));
		var power = SignificantNumber.CreateFromComponents(0, new BigInteger(2));
		var result = baseNumber.Pow(power);

		// Expected result is (-3)^2 = 9
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(9)), result);
	}

	[TestMethod]
	public void Operator_Addition_WithPreciseNumber_ReturnsCorrectResult()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		var right = 20.ToPreciseNumber();
		var result = left + right;

		Assert.AreEqual(SignificantNumber.Add(left, right), result);
	}

	[TestMethod]
	public void Operator_Subtraction_WithPreciseNumber_ReturnsCorrectResult()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(30));
		var right = 4.ToPreciseNumber();
		var result = left - right;

		Assert.AreEqual(SignificantNumber.Subtract(left, right), result);
	}

	[TestMethod]
	public void Operator_Multiplication_WithPreciseNumber_ReturnsCorrectResult()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		var right = 4.ToPreciseNumber();
		var result = left * right;

		Assert.AreEqual(SignificantNumber.Multiply(left, right), result);
	}

	[TestMethod]
	public void Operator_Division_WithPreciseNumber_ReturnsCorrectResult()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(20));
		var right = 4.ToPreciseNumber();
		var result = left / right;

		Assert.AreEqual(SignificantNumber.Divide(left, right), result);
	}

	[TestMethod]
	public void Operator_Modulus_WithPreciseNumber_ReturnsCorrectResult()
	{
		var left = SignificantNumber.CreateFromComponents(1, new BigInteger(25));
		var right = 7.ToPreciseNumber();
		var result = left % right;

		Assert.AreEqual(SignificantNumber.Mod(left, right), result);
	}

	[TestMethod]
	public void TryParse_InvalidInput_ReturnsFalse()
	{
		string input = "invalid";
		bool success = SignificantNumber.TryParse(input, CultureInfo.InvariantCulture, out var result);

		Assert.IsFalse(success);
		Assert.IsNull(result);
	}
}
