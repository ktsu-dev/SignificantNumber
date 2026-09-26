// Copyright (c) 2023-2026 ktsu-dev contributors

[assembly: Parallelize]

namespace SignificantNumber.Test;

using System.Globalization;
using System.Numerics;
using ktsu.PreciseNumber;
using ktsu.SignificantNumber;

[TestClass]
public class SignificantNumberTests
{
	[TestMethod]
	public void Add_TwoNumbers_ReturnsCorrectResult()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(2, new BigInteger(123));
		SignificantNumber right = SignificantNumber.CreateFromComponents(2, new BigInteger(456));
		SignificantNumber result = SignificantNumber.Add(left, right);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(2, new BigInteger(579)), result);
	}

	[TestMethod]
	public void Subtract_TwoNumbers_ReturnsCorrectResult()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(2, new BigInteger(456));
		SignificantNumber right = SignificantNumber.CreateFromComponents(2, new BigInteger(123));
		SignificantNumber result = SignificantNumber.Subtract(left, right);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(2, new BigInteger(333)), result);
	}

	[TestMethod]
	public void Multiply_TwoNumbers_ReturnsCorrectResult()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(20));
		SignificantNumber result = SignificantNumber.Multiply(left, right);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(2, new BigInteger(200)), result);
	}

	[TestMethod]
	public void Divide_TwoNumbers_ReturnsCorrectResult()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(2, new BigInteger(100));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		SignificantNumber result = SignificantNumber.Divide(left, right);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(1, new BigInteger(10)), result);
	}

	[TestMethod]
	public void Mod_TwoNumbers_ReturnsCorrectResult()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(25));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(7));
		SignificantNumber result = SignificantNumber.Mod(left, right);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(1, new BigInteger(4)), result);
	}

	[TestMethod]
	public void Increment_Number_ReturnsCorrectResult()
	{
		SignificantNumber value = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		SignificantNumber result = SignificantNumber.Increment(value);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(6)), result);
	}

	[TestMethod]
	public void Decrement_Number_ReturnsCorrectResult()
	{
		SignificantNumber value = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		SignificantNumber result = SignificantNumber.Decrement(value);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(4)), result);
	}

	[TestMethod]
	public void GreaterThan_TwoNumbers_ReturnsTrue()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		bool result = SignificantNumber.GreaterThan(left, right);

		Assert.IsTrue(result, "GreaterThan should return true when left (10) is greater than right (5)");
	}

	[TestMethod]
	public void LessThan_TwoNumbers_ReturnsTrue()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		bool result = SignificantNumber.LessThan(left, right);

		Assert.IsTrue(result, "LessThan should return true when left (5) is less than right (10)");
	}

	[TestMethod]
	public void Equal_TwoNumbers_ReturnsTrue()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		bool result = SignificantNumber.Equal(left, right);

		Assert.IsTrue(result, "Equal should return true when both numbers have the same value (10)");
	}

	[TestMethod]
	public void Pow_Number_ReturnsCorrectResult()
	{
		SignificantNumber baseNumber = SignificantNumber.CreateFromComponents(0, new BigInteger(2));
		SignificantNumber power = SignificantNumber.CreateFromComponents(0, new BigInteger(3));
		SignificantNumber result = baseNumber.Pow(power);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(8)), result);
	}

	[TestMethod]
	public void Exp_Number_ReturnsCorrectResult()
	{
		SignificantNumber power = SignificantNumber.CreateFromComponents(0, new BigInteger(1));
		SignificantNumber result = SignificantNumber.Exp(power);

		Assert.AreEqual(PreciseNumber.E, result);
	}

	[TestMethod]
	public void Negate_Number_ReturnsCorrectResult()
	{
		SignificantNumber value = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		SignificantNumber result = SignificantNumber.Negate(value);

		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(-5)), result);
	}

	[TestMethod]
	public void Plus_Number_ReturnsSameValue()
	{
		SignificantNumber value = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		SignificantNumber result = SignificantNumber.Plus(value);

		Assert.AreEqual(value, result);
	}

	[TestMethod]
	public void GreaterThanOrEqual_TwoNumbers_ReturnsTrue()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		bool result = SignificantNumber.GreaterThanOrEqual(left, right);

		Assert.IsTrue(result, "GreaterThanOrEqual should return true when both numbers are equal (10)");
	}

	[TestMethod]
	public void LessThanOrEqual_TwoNumbers_ReturnsTrue()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		bool result = SignificantNumber.LessThanOrEqual(left, right);

		Assert.IsTrue(result, "LessThanOrEqual should return true when left (5) is less than right (10)");
	}

	[TestMethod]
	public void NotEqual_TwoNumbers_ReturnsTrue()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		bool result = SignificantNumber.NotEqual(left, right);

		Assert.IsTrue(result, "NotEqual should return true when left (10) is not equal to right (5)");
	}

	[TestMethod]
	public void CompareTo_TwoNumbers_ReturnsCorrectComparison()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		int result = SignificantNumber.CompareTo(left, right);

		Assert.IsGreaterThan(0, result, "CompareTo should return a positive value when left (10) is greater than right (5)");
	}

	[TestMethod]
	public void Operator_Addition_ReturnsCorrectResult()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(2, new BigInteger(123));
		SignificantNumber right = SignificantNumber.CreateFromComponents(2, new BigInteger(456));
		SignificantNumber result = left + right;

		Assert.AreEqual(SignificantNumber.CreateFromComponents(2, new BigInteger(579)), result);
	}

	[TestMethod]
	public void Operator_Subtraction_ReturnsCorrectResult()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(2, new BigInteger(456));
		SignificantNumber right = SignificantNumber.CreateFromComponents(2, new BigInteger(123));
		SignificantNumber result = left - right;

		Assert.AreEqual(SignificantNumber.CreateFromComponents(2, new BigInteger(333)), result);
	}

	[TestMethod]
	public void Operator_Multiplication_ReturnsCorrectResult()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(20));
		SignificantNumber result = left * right;

		Assert.AreEqual(SignificantNumber.CreateFromComponents(2, new BigInteger(200)), result);
	}

	[TestMethod]
	public void Operator_Division_ReturnsCorrectResult()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(2, new BigInteger(100));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		SignificantNumber result = left / right;

		Assert.AreEqual(SignificantNumber.CreateFromComponents(1, new BigInteger(10)), result);
	}

	[TestMethod]
	public void Operator_Modulus_ReturnsCorrectResult()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(25));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(7));
		SignificantNumber result = left % right;

		Assert.AreEqual(SignificantNumber.CreateFromComponents(1, new BigInteger(4)), result);
	}

	[TestMethod]
	public void Operator_Equality_ReturnsTrue()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		bool result = left == right;

		Assert.IsTrue(result, "Equality operator should return true when both numbers have the same value (10)");
	}

	[TestMethod]
	public void Operator_Inequality_ReturnsTrue()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		bool result = left != right;

		Assert.IsTrue(result, "Inequality operator should return true when left (10) is not equal to right (5)");
	}

	[TestMethod]
	public void Operator_GreaterThan_ReturnsTrue()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		bool result = left > right;

		Assert.IsTrue(result, "Greater than operator should return true when left (10) is greater than right (5)");
	}

	[TestMethod]
	public void Operator_LessThan_ReturnsTrue()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		bool result = left < right;

		Assert.IsTrue(result, "Less than operator should return true when left (5) is less than right (10)");
	}

	[TestMethod]
	public void Operator_GreaterThanOrEqual_ReturnsTrue()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		bool result = left >= right;

		Assert.IsTrue(result, "Greater than or equal operator should return true when both numbers are equal (10)");
	}

	[TestMethod]
	public void Operator_LessThanOrEqual_ReturnsTrue()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		bool result = left <= right;

		Assert.IsTrue(result, "Less than or equal operator should return true when left (5) is less than right (10)");
	}

	[TestMethod]
	public void Pow_ZeroExponent_ReturnsOne()
	{
		SignificantNumber baseNumber = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		SignificantNumber power = SignificantNumber.CreateFromComponents(0, BigInteger.Zero);
		SignificantNumber result = baseNumber.Pow(power);
		// When exponent is zero, the result should be one
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(1)), result);
	}

	[TestMethod]
	public void Pow_BaseIsZero_ReturnsZero()
	{
		SignificantNumber baseNumber = SignificantNumber.CreateFromComponents(0, BigInteger.Zero);
		SignificantNumber power = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		SignificantNumber result = baseNumber.Pow(power);
		// When base is zero and power is nonzero, the result should be zero
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, BigInteger.Zero), result);
	}

	[TestMethod]
	public void Pow_BaseIsOne_ReturnsOne()
	{
		SignificantNumber baseNumber = SignificantNumber.CreateFromComponents(0, new BigInteger(1));
		SignificantNumber power = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		SignificantNumber result = baseNumber.Pow(power);
		// When base is one, the result should always be one
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(1)), result);
	}

	[TestMethod]
	public void Operator_UnaryNegation_ReturnsCorrectResult()
	{
		SignificantNumber number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		// Using the unary - operator; this calls op_UnaryNegation internally.
		SignificantNumber result = -number;
		Assert.AreEqual(SignificantNumber.Negate(number), result);
	}

	[TestMethod]
	public void Operator_UnaryPlus_ReturnsSameValue()
	{
		SignificantNumber number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		// Using the unary + operator; this calls op_UnaryPlus internally.
		SignificantNumber result = +number;
		Assert.AreEqual(SignificantNumber.Plus(number), result);
	}

	[TestMethod]
	public void Operator_Subtraction_Overload_ReturnsCorrectResult()
	{
		// Tests the overload operator: (PreciseNumber left, SignificantNumber right)
		PreciseNumber left = SignificantNumber.CreateFromComponents(2, new BigInteger(456));
		SignificantNumber right = SignificantNumber.CreateFromComponents(2, new BigInteger(123));
		SignificantNumber result = left - right;
		Assert.AreEqual(SignificantNumber.Subtract(left, right), result);
	}

	[TestMethod]
	public void Operator_Multiplication_BothSignificantNumber_ReturnsCorrectResult()
	{
		// Tests the overload operator that takes two SignificantNumber parameters.
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(20));
		SignificantNumber result = left * right;
		Assert.AreEqual(SignificantNumber.Multiply(left, right), result);
	}

	[TestMethod]
	public void Operator_Division_BothSignificantNumber_ReturnsCorrectResult()
	{
		// Tests the overload operator that takes two SignificantNumber parameters.
		SignificantNumber left = SignificantNumber.CreateFromComponents(2, new BigInteger(100));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		SignificantNumber result = left / right;
		Assert.AreEqual(SignificantNumber.Divide(left, right), result);
	}

	[TestMethod]
	public void Operator_Modulus_BothSignificantNumber_ReturnsCorrectResult()
	{
		// Tests the overload operator that takes two SignificantNumber parameters.
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(25));
		SignificantNumber right = SignificantNumber.CreateFromComponents(1, new BigInteger(7));
		SignificantNumber result = left % right;
		Assert.AreEqual(SignificantNumber.Mod(left, right), result);
	}

	[TestMethod]
	public void DoesImplementGenericInterface_ReturnsTrue_ForImplementedInterface()
	{
		// Dummy type that implements ITest<int>
		Type type = typeof(DummyTestClass);
		Type genericInterface = typeof(ITest<>);
		bool result = SignificantNumber.DoesImplementGenericInterface(type, genericInterface);
		Assert.IsTrue(result, "DoesImplementGenericInterface should return true for a type that implements the generic interface");
	}

	[TestMethod]
	public void DoesImplementGenericInterface_ReturnsFalse_ForNotImplementedInterface()
	{
		// Dummy type that does not implement ITest<>
		Type type = typeof(DummyNonTestClass);
		Type genericInterface = typeof(ITest<>);
		bool result = SignificantNumber.DoesImplementGenericInterface(type, genericInterface);
		Assert.IsFalse(result, "DoesImplementGenericInterface should return false for a type that does not implement the generic interface");
	}

	[TestMethod]
	public void DoesImplementGenericInterface_ThrowsArgumentException_ForNonGenericInterface()
	{
		// Passing a non-generic interface should throw an ArgumentException.
		Type type = typeof(DummyTestClass);
		Type nonGenericInterface = typeof(IDisposable); // IDisposable is not a generic interface.
		Assert.ThrowsExactly<ArgumentException>(() => SignificantNumber.DoesImplementGenericInterface(type, nonGenericInterface));
	}

	// Dummy types for testing DoesImplementGenericInterface.
	public interface ITest<T> { }
	public class DummyTestClass : ITest<int> { }
	public class DummyNonTestClass { }

	[TestMethod]
	public void CreateFromComponents_ReturnsNormalizedValue()
	{
		// Tests that CreateFromComponents keeps a significand with no trailing zeros as it is
		SignificantNumber number = SignificantNumber.CreateFromComponents(2, new BigInteger(123));

		Assert.AreEqual(new BigInteger(123), number.Significand);
		Assert.AreEqual(2, number.Exponent);
	}

	[TestMethod]
	public void Exp_NegativeValue_ReturnsCorrectResult()
	{
		// Testing Exp with negative exponent since that branch has low coverage
		SignificantNumber power = SignificantNumber.CreateFromComponents(0, new BigInteger(-2));
		SignificantNumber result = SignificantNumber.Exp(power);

		// The expected result is approximately 1/e^2, which is between zero and one
		Assert.IsTrue(SignificantNumber.IsPositive(result), "Exp of a negative power should be positive");
		Assert.IsLessThan(1.0, result.To<double>());
	}

	[TestMethod]
	public void AssertDoesImplementGenericInterface_Valid_DoesNotThrow()
	{
		// Tests that the assertion passes for valid cases
		Type type = typeof(List<int>);
		Type genericInterface = typeof(IEnumerable<>);

		// This should not throw - test passes if no exception is thrown
		SignificantNumber.AssertDoesImplementGenericInterface(type, genericInterface);
	}

	[TestMethod]
	public void AssertDoesImplementGenericInterface_Invalid_Throws()
	{
		// Tests that the assertion throws for invalid cases
		Type type = typeof(List<int>);
		Type genericInterface = typeof(IDictionary<,>); // List<int> doesn't implement IDictionary

		// This should throw
		Assert.ThrowsExactly<ArgumentException>(() => SignificantNumber.AssertDoesImplementGenericInterface(type, genericInterface));
	}

	[TestMethod]
	public void Operator_Increment_ReturnsIncrementedValue()
	{
		SignificantNumber number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		SignificantNumber result = ++number; // Uses op_Increment

		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(6)), result);
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(6)), number); // Original should also be changed
	}

	[TestMethod]
	public void Operator_Decrement_ReturnsDecrementedValue()
	{
		SignificantNumber number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		SignificantNumber result = --number; // Uses op_Decrement

		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(4)), result);
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(4)), number); // Original should also be changed
	}

	[TestMethod]
	public void Operator_Addition_WithDifferentTypes_ReturnsCorrectResult()
	{
		// Testing overloads with PreciseNumber
		SignificantNumber leftSN = SignificantNumber.CreateFromComponents(2, new BigInteger(123));
		PreciseNumber rightPN = 456.ToPreciseNumber();

		// Test both ways
		SignificantNumber result1 = leftSN + rightPN; // SignificantNumber + PreciseNumber
		SignificantNumber result2 = rightPN + leftSN; // PreciseNumber + SignificantNumber

		// Both should give the same result
		Assert.AreEqual(SignificantNumber.Add(leftSN, rightPN), result1);
		Assert.AreEqual(SignificantNumber.Add(rightPN, leftSN), result2);
		Assert.AreEqual(result1, result2);
	}

	[TestMethod]
	public void Operator_Equality_WithDifferentTypes_ReturnsCorrectResult()
	{
		SignificantNumber leftSN = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		PreciseNumber rightPN = 100.ToPreciseNumber();

		bool result1 = leftSN == rightPN;
		bool result2 = rightPN == leftSN;

		Assert.IsTrue(result1, "Equality operator should return true when SignificantNumber equals PreciseNumber");
		Assert.IsTrue(result2, "Equality operator should return true when PreciseNumber equals SignificantNumber");
	}

	[TestMethod]
	public void Operator_Comparison_WithDifferentTypes_ReturnsCorrectResults()
	{
		SignificantNumber smallerSN = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		PreciseNumber largerPN = 100.ToPreciseNumber();

		// Test greater than
		bool gtResult1 = largerPN > smallerSN;
		bool gtResult2 = smallerSN < largerPN;

		// Test greater than or equal
		bool gteResult1 = largerPN >= smallerSN;
		bool gteResult2 = smallerSN <= largerPN;

		Assert.IsTrue(gtResult1, "Greater than operator should return true when PreciseNumber (100) is greater than SignificantNumber (50)");
		Assert.IsTrue(gtResult2, "Less than operator should return true when SignificantNumber (50) is less than PreciseNumber (100)");
		Assert.IsTrue(gteResult1, "Greater than or equal operator should return true when PreciseNumber (100) is greater than SignificantNumber (50)");
		Assert.IsTrue(gteResult2, "Less than or equal operator should return true when SignificantNumber (50) is less than PreciseNumber (100)");
	}

	[TestMethod]
	public void ToSignificantNumber_FromPreciseNumber_KeepsValue()
	{
		PreciseNumber number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		SignificantNumber result = number.ToSignificantNumber();
		Assert.AreEqual<PreciseNumber>(number, result);
	}

	[TestMethod]
	public void ToSignificantNumber_FromSignificantNumber_ReturnsSameValue()
	{
		SignificantNumber number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		SignificantNumber result = number.ToSignificantNumber();
		Assert.AreEqual(number, result);
	}

	[TestMethod]
	public void Abs_PositiveNumber_ReturnsSameValue()
	{
		SignificantNumber number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		SignificantNumber result = SignificantNumber.Abs(number);
		Assert.AreEqual(number, result);
	}

	[TestMethod]
	public void Abs_NegativeNumber_ReturnsPositiveValue()
	{
		SignificantNumber number = SignificantNumber.CreateFromComponents(0, new BigInteger(-5));
		SignificantNumber result = SignificantNumber.Abs(number);
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(5)), result);
	}

	[TestMethod]
	public void IsCanonical_ValidNumber_ReturnsTrue()
	{
		SignificantNumber number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		bool result = SignificantNumber.IsCanonical(number);
		Assert.IsTrue(result, "IsCanonical should return true for a valid SignificantNumber");
	}

	[TestMethod]
	public void IsEvenInteger_EvenNumber_ReturnsTrue()
	{
		SignificantNumber number = SignificantNumber.CreateFromComponents(0, new BigInteger(4));
		bool result = SignificantNumber.IsEvenInteger(number);
		Assert.IsTrue(result, "IsEvenInteger should return true for an even number (4)");
	}

	[TestMethod]
	public void IsEvenInteger_OddNumber_ReturnsFalse()
	{
		SignificantNumber number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		bool result = SignificantNumber.IsEvenInteger(number);
		Assert.IsFalse(result, "IsEvenInteger should return false for an odd number (5)");
	}

	[TestMethod]
	public void IsOddInteger_OddNumber_ReturnsTrue()
	{
		SignificantNumber number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		bool result = SignificantNumber.IsOddInteger(number);
		Assert.IsTrue(result, "IsOddInteger should return true for an odd number (5)");
	}

	[TestMethod]
	public void IsOddInteger_EvenNumber_ReturnsFalse()
	{
		SignificantNumber number = SignificantNumber.CreateFromComponents(0, new BigInteger(4));
		bool result = SignificantNumber.IsOddInteger(number);
		Assert.IsFalse(result, "IsOddInteger should return false for an even number (4)");
	}

	[TestMethod]
	public void IsZero_ZeroNumber_ReturnsTrue()
	{
		SignificantNumber number = SignificantNumber.Zero;
		bool result = SignificantNumber.IsZero(number);
		Assert.IsTrue(result, "IsZero should return true for SignificantNumber.Zero");
	}

	[TestMethod]
	public void IsZero_NonZeroNumber_ReturnsFalse()
	{
		SignificantNumber number = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		bool result = SignificantNumber.IsZero(number);
		Assert.IsFalse(result, "IsZero should return false for a non-zero number (5)");
	}

	[TestMethod]
	public void MaxMagnitude_LargerMagnitude_ReturnsCorrectValue()
	{
		SignificantNumber number1 = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		SignificantNumber number2 = SignificantNumber.CreateFromComponents(0, new BigInteger(10));
		SignificantNumber result = SignificantNumber.MaxMagnitude(number1, number2);
		Assert.AreEqual(number2, result);
	}

	[TestMethod]
	public void MinMagnitude_SmallerMagnitude_ReturnsCorrectValue()
	{
		SignificantNumber number1 = SignificantNumber.CreateFromComponents(0, new BigInteger(5));
		SignificantNumber number2 = SignificantNumber.CreateFromComponents(0, new BigInteger(10));
		SignificantNumber result = SignificantNumber.MinMagnitude(number1, number2);
		Assert.AreEqual(number1, result);
	}

	[TestMethod]
	public void Parse_ValidString_ReturnsCorrectNumber()
	{
		string input = "5";
		SignificantNumber result = SignificantNumber.Parse(input, CultureInfo.InvariantCulture);
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(5)), result);
	}

	[TestMethod]
	public void TryParse_ValidString_ReturnsTrueAndCorrectNumber()
	{
		string input = "5";
		bool success = SignificantNumber.TryParse(input, CultureInfo.InvariantCulture, out SignificantNumber result);
		Assert.IsTrue(success, "TryParse should return true for a valid numeric string");
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(5)), result);
	}

	[TestMethod]
	public void TryParse_InvalidString_ReturnsFalse()
	{
		string input = "invalid";
		bool success = SignificantNumber.TryParse(input, CultureInfo.InvariantCulture, out SignificantNumber result);
		Assert.IsFalse(success, "TryParse should return false for an invalid string");
		Assert.AreEqual(SignificantNumber.Zero, result);
	}

	[TestMethod]
	public void CreateFromComponents_TrailingZeros_AreRemoved()
	{
		SignificantNumber number = SignificantNumber.CreateFromComponents(2, new BigInteger(12300));

		// Sanitization removes trailing zeros and moves them into the exponent
		Assert.AreEqual(new BigInteger(123), number.Significand);
		Assert.AreEqual(4, number.Exponent); // Adjusted exponent
	}

	[TestMethod]
	public void DoesImplementGenericInterface_InvalidGenericInterface_ThrowsArgumentException()
	{
		Type type = typeof(List<int>);
		Type invalidInterface = typeof(IDisposable); // Not a generic interface
		Assert.ThrowsExactly<ArgumentException>(() => SignificantNumber.DoesImplementGenericInterface(type, invalidInterface));
	}

	[TestMethod]
	public void Pow_PositiveBaseAndExponent_ReturnsCorrectResult()
	{
		SignificantNumber baseNumber = SignificantNumber.CreateFromComponents(0, new BigInteger(3));
		SignificantNumber power = SignificantNumber.CreateFromComponents(0, new BigInteger(2));
		SignificantNumber result = baseNumber.Pow(power);

		// Expected result is 3^2 = 9
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(9)), result);
	}

	[TestMethod]
	public void Pow_NegativeBaseAndPositiveExponent_ReturnsCorrectResult()
	{
		SignificantNumber baseNumber = SignificantNumber.CreateFromComponents(0, new BigInteger(-3));
		SignificantNumber power = SignificantNumber.CreateFromComponents(0, new BigInteger(2));
		SignificantNumber result = baseNumber.Pow(power);

		// Expected result is (-3)^2 = 9
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(9)), result);
	}

	[TestMethod]
	public void Pow_NegativeBaseAndOddExponent_ReturnsNegativeResult()
	{
		SignificantNumber baseNumber = SignificantNumber.CreateFromComponents(0, new BigInteger(-2));
		SignificantNumber power = SignificantNumber.CreateFromComponents(0, new BigInteger(3));
		SignificantNumber result = baseNumber.Pow(power);

		// Expected result is (-2)^3 = -8
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(-8)), result);
	}

	[TestMethod]
	public void Pow_IntegerExponent_MatchesRepeatedMultiplication()
	{
		SignificantNumber baseNumber = SignificantNumber.CreateFromComponents(-3, new BigInteger(1234));
		SignificantNumber two = SignificantNumber.CreateFromComponents(0, new BigInteger(2));
		SignificantNumber three = SignificantNumber.CreateFromComponents(0, new BigInteger(3));

		SignificantNumber squared = baseNumber.Pow(two);
		SignificantNumber cubed = baseNumber.Pow(three);

		// 1.234^2 = 1.522756 and 1.234^3 = 1.879080904, each kept to the base's 4 significant digits
		Assert.AreEqual(baseNumber * baseNumber, squared);
		Assert.AreEqual(1.523, squared.To<double>());
		Assert.AreEqual(4, squared.SignificantDigits);
		Assert.AreEqual(baseNumber * baseNumber * baseNumber, cubed);
		Assert.AreEqual(1.879, cubed.To<double>());
	}

	[TestMethod]
	public void Pow_NegativeBaseAndOddExponent_KeepsBasePrecision()
	{
		SignificantNumber baseNumber = SignificantNumber.CreateFromComponents(-1, new BigInteger(-25));
		SignificantNumber power = SignificantNumber.CreateFromComponents(0, new BigInteger(3));
		SignificantNumber result = baseNumber.Pow(power);

		// (-2.5)^3 = -15.625, kept to the base's 2 significant digits
		Assert.AreEqual(-16.0, result.To<double>());
	}

	[TestMethod]
	public void Pow_NegativeIntegerExponent_MatchesReciprocalOfPower()
	{
		SignificantNumber baseNumber = SignificantNumber.CreateFromComponents(-3, new BigInteger(1234));
		SignificantNumber power = SignificantNumber.CreateFromComponents(0, new BigInteger(-2));
		SignificantNumber result = baseNumber.Pow(power);

		// 1.234^-2 = 0.65670..., kept to the base's 4 significant digits
		Assert.AreEqual(0.6567, result.To<double>());
	}

	[TestMethod]
	public void Exp_IntegerExponent_KeepsMoreThanOneSignificantDigit()
	{
		SignificantNumber power = SignificantNumber.CreateFromComponents(0, new BigInteger(2));
		SignificantNumber result = SignificantNumber.Exp(power);

		Assert.IsGreaterThan(1, result.SignificantDigits);
		Assert.AreEqual(Math.Exp(2), result.To<double>(), 1e-12);
	}

	[TestMethod]
	public void Pow_NegativeOneAndOddExponent_ReturnsNegativeOne()
	{
		SignificantNumber baseNumber = SignificantNumber.CreateFromComponents(0, new BigInteger(-1));
		SignificantNumber power = SignificantNumber.CreateFromComponents(0, new BigInteger(3));
		SignificantNumber result = baseNumber.Pow(power);

		// Expected result is (-1)^3 = -1
		Assert.AreEqual(SignificantNumber.CreateFromComponents(0, new BigInteger(-1)), result);
	}

	[TestMethod]
	public void Pow_NegativeBaseAndFractionalExponent_ThrowsArgumentException()
	{
		SignificantNumber baseNumber = SignificantNumber.CreateFromComponents(0, new BigInteger(-2));
		SignificantNumber power = SignificantNumber.CreateFromComponents(-1, new BigInteger(5));

		// (-2)^0.5 has no real result
		Assert.ThrowsExactly<ArgumentException>(() => baseNumber.Pow(power));
	}

	[TestMethod]
	public void Pow_ZeroBaseAndNegativeExponent_ThrowsDivideByZeroException()
	{
		SignificantNumber baseNumber = SignificantNumber.CreateFromComponents(0, BigInteger.Zero);
		SignificantNumber power = SignificantNumber.CreateFromComponents(0, new BigInteger(-1));

		// 0^-1 is 1/0
		Assert.ThrowsExactly<DivideByZeroException>(() => baseNumber.Pow(power));
	}

	[TestMethod]
	public void Operator_Addition_WithPreciseNumber_ReturnsCorrectResult()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(10));
		PreciseNumber right = 20.ToPreciseNumber();
		SignificantNumber result = left + right;

		Assert.AreEqual(SignificantNumber.Add(left, right), result);
	}

	[TestMethod]
	public void Operator_Subtraction_WithPreciseNumber_ReturnsCorrectResult()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(30));
		PreciseNumber right = 4.ToPreciseNumber();
		SignificantNumber result = left - right;

		Assert.AreEqual(SignificantNumber.Subtract(left, right), result);
	}

	[TestMethod]
	public void Operator_Multiplication_WithPreciseNumber_ReturnsCorrectResult()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(5));
		PreciseNumber right = 4.ToPreciseNumber();
		SignificantNumber result = left * right;

		Assert.AreEqual(SignificantNumber.Multiply(left, right), result);
	}

	[TestMethod]
	public void Operator_Division_WithPreciseNumber_ReturnsCorrectResult()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(20));
		PreciseNumber right = 4.ToPreciseNumber();
		SignificantNumber result = left / right;

		Assert.AreEqual(SignificantNumber.Divide(left, right), result);
	}

	[TestMethod]
	public void Operator_Modulus_WithPreciseNumber_ReturnsCorrectResult()
	{
		SignificantNumber left = SignificantNumber.CreateFromComponents(1, new BigInteger(25));
		PreciseNumber right = 7.ToPreciseNumber();
		SignificantNumber result = left % right;

		Assert.AreEqual(SignificantNumber.Mod(left, right), result);
	}
}
