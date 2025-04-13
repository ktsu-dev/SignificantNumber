[assembly: CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.ComVisible(false)]
namespace ktsu.SignificantNumber;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

using ktsu.PreciseNumber;

/// <summary>
/// Represents a significant number.
/// </summary>
[DebuggerDisplay("{Significand}e{Exponent}")]
public record SignificantNumber
	: PreciseNumber, INumber<SignificantNumber>
{
	/// <summary>
	/// Gets the significant number representing one.
	/// </summary>
	public static new SignificantNumber One => PreciseNumber.One.ToSignificantNumber();

	/// <summary>
	/// Gets the significant number representing zero.
	/// </summary>
	public static new SignificantNumber Zero => PreciseNumber.Zero.ToSignificantNumber();

	/// <summary>
	/// Gets the additive identity for significant numbers, which is zero.
	/// </summary>
	public static new SignificantNumber AdditiveIdentity => Zero;

	/// <summary>
	/// Gets the multiplicative identity for significant numbers, which is one.
	/// </summary>
	public static new SignificantNumber MultiplicativeIdentity => One;

	/// <summary>
	/// Initializes a new instance of the <see cref="SignificantNumber"/> record using a <see cref="PreciseNumber"/> value.
	/// </summary>
	/// <param name="value">The <see cref="PreciseNumber"/> value to initialize the <see cref="SignificantNumber"/> with.</param>
	public SignificantNumber(PreciseNumber value) : base(value) { }

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

	internal static SignificantNumber CreateFromComponents(int exponent, BigInteger significand) =>
		new(exponent, significand);

	internal static SignificantNumber CreateFromComponents(int exponent, BigInteger significand, bool sanitize) =>
		new(exponent, significand, sanitize);

	/// <summary>
	/// Subtracts one number from another.
	/// </summary>
	/// <param name="left">The number to subtract from.</param>
	/// <param name="right">The number to subtract.</param>
	/// <returns>The result of the subtraction.</returns>
	public static new SignificantNumber Subtract(PreciseNumber left, PreciseNumber right)
	{
		int lowestDecimalDigits = LowestDecimalDigits(left, right);
		return PreciseNumber.Subtract(left, right)
			.Round(lowestDecimalDigits)
			.ToSignificantNumber();
	}

	/// <summary>
	/// Adds two numbers.
	/// </summary>
	/// <param name="left">The first number to add.</param>
	/// <param name="right">The second number to add.</param>
	/// <returns>The result of the addition.</returns>
	public static new SignificantNumber Add(PreciseNumber left, PreciseNumber right)
	{
		int lowestDecimalDigits = LowestDecimalDigits(left, right);
		return PreciseNumber.Add(left, right)
			.Round(lowestDecimalDigits)
			.ToSignificantNumber();
	}

	/// <summary>
	/// Multiplies two numbers.
	/// </summary>
	/// <param name="left">The first number to multiply.</param>
	/// <param name="right">The second number to multiply.</param>
	/// <returns>The result of the multiplication.</returns>
	public static new SignificantNumber Multiply(PreciseNumber left, PreciseNumber right)
	{
		int lowestSignificantDigits = LowestSignificantDigits(left, right);
		return PreciseNumber.Multiply(left, right)
			.ToSignificantNumber(lowestSignificantDigits);
	}

	/// <summary>
	/// Divides one number by another.
	/// </summary>

	/// <param name="left">The number to divide.</param>
	/// <param name="right">The number to divide by.</param>
	/// <returns>The result of the division.</returns>
	public static new SignificantNumber Divide(PreciseNumber left, PreciseNumber right)
	{
		int lowestSignificantDigits = LowestSignificantDigits(left, right);
		return PreciseNumber.Divide(left, right)
			.ToSignificantNumber(lowestSignificantDigits);
	}

	/// <summary>
	/// Computes the modulus of two numbers.
	/// </summary>
	/// <param name="left">The number to divide.</param>
	/// <param name="right">The number to divide by.</param>
	/// <returns>The modulus of the two numbers.</returns>
	public static new SignificantNumber Mod(PreciseNumber left, PreciseNumber right)
	{
		int lowestSignificantDigits = LowestSignificantDigits(left, right);
		return PreciseNumber.Mod(left, right)
			.ToSignificantNumber(lowestSignificantDigits);
	}

	/// <summary>
	/// Increments the specified significant number by one.
	/// </summary>
	/// <param name="value">The significant number to increment.</param>
	/// <returns>A new instance of <see cref="SignificantNumber"/> representing the incremented value.</returns>
	public static SignificantNumber Increment(SignificantNumber value) =>
		PreciseNumber.Increment(value)
		.ToSignificantNumber();

	/// <summary>
	/// Decrements the specified significant number by one.
	/// </summary>
	/// <param name="value">The significant number to decrement.</param>
	/// <returns>A new instance of <see cref="SignificantNumber"/> representing the decremented value.</returns>
	public static SignificantNumber Decrement(SignificantNumber value) =>
		PreciseNumber.Decrement(value)
		.ToSignificantNumber();

	/// <summary>
	/// Returns the unary plus of a number.
	/// </summary>
	/// <param name="value">The number.</param>
	/// <returns>The unary plus of the number.</returns>
	public static SignificantNumber Plus(SignificantNumber value) =>
		PreciseNumber.Plus(value)
		.ToSignificantNumber();

	/// <summary>
	/// Negates the specified significant number.
	/// </summary>
	/// <param name="value">The significant number to negate.</param>
	/// <returns>A new instance of <see cref="SignificantNumber"/> representing the negated value.</returns>
	public static SignificantNumber Negate(SignificantNumber value) =>
		PreciseNumber.Negate(value)
		.ToSignificantNumber();

	/// <summary>
	/// Determines whether one number is greater than another.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><c>true</c> if the first number is greater than the second; otherwise, <c>false</c>.</returns>
	public static new bool GreaterThan(PreciseNumber left, PreciseNumber right)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		return left.CompareTo(right) > 0;
	}

	/// <summary>
	/// Determines whether one number is greater than or equal to another.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><c>true</c> if the first number is greater than or equal to the second; otherwise, <c>false</c>.</returns>
	public static new bool GreaterThanOrEqual(PreciseNumber left, PreciseNumber right)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		return left.CompareTo(right) >= 0;
	}

	/// <summary>
	/// Determines whether one number is less than another.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><c>true</c> if the first number is less than the second; otherwise, <c>false</c>.</returns>
	public static new bool LessThan(PreciseNumber left, PreciseNumber right)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		return left.CompareTo(right) < 0;
	}

	/// <summary>
	/// Determines whether one number is less than or equal to another.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><c>true</c> if the first number is less than or equal to the second; otherwise, <c>false</c>.</returns>
	public static new bool LessThanOrEqual(PreciseNumber left, PreciseNumber right)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		return left.CompareTo(right) <= 0;
	}

	/// <summary>
	/// Determines whether two numbers are equal.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><c>true</c> if the two numbers are equal; otherwise, <c>false</c>.</returns>
	public static new bool Equal(PreciseNumber left, PreciseNumber right)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		return left.CompareTo(right) == 0;
	}

	/// <summary>
	/// Determines whether two numbers are not equal.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><c>true</c> if the two numbers are not equal; otherwise, <c>false</c>.</returns>
	public static new bool NotEqual(PreciseNumber left, PreciseNumber right)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		return left.CompareTo(right) != 0;
	}

	/// <summary>
	/// Compares two numbers and returns an integer that indicates their relative position in the sort order.
	/// </summary>
	/// <param name="left">The first number to compare.</param>
	/// <param name="right">The second number to compare.</param>
	/// <returns>
	/// A signed integer that indicates the relative values of <paramref name="left"/> and <paramref name="right"/>:
	/// <list type="bullet">
	/// <item>
	/// <description>Less than zero: <paramref name="left"/> is less than <paramref name="right"/>.</description>
	/// </item>
	/// <item>
	/// <description>Zero: <paramref name="left"/> is equal to <paramref name="right"/>.</description>
	/// </item>
	/// <item>
	/// <description>Greater than zero: <paramref name="left"/> is greater than <paramref name="right"/>.</description>
	/// </item>
	/// </list>
	/// </returns>
	/// <exception cref="ArgumentNullException">
	/// Thrown when <paramref name="left"/> or <paramref name="right"/> is <c>null</c>.
	/// </exception>
	public static int CompareTo(PreciseNumber left, PreciseNumber right)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		int lowestSignificantDigits = LowestSignificantDigits(left, right);
		return left.ReduceSignificance(lowestSignificantDigits).CompareTo(right.ReduceSignificance(lowestSignificantDigits));
	}

	/// <inheritdoc/>
	public static SignificantNumber operator -(SignificantNumber value) =>
		Negate(value);

	/// <inheritdoc/>
	public static SignificantNumber operator -(SignificantNumber left, PreciseNumber right) =>
		Subtract(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator -(PreciseNumber left, SignificantNumber right) =>
		Subtract(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator -(SignificantNumber left, SignificantNumber right) =>
		Subtract(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator *(SignificantNumber left, PreciseNumber right) =>
		Multiply(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator *(PreciseNumber left, SignificantNumber right) =>
		Multiply(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator *(SignificantNumber left, SignificantNumber right) =>
		Multiply(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator /(SignificantNumber left, PreciseNumber right) =>
		Divide(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator /(PreciseNumber left, SignificantNumber right) =>
		Divide(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator /(SignificantNumber left, SignificantNumber right) =>
		Divide(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator +(SignificantNumber value) =>
		Plus(value);

	/// <inheritdoc/>
	public static SignificantNumber operator +(SignificantNumber left, PreciseNumber right) =>
		Add(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator +(PreciseNumber left, SignificantNumber right) =>
		Add(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator +(SignificantNumber left, SignificantNumber right) =>
		Add(left, right);

	/// <inheritdoc/>
	public static bool operator ==(SignificantNumber left, PreciseNumber right) =>
		Equal(left, right);

	/// <inheritdoc/>
	public static bool operator ==(PreciseNumber left, SignificantNumber right) =>
		Equal(left, right);

	/// <inheritdoc/>
	public static bool operator !=(SignificantNumber left, PreciseNumber right) =>
		NotEqual(left, right);

	/// <inheritdoc/>
	public static bool operator !=(PreciseNumber left, SignificantNumber right) =>
		NotEqual(left, right);

	/// <inheritdoc/>
	public static bool operator >(SignificantNumber left, PreciseNumber right) =>
		GreaterThan(left, right);

	/// <inheritdoc/>
	public static bool operator >(PreciseNumber left, SignificantNumber right) =>
		GreaterThan(left, right);

	/// <inheritdoc/>
	public static bool operator >(SignificantNumber left, SignificantNumber right) =>
		GreaterThan(left, right);

	/// <inheritdoc/>
	public static bool operator <(SignificantNumber left, PreciseNumber right) =>
		LessThan(left, right);

	/// <inheritdoc/>
	public static bool operator <(PreciseNumber left, SignificantNumber right) =>
		LessThan(left, right);

	/// <inheritdoc/>
	public static bool operator <(SignificantNumber left, SignificantNumber right) =>
		LessThan(left, right);

	/// <inheritdoc/>
	public static bool operator >=(SignificantNumber left, PreciseNumber right) =>
		GreaterThanOrEqual(left, right);

	/// <inheritdoc/>
	public static bool operator >=(PreciseNumber left, SignificantNumber right) =>
		GreaterThanOrEqual(left, right);

	/// <inheritdoc/>
	public static bool operator >=(SignificantNumber left, SignificantNumber right) =>
		GreaterThanOrEqual(left, right);

	/// <inheritdoc/>
	public static bool operator <=(SignificantNumber left, PreciseNumber right) =>
		LessThanOrEqual(left, right);

	/// <inheritdoc/>
	public static bool operator <=(PreciseNumber left, SignificantNumber right) =>
		LessThanOrEqual(left, right);

	/// <inheritdoc/>
	public static bool operator <=(SignificantNumber left, SignificantNumber right) =>
		LessThanOrEqual(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator %(SignificantNumber left, PreciseNumber right) =>
		Mod(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator %(PreciseNumber left, SignificantNumber right) =>
		Mod(left, right);

	/// <inheritdoc/>
	public static SignificantNumber operator %(SignificantNumber left, SignificantNumber right) =>
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
	internal static void AssertDoesImplementGenericInterface(Type type, Type genericInterface)
	{
		if (!DoesImplementGenericInterface(type, genericInterface))
		{
			throw new ArgumentException($"{type.Name} does not implement {genericInterface.Name}", nameof(type));
		}
	}

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
	public new SignificantNumber Pow(PreciseNumber power)
	{
		ArgumentNullException.ThrowIfNull(power);

		if (Equal(power, Zero))
		{
			return One;
		}
		else if (Equal(this, Zero))
		{
			return Zero;
		}
		else if (Equal(this, One))
		{
			return One;
		}

		int significantDigits = LowestSignificantDigits(this, power);

		// Use logarithm and exponential to support decimal powers
		double logValue = Math.Log(Math.Abs(To<double>()));
		return Math.Exp(logValue * power.To<double>()).ToSignificantNumber(significantDigits);
	}

	/// <summary>
	/// Returns the result of raising e to the specified power.
	/// </summary>
	/// <param name="power">The power to raise e to.</param>
	/// <returns>A new instance of <see cref="SignificantNumber"/> that is the result of raising e to the specified power.</returns>
	public static new SignificantNumber Exp(PreciseNumber power)
	{
		ArgumentNullException.ThrowIfNull(power);

		if (Equal(power, Zero))
		{
			return One;
		}
		else if (Equal(power, One))
		{
			return E.ToSignificantNumber();
		}

		int significantDigits = LowestSignificantDigits(E, power);

		return Math.Exp(power.To<double>())
			.ToSignificantNumber(significantDigits);
	}

	/// <summary>
	/// Compares the current instance with another <see cref="SignificantNumber"/> and returns an integer that indicates their relative position in the sort order.
	/// </summary>
	/// <param name="other">The <see cref="SignificantNumber"/> to compare with the current instance.</param>
	/// <returns>
	/// A signed integer that indicates the relative values of the current instance and <paramref name="other"/>:
	/// <list type="bullet">
	/// <item>
	/// <description>Less than zero: The current instance is less than <paramref name="other"/>.</description>
	/// </item>
	/// <item>
	/// <description>Zero: The current instance is equal to <paramref name="other"/>.</description>
	/// </item>
	/// <item>
	/// <description>Greater than zero: The current instance is greater than <paramref name="other"/>.</description>
	/// </item>
	/// </list>
	/// </returns>
	public int CompareTo(SignificantNumber? other) =>
	other is null ? 1 : CompareTo(this, other);

	/// <summary>
	/// Returns the absolute value of the specified <see cref="SignificantNumber"/>.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to compute the absolute value for.</param>
	/// <returns>A new <see cref="SignificantNumber"/> representing the absolute value of <paramref name="value"/>.</returns>
	public static SignificantNumber Abs(SignificantNumber value) =>
		PreciseNumber.Abs(value).ToSignificantNumber();

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is canonical.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> is canonical; otherwise, <c>false</c>.</returns>
	public static bool IsCanonical(SignificantNumber value) =>
		PreciseNumber.IsCanonical(value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is a complex number.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> is a complex number; otherwise, <c>false</c>.</returns>
	public static bool IsComplexNumber(SignificantNumber value) =>
		PreciseNumber.IsComplexNumber(value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is an even integer.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> is an even integer; otherwise, <c>false</c>.</returns>
	public static bool IsEvenInteger(SignificantNumber value) =>
		PreciseNumber.IsEvenInteger(value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is finite.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> is finite; otherwise, <c>false</c>.</returns>
	public static bool IsFinite(SignificantNumber value) =>
		PreciseNumber.IsFinite(value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is an imaginary number.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> is an imaginary number; otherwise, <c>false</c>.</returns>
	public static bool IsImaginaryNumber(SignificantNumber value) =>
		PreciseNumber.IsImaginaryNumber(value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> represents infinity.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> represents infinity; otherwise, <c>false</c>.</returns>
	public static bool IsInfinity(SignificantNumber value) =>
		PreciseNumber.IsInfinity(value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is an integer.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> is an integer; otherwise, <c>false</c>.</returns>
	public static bool IsInteger(SignificantNumber value) =>
		PreciseNumber.IsInteger(value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is not a number (NaN).
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> is NaN; otherwise, <c>false</c>.</returns>
	public static bool IsNaN(SignificantNumber value) =>
		PreciseNumber.IsNaN(value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is negative.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> is negative; otherwise, <c>false</c>.</returns>
	public static bool IsNegative(SignificantNumber value) =>
		PreciseNumber.IsNegative(value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> represents negative infinity.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> represents negative infinity; otherwise, <c>false</c>.</returns>
	public static bool IsNegativeInfinity(SignificantNumber value) =>
		PreciseNumber.IsNegativeInfinity(value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is a normal number.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> is a normal number; otherwise, <c>false</c>.</returns>
	public static bool IsNormal(SignificantNumber value) =>
		PreciseNumber.IsNormal(value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is an odd integer.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> is an odd integer; otherwise, <c>false</c>.</returns>
	public static bool IsOddInteger(SignificantNumber value) =>
		PreciseNumber.IsOddInteger(value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is positive.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> is positive; otherwise, <c>false</c>.</returns>
	public static bool IsPositive(SignificantNumber value) =>
		PreciseNumber.IsPositive(value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> represents positive infinity.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> represents positive infinity; otherwise, <c>false</c>.</returns>
	public static bool IsPositiveInfinity(SignificantNumber value) =>
		PreciseNumber.IsPositiveInfinity(value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is a real number.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> is a real number; otherwise, <c>false</c>.</returns>
	public static bool IsRealNumber(SignificantNumber value) =>
		PreciseNumber.IsRealNumber(value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is subnormal.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> is subnormal; otherwise, <c>false</c>.</returns>
	public static bool IsSubnormal(SignificantNumber value) =>
		PreciseNumber.IsSubnormal(value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is zero.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><c>true</c> if the <paramref name="value"/> is zero; otherwise, <c>false</c>.</returns>
	public static bool IsZero(SignificantNumber value) =>
		PreciseNumber.IsZero(value);

	/// <summary>
	/// Returns the larger magnitude of two <see cref="SignificantNumber"/> instances.
	/// </summary>
	/// <param name="x">The first <see cref="SignificantNumber"/> to compare.</param>
	/// <param name="y">The second <see cref="SignificantNumber"/> to compare.</param>
	/// <returns>The <see cref="SignificantNumber"/> with the larger magnitude.</returns>
	public static SignificantNumber MaxMagnitude(SignificantNumber x, SignificantNumber y) =>
		PreciseNumber.MaxMagnitude(x, y).ToSignificantNumber();

	/// <summary>
	/// Returns the larger magnitude of two <see cref="SignificantNumber"/> instances, or the first one if both have the same magnitude.
	/// </summary>
	/// <param name="x">The first <see cref="SignificantNumber"/> to compare.</param>
	/// <param name="y">The second <see cref="SignificantNumber"/> to compare.</param>
	/// <returns>The <see cref="SignificantNumber"/> with the larger magnitude, or <paramref name="x"/> if both have the same magnitude.</returns>
	public static SignificantNumber MaxMagnitudeNumber(SignificantNumber x, SignificantNumber y) =>
		PreciseNumber.MaxMagnitudeNumber(x, y).ToSignificantNumber();

	/// <summary>
	/// Returns the smaller magnitude of two <see cref="SignificantNumber"/> instances.
	/// </summary>
	/// <param name="x">The first <see cref="SignificantNumber"/> to compare.</param>
	/// <param name="y">The second <see cref="SignificantNumber"/> to compare.</param>
	/// <returns>The <see cref="SignificantNumber"/> with the smaller magnitude.</returns>
	public static SignificantNumber MinMagnitude(SignificantNumber x, SignificantNumber y) =>
		PreciseNumber.MinMagnitude(x, y).ToSignificantNumber();

	/// <summary>
	/// Returns the smaller magnitude of two <see cref="SignificantNumber"/> instances, or the first one if both have the same magnitude.
	/// </summary>
	/// <param name="x">The first <see cref="SignificantNumber"/> to compare.</param>
	/// <param name="y">The second <see cref="SignificantNumber"/> to compare.</param>
	/// <returns>The <see cref="SignificantNumber"/> with the smaller magnitude, or <paramref name="x"/> if both have the same magnitude.</returns>
	public static SignificantNumber MinMagnitudeNumber(SignificantNumber x, SignificantNumber y) =>
		PreciseNumber.MinMagnitudeNumber(x, y).ToSignificantNumber();

	/// <summary>
	/// Parses a span of characters into a <see cref="SignificantNumber"/> using the specified style and format provider.
	/// </summary>
	/// <param name="s">The span of characters to parse.</param>
	/// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>.</param>
	/// <param name="provider">An object that provides culture-specific formatting information.</param>
	/// <returns>A <see cref="SignificantNumber"/> parsed from the input span.</returns>
	public static new SignificantNumber Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) =>
		PreciseNumber.Parse(s, style, provider).ToSignificantNumber();

	/// <summary>
	/// Parses a string into a <see cref="SignificantNumber"/> using the specified style and format provider.
	/// </summary>
	/// <param name="s">The string to parse.</param>
	/// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>.</param>
	/// <param name="provider">An object that provides culture-specific formatting information.</param>
	/// <returns>A <see cref="SignificantNumber"/> parsed from the input string.</returns>
	public static new SignificantNumber Parse(string s, NumberStyles style, IFormatProvider? provider) =>
		PreciseNumber.Parse(s, style, provider).ToSignificantNumber();

	/// <summary>
	/// Attempts to convert a value of type <typeparamref name="TOther"/> to a <see cref="SignificantNumber"/> using a checked conversion.
	/// </summary>
	/// <typeparam name="TOther">The type of the value to convert.</typeparam>
	/// <param name="value">The value to convert.</param>
	/// <param name="result">When this method returns, contains the converted <see cref="SignificantNumber"/>, if the conversion succeeded; otherwise, <c>null</c>.</param>
	/// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
	public static bool TryConvertFromChecked<TOther>(TOther value, [NotNullWhen(true)] out SignificantNumber? result) where TOther : INumberBase<TOther>
	{
		bool tryResult = PreciseNumber.TryConvertFromChecked(value, out var preciseResult);
		result = tryResult ? preciseResult.ToSignificantNumber() : null;
		return tryResult;
	}

	/// <summary>
	/// Attempts to convert a value of type <typeparamref name="TOther"/> to a <see cref="SignificantNumber"/> using a saturating conversion.
	/// </summary>
	/// <typeparam name="TOther">The type of the value to convert.</typeparam>
	/// <param name="value">The value to convert.</param>
	/// <param name="result">When this method returns, contains the converted <see cref="SignificantNumber"/>, if the conversion succeeded; otherwise, <c>null</c>.</param>
	/// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
	public static bool TryConvertFromSaturating<TOther>(TOther value, [NotNullWhen(true)] out SignificantNumber? result) where TOther : INumberBase<TOther>
	{
		bool tryResult = PreciseNumber.TryConvertFromSaturating(value, out var preciseResult);
		result = tryResult ? preciseResult.ToSignificantNumber() : null;
		return tryResult;
	}

	/// <summary>
	/// Attempts to convert a value of type <typeparamref name="TOther"/> to a <see cref="SignificantNumber"/> using a truncating conversion.
	/// </summary>
	/// <typeparam name="TOther">The type of the value to convert.</typeparam>
	/// <param name="value">The value to convert.</param>
	/// <param name="result">When this method returns, contains the converted <see cref="SignificantNumber"/>, if the conversion succeeded; otherwise, <c>null</c>.</param>
	/// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
	public static bool TryConvertFromTruncating<TOther>(TOther value, [NotNullWhen(true)] out SignificantNumber? result) where TOther : INumberBase<TOther>
	{
		bool tryResult = PreciseNumber.TryConvertFromTruncating(value, out var preciseResult);
		result = tryResult ? preciseResult.ToSignificantNumber() : null;
		return tryResult;
	}

	/// <summary>
	/// Attempts to convert a <see cref="SignificantNumber"/> to a value of type <typeparamref name="TOther"/> using a checked conversion.
	/// </summary>
	/// <typeparam name="TOther">The type to convert to.</typeparam>
	/// <param name="value">The <see cref="SignificantNumber"/> to convert.</param>
	/// <param name="result">When this method returns, contains the converted value of type <typeparamref name="TOther"/>, if the conversion succeeded; otherwise, <c>null</c>.</param>
	/// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
	public static bool TryConvertToChecked<TOther>(SignificantNumber value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> =>
		PreciseNumber.TryConvertToChecked(value, out result);

	/// <summary>
	/// Attempts to convert a <see cref="SignificantNumber"/> to a value of type <typeparamref name="TOther"/> using a saturating conversion.
	/// </summary>
	/// <typeparam name="TOther">The type to convert to.</typeparam>
	/// <param name="value">The <see cref="SignificantNumber"/> to convert.</param>
	/// <param name="result">When this method returns, contains the converted value of type <typeparamref name="TOther"/>, if the conversion succeeded; otherwise, <c>null</c>.</param>
	/// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
	public static bool TryConvertToSaturating<TOther>(SignificantNumber value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> =>
		PreciseNumber.TryConvertToSaturating(value, out result);

	/// <summary>
	/// Attempts to convert a <see cref="SignificantNumber"/> to a value of type <typeparamref name="TOther"/> using a truncating conversion.
	/// </summary>
	/// <typeparam name="TOther">The type to convert to.</typeparam>
	/// <param name="value">The <see cref="SignificantNumber"/> to convert.</param>
	/// <param name="result">When this method returns, contains the converted value of type <typeparamref name="TOther"/>, if the conversion succeeded; otherwise, <c>null</c>.</param>
	/// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
	public static bool TryConvertToTruncating<TOther>(SignificantNumber value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> =>
		PreciseNumber.TryConvertToTruncating(value, out result);

	/// <summary>
	/// Attempts to parse a span of characters into a <see cref="SignificantNumber"/> using the specified style and format provider.
	/// </summary>
	/// <param name="s">The span of characters to parse.</param>
	/// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>.</param>
	/// <param name="provider">An object that provides culture-specific formatting information.</param>
	/// <param name="result">
	/// When this method returns, contains the parsed <see cref="SignificantNumber"/>, if the parsing succeeded; otherwise, <c>null</c>.
	/// </param>
	/// <returns><c>true</c> if the parsing succeeded; otherwise, <c>false</c>.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="s"/> is null.</exception>
	/// <exception cref="FormatException">Thrown when <paramref name="s"/> is not in a valid format.</exception>
	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [NotNullWhen(true)] out SignificantNumber? result)
	{
		bool tryResult = PreciseNumber.TryParse(s, style, provider, out var preciseResult);
		result = tryResult ? preciseResult?.ToSignificantNumber() : null;
		return tryResult;
	}

	/// <summary>
	/// Parses a span of characters into a <see cref="SignificantNumber"/> using the specified format provider.
	/// </summary>
	/// <param name="s">The span of characters to parse.</param>
	/// <param name="provider">An object that provides culture-specific formatting information.</param>
	/// <returns>A <see cref="SignificantNumber"/> parsed from the input span.</returns>
	/// <exception cref="FormatException">Thrown when the input span is not in a valid format.</exception>
	/// <exception cref="ArgumentNullException">Thrown when the input span is null.</exception>
	public static new SignificantNumber Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
		PreciseNumber.Parse(s, provider).ToSignificantNumber();

	/// <summary>
	/// Attempts to parse a span of characters into a <see cref="SignificantNumber"/> using the specified format provider.
	/// </summary>
	/// <param name="s">The span of characters to parse.</param>
	/// <param name="provider">An object that provides culture-specific formatting information.</param>
	/// <param name="result">When this method returns, contains the parsed <see cref="SignificantNumber"/>, if the parsing succeeded; otherwise, <c>null</c>.</param>
	/// <returns><c>true</c> if the parsing succeeded; otherwise, <c>false</c>.</returns>
	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [NotNullWhen(true)] out SignificantNumber? result)
	{
		bool tryResult = PreciseNumber.TryParse(s, provider, out var preciseResult);
		result = tryResult ? preciseResult?.ToSignificantNumber() : null;
		return tryResult;
	}

	/// <summary>
	/// Parses a string into a <see cref="SignificantNumber"/> using the specified format provider.
	/// </summary>
	/// <param name="s">The string to parse.</param>
	/// <param name="provider">An object that provides culture-specific formatting information.</param>
	/// <returns>A <see cref="SignificantNumber"/> parsed from the input string.</returns>
	/// <exception cref="FormatException">Thrown when the input string is not in a valid format.</exception>
	/// <exception cref="ArgumentNullException">Thrown when the input string is null.</exception>
	public static new SignificantNumber Parse(string s, IFormatProvider? provider) =>
		PreciseNumber.Parse(s, provider).ToSignificantNumber();

	/// <summary>
	/// Attempts to parse a string into a <see cref="SignificantNumber"/> using the specified format provider.
	/// </summary>
	/// <param name="s">The string to parse.</param>
	/// <param name="provider">An object that provides culture-specific formatting information.</param>
	/// <param name="result">When this method returns, contains the parsed <see cref="SignificantNumber"/>, if the parsing succeeded; otherwise, <c>null</c>.</param>
	/// <returns><c>true</c> if the parsing succeeded; otherwise, <c>false</c>.</returns>
	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [NotNullWhen(true)] out SignificantNumber? result)
	{
		bool tryResult = PreciseNumber.TryParse(s, provider, out var preciseResult);
		result = tryResult ? preciseResult?.ToSignificantNumber() : null;
		return tryResult;
	}

	/// <summary>
	/// Attempts to parse a string into a <see cref="SignificantNumber"/> using the specified style and format provider.
	/// </summary>
	/// <param name="s">The string to parse.</param>
	/// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>.</param>
	/// <param name="provider">An object that provides culture-specific formatting information.</param>
	/// <param name="result">
	/// When this method returns, contains the parsed <see cref="SignificantNumber"/>, if the parsing succeeded; otherwise, <see cref="Zero"/>.
	/// </param>
	/// <returns><c>true</c> if the parsing succeeded; otherwise, <c>false</c>.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="s"/> is null.</exception>
	/// <exception cref="FormatException">Thrown when <paramref name="s"/> is not in a valid format.</exception>
	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result)
	{
		bool tryResult = PreciseNumber.TryParse(s, provider, out var preciseResult);
		result = tryResult ? preciseResult?.ToSignificantNumber() : Zero;
		return tryResult;
	}

	/// <summary>
	/// Provides a specific implementation of the IUtf8SpanFormattable.TryFormat method
	/// to resolve ambiguity.
	/// <param name="utf8Destination">The destination span for the UTF-8 formatted output.</param>
	/// <param name="bytesWritten">The number of bytes written to the destination span.</param>
	/// <param name="format">The format specifier.</param>
	/// <param name="formatProvider">The format provider.</param>
	/// <returns><c>true</c> if the formatting was successful; otherwise, <c>false</c>.</returns>
	/// </summary>
	bool IUtf8SpanFormattable.TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? formatProvider) =>
		 // Explicitly delegate to the base implementation to resolve ambiguity.
		 ((IUtf8SpanFormattable)this).TryFormat(utf8Destination, out bytesWritten, format, formatProvider);
}
