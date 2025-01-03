// Ignore Spelling: Commonized

[assembly: CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.ComVisible(false)]
namespace ktsu.SignificantNumber;

using System;
using System.Diagnostics;
using System.Numerics;
using ktsu.PreciseNumber;


/// <summary>
/// Represents a significant number.
/// </summary>
public record SignificantNumber
	: PreciseNumber
{

	/// <summary>
	/// Initializes a new instance of the <see cref="SignificantNumber"/> record.
	/// </summary>
	/// <param name="exponent">The exponent of the number.</param>
	/// <param name="significand">The significand of the number.</param>
	/// <param name="sanitize">If true, trailing zeros in the significand will be removed.</param>
	protected SignificantNumber(int exponent, BigInteger significand, bool sanitize)
		: base(exponent, significand, sanitize)
	{ }

	/// <summary>
	/// Initializes a new instance of the <see cref="SignificantNumber"/> record.
	/// </summary>
	/// <param name="exponent">The exponent of the number.</param>
	/// <param name="significand">The significand of the number.</param>
	protected SignificantNumber(int exponent, BigInteger significand)
		: this(exponent, significand, sanitize: true)
	{ }

	/// <summary>
	/// Subtracts one number from another.
	/// </summary>
	/// <param name="left">The number to subtract from.</param>
	/// <param name="right">The number to subtract.</param>
	/// <returns>The result of the subtraction.</returns>
	public static SignificantNumber Subtract(SignificantNumber left, PreciseNumber right)
	{
		int lowestDecimalDigits = LowestDecimalDigits(left, right);
		return PreciseNumber.Subtract(left, right).Round(lowestDecimalDigits).ToSignificantNumber();
	}

	/// <summary>
	/// Adds two numbers.
	/// </summary>
	/// <param name="left">The first number to add.</param>
	/// <param name="right">The second number to add.</param>
	/// <returns>The result of the addition.</returns>
	public static SignificantNumber Add(SignificantNumber left, PreciseNumber right)
	{
		int lowestDecimalDigits = LowestDecimalDigits(left, right);
		return PreciseNumber.Add(left, right).Round(lowestDecimalDigits).ToSignificantNumber();
	}

	/// <summary>
	/// Multiplies two numbers.
	/// </summary>
	/// <param name="left">The first number to multiply.</param>
	/// <param name="right">The second number to multiply.</param>
	/// <returns>The result of the multiplication.</returns>
	public static SignificantNumber Multiply(SignificantNumber left, PreciseNumber right)
	{
		int lowestSignificantDigits = LowestSignificantDigits(left, right);
		return PreciseNumber.Multiply(left, right)
			.ReduceSignificance(lowestSignificantDigits)
			.ToSignificantNumber();
	}

	/// <summary>
	/// Divides one number by another.
	/// </summary>
	/// <param name="left">The number to divide.</param>
	/// <param name="right">The number to divide by.</param>
	/// <returns>The result of the division.</returns>
	public static SignificantNumber Divide(SignificantNumber left, PreciseNumber right)
	{
		int lowestSignificantDigits = LowestSignificantDigits(left, right);
		return PreciseNumber.Divide(left, right)
			.ReduceSignificance(lowestSignificantDigits)
			.ToSignificantNumber();
	}

	/// <summary>
	/// Computes the modulus of two numbers.
	/// </summary>
	/// <param name="left">The number to divide.</param>
	/// <param name="right">The number to divide by.</param>
	/// <returns>The modulus of the two numbers.</returns>
	public static SignificantNumber Mod(SignificantNumber left, PreciseNumber right)
	{
		int lowestSignificantDigits = LowestSignificantDigits(left, right);
		return PreciseNumber.Mod(left, right)
			.ReduceSignificance(lowestSignificantDigits)
			.ToSignificantNumber();
	}

	public static SignificantNumber Increment(SignificantNumber value) =>
		PreciseNumber.Increment(value);

	public static SignificantNumber Decrement(SignificantNumber value) =>
		PreciseNumber.Decrement(value);

	/// <summary>
	/// Returns the unary plus of a number.
	/// </summary>
	/// <param name="value">The number.</param>
	/// <returns>The unary plus of the number.</returns>
	public static SignificantNumber UnaryPlus(SignificantNumber value) =>
		PreciseNumber.UnaryPlus(value);

	/// <summary>
	/// Determines whether one number is greater than another.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><c>true</c> if the first number is greater than the second; otherwise, <c>false</c>.</returns>
	public static bool GreaterThan(SignificantNumber left, PreciseNumber right)
	{
		var (commonLeft, commonRight) = MakeCommonized(left, right);
		AssertExponentsMatch(commonLeft, commonRight);
		return commonLeft.Significand > commonRight.Significand;
	}

	/// <summary>
	/// Determines whether one number is greater than or equal to another.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><c>true</c> if the first number is greater than or equal to the second; otherwise, <c>false</c>.</returns>
	public static bool GreaterThanOrEqual(SignificantNumber left, PreciseNumber right)
	{
		var (commonLeft, commonRight) = MakeCommonized(left, right);
		AssertExponentsMatch(commonLeft, commonRight);
		return commonLeft.Significand >= commonRight.Significand;
	}

	/// <summary>
	/// Determines whether one number is less than another.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><c>true</c> if the first number is less than the second; otherwise, <c>false</c>.</returns>
	public static bool LessThan(SignificantNumber left, PreciseNumber right)
	{
		var (commonLeft, commonRight) = MakeCommonized(left, right);
		AssertExponentsMatch(commonLeft, commonRight);
		return commonLeft.Significand < commonRight.Significand;
	}

	/// <summary>
	/// Determines whether one number is less than or equal to another.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><c>true</c> if the first number is less than or equal to the second; otherwise, <c>false</c>.</returns>
	public static bool LessThanOrEqual(SignificantNumber left, PreciseNumber right)
	{
		var (commonLeft, commonRight) = MakeCommonized(left, right);
		AssertExponentsMatch(commonLeft, commonRight);
		return commonLeft.Significand <= commonRight.Significand;
	}

	/// <summary>
	/// Determines whether two numbers are equal.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><c>true</c> if the two numbers are equal; otherwise, <c>false</c>.</returns>
	public static bool Equal(SignificantNumber left, PreciseNumber right)
	{
		int decimalDigits = LowestDecimalDigits(left, right);
		int significantDigits = LowestSignificantDigits(left, right);
		var (commonLeft, commonRight) = MakeCommonized(left, right);
		AssertExponentsMatch(commonLeft, commonRight);
		var leftSignificant = commonLeft.Round(decimalDigits).ReduceSignificance();
		var rightSignificant = commonRight.Round(decimalDigits).ReduceSignificance();
		var (commonLeftSignificant, commonRightSignificant) = MakeCommonized(leftSignificant, rightSignificant);
		AssertExponentsMatch(commonLeftSignificant, commonRightSignificant);

		return commonLeftSignificant.Significand == commonRightSignificant.Significand;



		var (commonLeft, commonRight) = MakeCommonized(left, right);
		AssertExponentsMatch(commonLeft, commonRight);
		return commonLeft.Significand == commonRight.Significand;
	}

	/// <summary>
	/// Determines whether two numbers are not equal.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><c>true</c> if the two numbers are not equal; otherwise, <c>false</c>.</returns>
	public static bool NotEqual(SignificantNumber left, PreciseNumber right)
	{
		var (commonLeft, commonRight) = MakeCommonized(left, right);
		AssertExponentsMatch(commonLeft, commonRight);
		return commonLeft.Significand != commonRight.Significand;
	}

	/// <inheritdoc/>
	public static SignificantNumber operator -(SignificantNumber value) =>
		Negate(value);

	/// <inheritdoc/>
	public static SignificantNumber operator -(SignificantNumber left, PreciseNumber right) =>
		Subtract(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator *(SignificantNumber left, PreciseNumber right) =>
		Multiply(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator /(SignificantNumber left, PreciseNumber right) =>
		Divide(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator +(SignificantNumber value) =>
		UnaryPlus(value);

	/// <inheritdoc/>
	public static SignificantNumber operator +(SignificantNumber left, PreciseNumber right) =>
		Add(left, right);

	/// <inheritdoc/>
	public static bool operator >(SignificantNumber left, PreciseNumber right) =>
		GreaterThan(left, right);

	/// <inheritdoc/>
	public static bool operator <(SignificantNumber left, PreciseNumber right) =>
		LessThan(left, right);

	/// <inheritdoc/>
	public static bool operator >=(SignificantNumber left, PreciseNumber right) =>
		GreaterThanOrEqual(left, right);

	/// <inheritdoc/>
	public static bool operator <=(SignificantNumber left, PreciseNumber right) =>
		LessThanOrEqual(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator %(SignificantNumber left, PreciseNumber right) =>
		Mod(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator --(SignificantNumber value) =>
		Decrement(value);

	/// <inheritdoc/>
	public static SignificantNumber operator ++(SignificantNumber value) =>
		Increment(value);

	/// <summary>
	/// Asserts that a type implements a specified generic interface.
	/// </summary>
	/// <param name="type">The type to check.</param>
	/// <param name="genericInterface">The generic interface to check for.</param>
	/// <exception cref="ArgumentException">Thrown when the specified type does not implement the generic interface.</exception>
	internal static void AssertDoesImplementGenericInterface(Type type, Type genericInterface) =>
		Debug.Assert(DoesImplementGenericInterface(type, genericInterface), $"{type.Name} does not implement {genericInterface.Name}");

	/// <summary>
	/// Determines whether a type implements a specified generic interface.
	/// </summary>
	/// <param name="type">The type to check.</param>
	/// <param name="genericInterface">The generic interface to check for.</param>
	/// <returns><c>true</c> if the type implements the generic interface; otherwise, <c>false</c>.</returns>
	/// <exception cref="ArgumentException">Thrown when the specified type is not a valid generic interface.</exception>
	internal static bool DoesImplementGenericInterface(Type type, Type genericInterface)
	{
		bool genericInterfaceIsValid = genericInterface.IsInterface && genericInterface.IsGenericType;

		return genericInterfaceIsValid
			? Array.Exists(type.GetInterfaces(), x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterface)
			: throw new ArgumentException($"{genericInterface.Name} is not a generic interface");
	}

	/// <summary>
	/// Returns the result of raising the current significant number to the specified power.
	/// </summary>
	/// <param name="power">The power to raise the significant number to.</param>
	/// <returns>A new instance of <see cref="SignificantNumber"/> that is the result of raising the current instance to the specified power.</returns>
	public SignificantNumber Pow(SignificantNumber power)
	{
		if (power == Zero)
		{
			return One;
		}
		else if (this == Zero)
		{
			return Zero;
		}
		else if (this == One)
		{
			return One;
		}

		int significantDigits = LowestSignificantDigits(this, power);

		// Use logarithm and exponential to support decimal powers
		double logValue = Math.Log(To<double>());
		return Math.Exp(logValue * power.To<double>()).ToSignificantNumber().ReduceSignificance(significantDigits);
	}

	/// <summary>
	/// Returns the result of raising e to the specified power.
	/// </summary>
	/// <param name="power">The power to raise e to.</param>
	/// <returns>A new instance of <see cref="SignificantNumber"/> that is the result of raising e to the specified power.</returns>
	public static SignificantNumber Exp(SignificantNumber power)
	{
		if (power == Zero)
		{
			return One;
		}
		else if (power == One)
		{
			return E;
		}

		int significantDigits = LowestSignificantDigits(E, power);

		return Math.Exp(power.To<double>()).ToSignificantNumber().ReduceSignificance(significantDigits);
	}
}
