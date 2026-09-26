// Copyright (c) 2023-2026 ktsu-dev contributors

namespace ktsu.SignificantNumber;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

using ktsu.PreciseNumber;

/// <summary>
/// Represents a number whose arithmetic follows the rules for significant figures.
/// </summary>
/// <remarks>
/// <para>
/// A <see cref="SignificantNumber"/> is a value type that holds a <see cref="PreciseNumber"/>. Its default value is
/// zero, so an uninitialized field or array element is a valid number.
/// </para>
/// <para>
/// Addition and subtraction round the result to the fewest decimal places among the operands. Multiplication,
/// division, and modulus round it to the fewest significant digits. An operand of exactly -1, 0, or 1 is treated as
/// having unlimited precision, so it never limits the result.
/// </para>
/// </remarks>
[DebuggerDisplay("{Significand}e{Exponent}")]
public readonly record struct SignificantNumber
	: INumber<SignificantNumber>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SignificantNumber"/> struct that holds the specified value.
	/// </summary>
	/// <param name="value">The value to hold.</param>
	public SignificantNumber(PreciseNumber value) => Value = value;

	/// <summary>
	/// Gets the value this number holds.
	/// </summary>
	public PreciseNumber Value { get; }

	/// <summary>
	/// Gets the value -1.
	/// </summary>
	public static SignificantNumber NegativeOne { get; } = new(PreciseNumber.NegativeOne);

	/// <summary>
	/// Gets the value 1.
	/// </summary>
	public static SignificantNumber One { get; } = new(PreciseNumber.One);

	/// <summary>
	/// Gets the value 0, which is also the default value of the type.
	/// </summary>
	public static SignificantNumber Zero => default;

	/// <summary>
	/// Gets the value of e.
	/// </summary>
	public static SignificantNumber E { get; } = new(PreciseNumber.E);

	/// <summary>
	/// Gets the value of pi.
	/// </summary>
	public static SignificantNumber Pi { get; } = new(PreciseNumber.Pi);

	/// <summary>
	/// Gets the value of tau.
	/// </summary>
	public static SignificantNumber Tau { get; } = new(PreciseNumber.Tau);

	/// <inheritdoc/>
	public static int Radix => PreciseNumber.Radix;

	/// <summary>
	/// Gets the additive identity for significant numbers, which is zero.
	/// </summary>
	public static SignificantNumber AdditiveIdentity => Zero;

	/// <summary>
	/// Gets the multiplicative identity for significant numbers, which is one.
	/// </summary>
	public static SignificantNumber MultiplicativeIdentity => One;

	/// <summary>
	/// Gets the exponent of the number.
	/// </summary>
	public int Exponent => Value.Exponent;

	/// <summary>
	/// Gets the significand of the number.
	/// </summary>
	public BigInteger Significand => Value.Significand;

	/// <summary>
	/// Gets the number of significant digits in the number.
	/// </summary>
	public int SignificantDigits => Value.SignificantDigits;

	/// <summary>
	/// Converts a significant number to the <see cref="PreciseNumber"/> it holds.
	/// </summary>
	/// <param name="value">The significant number to convert.</param>
	public static implicit operator PreciseNumber(SignificantNumber value) => value.Value;

	/// <summary>
	/// Converts a <see cref="PreciseNumber"/> to a significant number that holds it.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	public static explicit operator SignificantNumber(PreciseNumber value) => new(value);

	/// <summary>
	/// Gets the <see cref="PreciseNumber"/> this number holds.
	/// </summary>
	/// <returns>The value this number holds.</returns>
	public PreciseNumber ToPreciseNumber() => Value;

	/// <summary>
	/// Creates a significant number that holds the specified value.
	/// </summary>
	/// <param name="value">The value to hold.</param>
	/// <returns>A significant number that holds <paramref name="value"/>.</returns>
	public static SignificantNumber FromPreciseNumber(PreciseNumber value) => new(value);

	/// <summary>
	/// Creates a significant number from a significand and an exponent, removing trailing zeros from the significand.
	/// </summary>
	/// <param name="exponent">The exponent of the number.</param>
	/// <param name="significand">The significand of the number.</param>
	/// <returns>The number <paramref name="significand"/> × 10^<paramref name="exponent"/>.</returns>
	internal static SignificantNumber CreateFromComponents(int exponent, BigInteger significand) =>
		// PreciseNumber has no public constructor that takes components, and parsing scientific notation is exact.
		new(PreciseNumber.Parse(
			string.Create(CultureInfo.InvariantCulture, $"{significand}E{exponent}"),
			NumberStyles.Float,
			CultureInfo.InvariantCulture));

	/// <summary>
	/// Determines whether a number is exactly -1, 0, or 1, which the significant figure rules treat as having
	/// unlimited precision.
	/// </summary>
	/// <param name="value">The number to check.</param>
	/// <returns><see langword="true"/> when <paramref name="value"/> is -1, 0, or 1.</returns>
	private static bool HasInfinitePrecision(PreciseNumber value) =>
		value.Exponent == 0 && BigInteger.Abs(value.Significand) <= BigInteger.One;

	/// <summary>
	/// Counts the digits after the decimal point in a number.
	/// </summary>
	/// <param name="value">The number to count the decimal digits of.</param>
	/// <returns>The number of digits after the decimal point.</returns>
	private static int CountDecimalDigits(PreciseNumber value) =>
		value.Exponent > 0
		? 0
		: int.Abs(value.Exponent);

	/// <summary>
	/// Gets the lower of the decimal digit counts of two numbers, ignoring an operand with unlimited precision.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns>The lower of the decimal digit counts of the two numbers.</returns>
	private static int LowestDecimalDigits(PreciseNumber left, PreciseNumber right)
	{
		int leftDecimalDigits = CountDecimalDigits(left);
		int rightDecimalDigits = CountDecimalDigits(right);

		leftDecimalDigits = HasInfinitePrecision(left) ? rightDecimalDigits : leftDecimalDigits;
		rightDecimalDigits = HasInfinitePrecision(right) ? leftDecimalDigits : rightDecimalDigits;

		return leftDecimalDigits < rightDecimalDigits
			? leftDecimalDigits
			: rightDecimalDigits;
	}

	/// <summary>
	/// Gets the lower of the significant digit counts of two numbers, ignoring an operand with unlimited precision.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns>The lower of the significant digit counts of the two numbers.</returns>
	private static int LowestSignificantDigits(PreciseNumber left, PreciseNumber right)
	{
		int leftSignificantDigits = left.SignificantDigits;
		int rightSignificantDigits = right.SignificantDigits;

		leftSignificantDigits = HasInfinitePrecision(left) ? rightSignificantDigits : leftSignificantDigits;
		rightSignificantDigits = HasInfinitePrecision(right) ? leftSignificantDigits : rightSignificantDigits;

		return leftSignificantDigits < rightSignificantDigits
			? leftSignificantDigits
			: rightSignificantDigits;
	}

	/// <summary>
	/// Subtracts one number from another.
	/// </summary>
	/// <param name="left">The number to subtract from.</param>
	/// <param name="right">The number to subtract.</param>
	/// <returns>The result of the subtraction.</returns>
	public static SignificantNumber Subtract(PreciseNumber left, PreciseNumber right)
	{
		int lowestDecimalDigits = LowestDecimalDigits(left, right);
		return new(PreciseNumber.Subtract(left, right).Round(lowestDecimalDigits));
	}

	/// <summary>
	/// Adds two numbers.
	/// </summary>
	/// <param name="left">The first number to add.</param>
	/// <param name="right">The second number to add.</param>
	/// <returns>The result of the addition.</returns>
	public static SignificantNumber Add(PreciseNumber left, PreciseNumber right)
	{
		int lowestDecimalDigits = LowestDecimalDigits(left, right);
		return new(PreciseNumber.Add(left, right).Round(lowestDecimalDigits));
	}

	/// <summary>
	/// Multiplies two numbers.
	/// </summary>
	/// <param name="left">The first number to multiply.</param>
	/// <param name="right">The second number to multiply.</param>
	/// <returns>The result of the multiplication.</returns>
	public static SignificantNumber Multiply(PreciseNumber left, PreciseNumber right)
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
	public static SignificantNumber Divide(PreciseNumber left, PreciseNumber right)
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
	public static SignificantNumber Mod(PreciseNumber left, PreciseNumber right)
	{
		int lowestSignificantDigits = LowestSignificantDigits(left, right);
		return PreciseNumber.Mod(left, right)
			.ToSignificantNumber(lowestSignificantDigits);
	}

	/// <summary>
	/// Increments the specified significant number by one.
	/// </summary>
	/// <param name="value">The significant number to increment.</param>
	/// <returns>The incremented value.</returns>
	public static SignificantNumber Increment(SignificantNumber value) =>
		new(PreciseNumber.Increment(value.Value));

	/// <summary>
	/// Decrements the specified significant number by one.
	/// </summary>
	/// <param name="value">The significant number to decrement.</param>
	/// <returns>The decremented value.</returns>
	public static SignificantNumber Decrement(SignificantNumber value) =>
		new(PreciseNumber.Decrement(value.Value));

	/// <summary>
	/// Returns the unary plus of a number.
	/// </summary>
	/// <param name="value">The number.</param>
	/// <returns>The unary plus of the number.</returns>
	public static SignificantNumber Plus(SignificantNumber value) =>
		new(PreciseNumber.Plus(value.Value));

	/// <summary>
	/// Negates the specified significant number.
	/// </summary>
	/// <param name="value">The significant number to negate.</param>
	/// <returns>The negated value.</returns>
	public static SignificantNumber Negate(SignificantNumber value) =>
		new(PreciseNumber.Negate(value.Value));

	/// <summary>
	/// Determines whether one number is greater than another.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><see langword="true"/> if the first number is greater than the second; otherwise, <see langword="false"/>.</returns>
	public static bool GreaterThan(PreciseNumber left, PreciseNumber right) =>
		left.CompareTo(right) > 0;

	/// <summary>
	/// Determines whether one number is greater than or equal to another.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><see langword="true"/> if the first number is greater than or equal to the second; otherwise, <see langword="false"/>.</returns>
	public static bool GreaterThanOrEqual(PreciseNumber left, PreciseNumber right) =>
		left.CompareTo(right) >= 0;

	/// <summary>
	/// Determines whether one number is less than another.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><see langword="true"/> if the first number is less than the second; otherwise, <see langword="false"/>.</returns>
	public static bool LessThan(PreciseNumber left, PreciseNumber right) =>
		left.CompareTo(right) < 0;

	/// <summary>
	/// Determines whether one number is less than or equal to another.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><see langword="true"/> if the first number is less than or equal to the second; otherwise, <see langword="false"/>.</returns>
	public static bool LessThanOrEqual(PreciseNumber left, PreciseNumber right) =>
		left.CompareTo(right) <= 0;

	/// <summary>
	/// Determines whether two numbers are equal.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><see langword="true"/> if the two numbers are equal; otherwise, <see langword="false"/>.</returns>
	public static bool Equal(PreciseNumber left, PreciseNumber right) =>
		left.CompareTo(right) == 0;

	/// <summary>
	/// Determines whether two numbers are not equal.
	/// </summary>
	/// <param name="left">The first number.</param>
	/// <param name="right">The second number.</param>
	/// <returns><see langword="true"/> if the two numbers are not equal; otherwise, <see langword="false"/>.</returns>
	public static bool NotEqual(PreciseNumber left, PreciseNumber right) =>
		left.CompareTo(right) != 0;

	/// <summary>
	/// Compares two numbers at the lower of their significant digit counts.
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
	public static int CompareTo(PreciseNumber left, PreciseNumber right)
	{
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

	/// <summary>
	/// Determines whether a significant number and a <see cref="PreciseNumber"/> are equal.
	/// </summary>
	/// <param name="left">The significant number.</param>
	/// <param name="right">The precise number.</param>
	/// <returns><see langword="true"/> if the two numbers are equal; otherwise, <see langword="false"/>.</returns>
	public static bool operator ==(SignificantNumber left, PreciseNumber right) =>
		Equal(left, right);

	/// <summary>
	/// Determines whether a <see cref="PreciseNumber"/> and a significant number are equal.
	/// </summary>
	/// <param name="left">The precise number.</param>
	/// <param name="right">The significant number.</param>
	/// <returns><see langword="true"/> if the two numbers are equal; otherwise, <see langword="false"/>.</returns>
	public static bool operator ==(PreciseNumber left, SignificantNumber right) =>
		Equal(left, right);

	/// <summary>
	/// Determines whether a significant number and a <see cref="PreciseNumber"/> are not equal.
	/// </summary>
	/// <param name="left">The significant number.</param>
	/// <param name="right">The precise number.</param>
	/// <returns><see langword="true"/> if the two numbers are not equal; otherwise, <see langword="false"/>.</returns>
	public static bool operator !=(SignificantNumber left, PreciseNumber right) =>
		NotEqual(left, right);

	/// <summary>
	/// Determines whether a <see cref="PreciseNumber"/> and a significant number are not equal.
	/// </summary>
	/// <param name="left">The precise number.</param>
	/// <param name="right">The significant number.</param>
	/// <returns><see langword="true"/> if the two numbers are not equal; otherwise, <see langword="false"/>.</returns>
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
	/// <returns><see langword="true"/> if the type implements the generic interface; otherwise, <see langword="false"/>.</returns>
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
	/// <returns>The current number raised to <paramref name="power"/>, rounded to the fewest significant digits of the two.</returns>
	/// <exception cref="DivideByZeroException">Thrown when the current number is zero and <paramref name="power"/> is negative.</exception>
	/// <exception cref="ArgumentException">Thrown when the current number is negative and <paramref name="power"/> is not an integer, as the result is not a real number.</exception>
	public SignificantNumber Pow(PreciseNumber power)
	{
		if (Equal(power, Zero))
		{
			return One;
		}
		else if (Equal(this, Zero))
		{
			return PreciseNumber.IsNegative(power)
				? throw new DivideByZeroException("Cannot raise zero to a negative power.")
				: Zero;
		}
		else if (Equal(this, One))
		{
			return One;
		}

		bool isNegativeBase = PreciseNumber.IsNegative(Value);
		if (isNegativeBase && !PreciseNumber.IsInteger(power))
		{
			throw new ArgumentException("Cannot raise a negative number to a non-integer power.", nameof(power));
		}

		if (PreciseNumber.IsInteger(power) && Math.Abs(power.To<double>()) <= MaxExactIntegerPower)
		{
			// An integer power is an exact count, like the counting numbers in a multiplication, so the
			// result keeps the base's significant digits. Computing it by exact repeated multiplication
			// makes x.Pow(2) agree with x * x and keeps the sign of a negative base.
			BigInteger exponent = IntegerValue(power);
			PreciseNumber positivePower = ExactIntegerPower(Value, BigInteger.Abs(exponent));
			PreciseNumber exact = exponent.Sign < 0 ? PreciseNumber.Divide(PreciseNumber.One, positivePower) : positivePower;
			return exact.ToSignificantNumber(Value.SignificantDigits);
		}

		// An integer power too large to compute exactly is still exact, so it keeps the base's
		// precision, bounded by the double the result is computed in.
		int significantDigits = PreciseNumber.IsInteger(power)
			? int.Min(Value.SignificantDigits, DoubleSignificantDigits)
			: LowestSignificantDigits(this, power);

		// Use logarithm and exponential to support decimal powers. This computes |x|^p, so the
		// sign of a negative base is restored for odd integer powers.
		double logValue = Math.Log(Math.Abs(Value.To<double>()));
		double magnitude = Math.Exp(logValue * power.To<double>());
		double result = isNegativeBase && PreciseNumber.IsOddInteger(power) ? -magnitude : magnitude;
		return result.ToSignificantNumber(significantDigits);
	}

	/// <summary>
	/// Returns the result of raising e to the specified power.
	/// </summary>
	/// <param name="power">The power to raise e to.</param>
	/// <returns>e raised to <paramref name="power"/>, rounded to the fewest significant digits of the two.</returns>
	public static SignificantNumber Exp(PreciseNumber power)
	{
		if (Equal(power, Zero))
		{
			return One;
		}
		else if (Equal(power, One))
		{
			return E;
		}

		// An integer power is exact, so the precision is bounded only by the double the result is
		// computed in, not by the single significant digit an integer like 2 carries.
		int significantDigits = PreciseNumber.IsInteger(power)
			? int.Min(E.SignificantDigits, DoubleSignificantDigits)
			: LowestSignificantDigits(E, power);

		return Math.Exp(power.To<double>())
			.ToSignificantNumber(significantDigits);
	}

	/// <summary>
	/// The number of significant decimal digits a <see cref="double"/> always represents exactly.
	/// </summary>
	private const int DoubleSignificantDigits = 15;

	/// <summary>
	/// The largest integer power <see cref="Pow"/> computes exactly. Larger powers fall back to the
	/// logarithm path, so that an extreme exponent cannot build an arbitrarily large intermediate.
	/// </summary>
	private const int MaxExactIntegerPower = 1024;

	/// <summary>
	/// Gets the value of an integer <see cref="PreciseNumber"/> as a <see cref="BigInteger"/>.
	/// </summary>
	/// <param name="value">A number that <see cref="PreciseNumber.IsInteger"/> reports as an integer.</param>
	/// <returns>The integer value of <paramref name="value"/>.</returns>
	private static BigInteger IntegerValue(PreciseNumber value) =>
		value.Exponent >= 0
			? value.Significand * BigInteger.Pow(10, value.Exponent)
			: value.Significand / BigInteger.Pow(10, -value.Exponent);

	/// <summary>
	/// Raises a number to a non-negative integer power by exact repeated squaring.
	/// </summary>
	/// <param name="value">The base.</param>
	/// <param name="exponent">The non-negative power.</param>
	/// <returns><paramref name="value"/> raised to <paramref name="exponent"/>, with no rounding.</returns>
	private static PreciseNumber ExactIntegerPower(PreciseNumber value, BigInteger exponent)
	{
		PreciseNumber result = PreciseNumber.One;
		PreciseNumber square = value;
		while (!exponent.IsZero)
		{
			if (!exponent.IsEven)
			{
				result = PreciseNumber.Multiply(result, square);
			}

			exponent >>= 1;
			if (!exponent.IsZero)
			{
				square = PreciseNumber.Multiply(square, square);
			}
		}

		return result;
	}

	/// <summary>
	/// Compares the current instance with another <see cref="SignificantNumber"/> at the lower of their significant digit counts.
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
	public int CompareTo(SignificantNumber other) =>
		CompareTo(this, other);

	/// <summary>
	/// Compares the current instance with an object.
	/// </summary>
	/// <param name="obj">The object to compare with the current instance.</param>
	/// <returns>
	/// A signed integer that indicates the relative values of the current instance and <paramref name="obj"/>. A
	/// <see langword="null"/> object sorts before every number.
	/// </returns>
	/// <remarks>
	/// A <see cref="SignificantNumber"/> or <see cref="PreciseNumber"/> is compared at the lower of the two significant
	/// digit counts. Any other object is passed to <see cref="PreciseNumber.CompareTo(object)"/>.
	/// </remarks>
	public int CompareTo(object? obj) =>
		obj switch
		{
			null => 1,
			SignificantNumber significantNumber => CompareTo(significantNumber),
			PreciseNumber preciseNumber => CompareTo(this, preciseNumber),
			_ => Value.CompareTo(obj),
		};

	/// <summary>
	/// Compares the value of the current instance with another number, without applying significant figure rules.
	/// </summary>
	/// <typeparam name="TInput">The type of the other number.</typeparam>
	/// <param name="other">The number to compare with the current instance.</param>
	/// <returns>A signed integer that indicates the relative values of the current instance and <paramref name="other"/>.</returns>
	public int CompareTo<TInput>(TInput other)
		where TInput : INumber<TInput> =>
		typeof(TInput) == typeof(SignificantNumber)
			? Value.CompareTo(((SignificantNumber)(object)other).Value)
			: Value.CompareTo(other);

	/// <summary>
	/// Compares the value of the current instance with another number, without applying significant figure rules.
	/// </summary>
	/// <typeparam name="TNumber">The type of the other number.</typeparam>
	/// <param name="obj">The number to compare with the current instance.</param>
	/// <returns>A signed integer that indicates the relative values of the current instance and <paramref name="obj"/>.</returns>
	public int CompareTo<TNumber>(INumber<TNumber>? obj)
		where TNumber : INumber<TNumber> =>
		obj is SignificantNumber significantNumber
			? Value.CompareTo(significantNumber.Value)
			: Value.CompareTo(obj);

	/// <summary>
	/// Returns the absolute value of the specified <see cref="SignificantNumber"/>.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to compute the absolute value for.</param>
	/// <returns>The absolute value of <paramref name="value"/>.</returns>
	public static SignificantNumber Abs(SignificantNumber value) =>
		new(PreciseNumber.Abs(value.Value));

	/// <summary>
	/// Returns the absolute value of the current instance.
	/// </summary>
	/// <returns>The absolute value of the number this instance holds.</returns>
	public PreciseNumber Abs() => Value.Abs();

	/// <summary>
	/// Rounds the current instance to the specified number of decimal digits.
	/// </summary>
	/// <param name="decimalDigits">The number of digits to keep after the decimal point.</param>
	/// <returns>The rounded value.</returns>
	public PreciseNumber Round(int decimalDigits) => Value.Round(decimalDigits);

	/// <summary>
	/// Reduces the current instance to the specified number of significant digits.
	/// </summary>
	/// <param name="significantDigits">The number of significant digits to keep.</param>
	/// <returns>The reduced value.</returns>
	public PreciseNumber ReduceSignificance(int significantDigits) => Value.ReduceSignificance(significantDigits);

	/// <summary>
	/// Clamps the current instance between a minimum and a maximum.
	/// </summary>
	/// <typeparam name="TNumber">The type of the bounds.</typeparam>
	/// <param name="min">The lowest value to return.</param>
	/// <param name="max">The highest value to return.</param>
	/// <returns>The clamped value.</returns>
	public PreciseNumber Clamp<TNumber>(TNumber min, TNumber max)
		where TNumber : INumber<TNumber> =>
		Value.Clamp(min, max);

	/// <summary>
	/// Returns the larger of two numbers, compared by value without applying significant figure rules.
	/// </summary>
	/// <param name="x">The first number.</param>
	/// <param name="y">The second number.</param>
	/// <returns><paramref name="x"/> when it is greater than <paramref name="y"/>; otherwise, <paramref name="y"/>.</returns>
	public static SignificantNumber Max(SignificantNumber x, SignificantNumber y) =>
		new(PreciseNumber.Max(x.Value, y.Value));

	/// <summary>
	/// Returns the smaller of two numbers, compared by value without applying significant figure rules.
	/// </summary>
	/// <param name="x">The first number.</param>
	/// <param name="y">The second number.</param>
	/// <returns><paramref name="x"/> when it is less than <paramref name="y"/>; otherwise, <paramref name="y"/>.</returns>
	public static SignificantNumber Min(SignificantNumber x, SignificantNumber y) =>
		new(PreciseNumber.Min(x.Value, y.Value));

	/// <summary>
	/// Clamps a number between a minimum and a maximum, compared by value without applying significant figure rules.
	/// </summary>
	/// <param name="value">The number to clamp.</param>
	/// <param name="min">The lowest value to return.</param>
	/// <param name="max">The highest value to return.</param>
	/// <returns>The clamped value.</returns>
	public static SignificantNumber Clamp(SignificantNumber value, SignificantNumber min, SignificantNumber max) =>
		new(PreciseNumber.Clamp(value.Value, min.Value, max.Value));

	/// <summary>
	/// Rounds a number to the specified number of decimal digits.
	/// </summary>
	/// <param name="value">The number to round.</param>
	/// <param name="decimalDigits">The number of digits to keep after the decimal point.</param>
	/// <returns>The rounded value.</returns>
	public static SignificantNumber Round(SignificantNumber value, int decimalDigits) =>
		new(PreciseNumber.Round(value.Value, decimalDigits));

	/// <summary>
	/// Returns the square of the current instance.
	/// </summary>
	/// <returns>The value multiplied by itself.</returns>
	public PreciseNumber Squared() => Value.Squared();

	/// <summary>
	/// Returns the cube of the current instance.
	/// </summary>
	/// <returns>The value multiplied by itself twice.</returns>
	public PreciseNumber Cubed() => Value.Cubed();

	/// <summary>
	/// Converts the current instance to the specified numeric type.
	/// </summary>
	/// <typeparam name="TOutput">The type to convert to.</typeparam>
	/// <returns>The converted value.</returns>
	/// <exception cref="OverflowException">Thrown when the value is outside the range of <typeparamref name="TOutput"/>.</exception>
	public TOutput To<TOutput>()
		where TOutput : INumber<TOutput> =>
		Value.To<TOutput>();

	/// <inheritdoc/>
	public override string ToString() => PreciseNumber.ToString(Value, null, null);

	/// <summary>
	/// Converts the current instance to a string using the specified format provider.
	/// </summary>
	/// <param name="formatProvider">An object that provides culture-specific formatting information.</param>
	/// <returns>The string representation of the number.</returns>
	public string ToString(IFormatProvider? formatProvider) => PreciseNumber.ToString(Value, null, formatProvider);

	/// <summary>
	/// Converts the current instance to a string using the specified format.
	/// </summary>
	/// <param name="format">The format to use.</param>
	/// <returns>The string representation of the number.</returns>
	public string ToString(string format) => PreciseNumber.ToString(Value, format, null);

	/// <inheritdoc/>
	public string ToString(string? format, IFormatProvider? formatProvider) => PreciseNumber.ToString(Value, format, formatProvider);

	/// <inheritdoc/>
	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) =>
		Value.TryFormat(destination, out charsWritten, format, provider);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is canonical.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> is canonical; otherwise, <see langword="false"/>.</returns>
	public static bool IsCanonical(SignificantNumber value) =>
		PreciseNumber.IsCanonical(value.Value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is a complex number.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> is a complex number; otherwise, <see langword="false"/>.</returns>
	public static bool IsComplexNumber(SignificantNumber value) =>
		PreciseNumber.IsComplexNumber(value.Value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is an even integer.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> is an even integer; otherwise, <see langword="false"/>.</returns>
	public static bool IsEvenInteger(SignificantNumber value) =>
		PreciseNumber.IsEvenInteger(value.Value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is finite.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> is finite; otherwise, <see langword="false"/>.</returns>
	public static bool IsFinite(SignificantNumber value) =>
		PreciseNumber.IsFinite(value.Value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is an imaginary number.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> is an imaginary number; otherwise, <see langword="false"/>.</returns>
	public static bool IsImaginaryNumber(SignificantNumber value) =>
		PreciseNumber.IsImaginaryNumber(value.Value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> represents infinity.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> represents infinity; otherwise, <see langword="false"/>.</returns>
	public static bool IsInfinity(SignificantNumber value) =>
		PreciseNumber.IsInfinity(value.Value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is an integer.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> is an integer; otherwise, <see langword="false"/>.</returns>
	public static bool IsInteger(SignificantNumber value) =>
		PreciseNumber.IsInteger(value.Value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is not a number (NaN).
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> is NaN; otherwise, <see langword="false"/>.</returns>
	public static bool IsNaN(SignificantNumber value) =>
		PreciseNumber.IsNaN(value.Value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is negative.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> is negative; otherwise, <see langword="false"/>.</returns>
	public static bool IsNegative(SignificantNumber value) =>
		PreciseNumber.IsNegative(value.Value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> represents negative infinity.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> represents negative infinity; otherwise, <see langword="false"/>.</returns>
	public static bool IsNegativeInfinity(SignificantNumber value) =>
		PreciseNumber.IsNegativeInfinity(value.Value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is a normal number.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> is a normal number; otherwise, <see langword="false"/>.</returns>
	public static bool IsNormal(SignificantNumber value) =>
		PreciseNumber.IsNormal(value.Value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is an odd integer.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> is an odd integer; otherwise, <see langword="false"/>.</returns>
	public static bool IsOddInteger(SignificantNumber value) =>
		PreciseNumber.IsOddInteger(value.Value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is positive.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> is positive; otherwise, <see langword="false"/>.</returns>
	public static bool IsPositive(SignificantNumber value) =>
		PreciseNumber.IsPositive(value.Value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> represents positive infinity.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> represents positive infinity; otherwise, <see langword="false"/>.</returns>
	public static bool IsPositiveInfinity(SignificantNumber value) =>
		PreciseNumber.IsPositiveInfinity(value.Value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is a real number.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> is a real number; otherwise, <see langword="false"/>.</returns>
	public static bool IsRealNumber(SignificantNumber value) =>
		PreciseNumber.IsRealNumber(value.Value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is subnormal.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> is subnormal; otherwise, <see langword="false"/>.</returns>
	public static bool IsSubnormal(SignificantNumber value) =>
		PreciseNumber.IsSubnormal(value.Value);

	/// <summary>
	/// Determines whether the specified <see cref="SignificantNumber"/> is zero.
	/// </summary>
	/// <param name="value">The <see cref="SignificantNumber"/> to check.</param>
	/// <returns><see langword="true"/> if the <paramref name="value"/> is zero; otherwise, <see langword="false"/>.</returns>
	public static bool IsZero(SignificantNumber value) =>
		PreciseNumber.IsZero(value.Value);

	/// <summary>
	/// Returns the larger magnitude of two <see cref="SignificantNumber"/> instances.
	/// </summary>
	/// <param name="x">The first <see cref="SignificantNumber"/> to compare.</param>
	/// <param name="y">The second <see cref="SignificantNumber"/> to compare.</param>
	/// <returns>The <see cref="SignificantNumber"/> with the larger magnitude.</returns>
	public static SignificantNumber MaxMagnitude(SignificantNumber x, SignificantNumber y) =>
		new(PreciseNumber.MaxMagnitude(x.Value, y.Value));

	/// <summary>
	/// Returns the larger magnitude of two <see cref="SignificantNumber"/> instances, or the first one if both have the same magnitude.
	/// </summary>
	/// <param name="x">The first <see cref="SignificantNumber"/> to compare.</param>
	/// <param name="y">The second <see cref="SignificantNumber"/> to compare.</param>
	/// <returns>The <see cref="SignificantNumber"/> with the larger magnitude, or <paramref name="x"/> if both have the same magnitude.</returns>
	public static SignificantNumber MaxMagnitudeNumber(SignificantNumber x, SignificantNumber y) =>
		new(PreciseNumber.MaxMagnitudeNumber(x.Value, y.Value));

	/// <summary>
	/// Returns the smaller magnitude of two <see cref="SignificantNumber"/> instances.
	/// </summary>
	/// <param name="x">The first <see cref="SignificantNumber"/> to compare.</param>
	/// <param name="y">The second <see cref="SignificantNumber"/> to compare.</param>
	/// <returns>The <see cref="SignificantNumber"/> with the smaller magnitude.</returns>
	public static SignificantNumber MinMagnitude(SignificantNumber x, SignificantNumber y) =>
		new(PreciseNumber.MinMagnitude(x.Value, y.Value));

	/// <summary>
	/// Returns the smaller magnitude of two <see cref="SignificantNumber"/> instances, or the first one if both have the same magnitude.
	/// </summary>
	/// <param name="x">The first <see cref="SignificantNumber"/> to compare.</param>
	/// <param name="y">The second <see cref="SignificantNumber"/> to compare.</param>
	/// <returns>The <see cref="SignificantNumber"/> with the smaller magnitude, or <paramref name="x"/> if both have the same magnitude.</returns>
	public static SignificantNumber MinMagnitudeNumber(SignificantNumber x, SignificantNumber y) =>
		new(PreciseNumber.MinMagnitudeNumber(x.Value, y.Value));

	/// <summary>
	/// Parses a span of characters into a <see cref="SignificantNumber"/> using the specified style and format provider.
	/// </summary>
	/// <param name="s">The span of characters to parse.</param>
	/// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>.</param>
	/// <param name="provider">An object that provides culture-specific formatting information.</param>
	/// <returns>A <see cref="SignificantNumber"/> parsed from the input span.</returns>
	public static SignificantNumber Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) =>
		new(PreciseNumber.Parse(s, style, provider));

	/// <summary>
	/// Parses a string into a <see cref="SignificantNumber"/> using the specified style and format provider.
	/// </summary>
	/// <param name="s">The string to parse.</param>
	/// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>.</param>
	/// <param name="provider">An object that provides culture-specific formatting information.</param>
	/// <returns>A <see cref="SignificantNumber"/> parsed from the input string.</returns>
	public static SignificantNumber Parse(string s, NumberStyles style, IFormatProvider? provider) =>
		new(PreciseNumber.Parse(s, style, provider));

	/// <summary>
	/// Parses a span of characters into a <see cref="SignificantNumber"/> using the specified format provider.
	/// </summary>
	/// <param name="s">The span of characters to parse.</param>
	/// <param name="provider">An object that provides culture-specific formatting information.</param>
	/// <returns>A <see cref="SignificantNumber"/> parsed from the input span.</returns>
	/// <exception cref="FormatException">Thrown when the input span is not in a valid format.</exception>
	public static SignificantNumber Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
		new(PreciseNumber.Parse(s, provider));

	/// <summary>
	/// Parses a string into a <see cref="SignificantNumber"/> using the specified format provider.
	/// </summary>
	/// <param name="s">The string to parse.</param>
	/// <param name="provider">An object that provides culture-specific formatting information.</param>
	/// <returns>A <see cref="SignificantNumber"/> parsed from the input string.</returns>
	/// <exception cref="FormatException">Thrown when the input string is not in a valid format.</exception>
	public static SignificantNumber Parse(string s, IFormatProvider? provider) =>
		new(PreciseNumber.Parse(s, provider));

	/// <summary>
	/// Attempts to parse a span of characters into a <see cref="SignificantNumber"/> using the specified style and format provider.
	/// </summary>
	/// <param name="s">The span of characters to parse.</param>
	/// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>.</param>
	/// <param name="provider">An object that provides culture-specific formatting information.</param>
	/// <param name="result">When this method returns, contains the parsed number if parsing succeeded, or zero if it failed.</param>
	/// <returns><see langword="true"/> if the parsing succeeded; otherwise, <see langword="false"/>.</returns>
	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out SignificantNumber result)
	{
		bool parsed = PreciseNumber.TryParse(s, style, provider, out PreciseNumber preciseResult);
		result = new(preciseResult);
		return parsed;
	}

	/// <summary>
	/// Attempts to parse a string into a <see cref="SignificantNumber"/> using the specified style and format provider.
	/// </summary>
	/// <param name="s">The string to parse.</param>
	/// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s"/>.</param>
	/// <param name="provider">An object that provides culture-specific formatting information.</param>
	/// <param name="result">When this method returns, contains the parsed number if parsing succeeded, or zero if it failed.</param>
	/// <returns><see langword="true"/> if the parsing succeeded; otherwise, <see langword="false"/>.</returns>
	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out SignificantNumber result)
	{
		bool parsed = PreciseNumber.TryParse(s, style, provider, out PreciseNumber preciseResult);
		result = new(preciseResult);
		return parsed;
	}

	/// <summary>
	/// Attempts to parse a span of characters into a <see cref="SignificantNumber"/> using the specified format provider.
	/// </summary>
	/// <param name="s">The span of characters to parse.</param>
	/// <param name="provider">An object that provides culture-specific formatting information.</param>
	/// <param name="result">When this method returns, contains the parsed number if parsing succeeded, or zero if it failed.</param>
	/// <returns><see langword="true"/> if the parsing succeeded; otherwise, <see langword="false"/>.</returns>
	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out SignificantNumber result)
	{
		bool parsed = PreciseNumber.TryParse(s, provider, out PreciseNumber preciseResult);
		result = new(preciseResult);
		return parsed;
	}

	/// <summary>
	/// Attempts to parse a string into a <see cref="SignificantNumber"/> using the specified format provider.
	/// </summary>
	/// <param name="s">The string to parse.</param>
	/// <param name="provider">An object that provides culture-specific formatting information.</param>
	/// <param name="result">When this method returns, contains the parsed number if parsing succeeded, or zero if it failed.</param>
	/// <returns><see langword="true"/> if the parsing succeeded; otherwise, <see langword="false"/>.</returns>
	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out SignificantNumber result)
	{
		bool parsed = PreciseNumber.TryParse(s, provider, out PreciseNumber preciseResult);
		result = new(preciseResult);
		return parsed;
	}

	/// <summary>
	/// Attempts to convert a value of type <typeparamref name="TOther"/> to a <see cref="SignificantNumber"/> using a checked conversion.
	/// </summary>
	/// <typeparam name="TOther">The type of the value to convert.</typeparam>
	/// <param name="value">The value to convert.</param>
	/// <param name="result">When this method returns, contains the converted number if the conversion succeeded, or zero if it failed.</param>
	/// <returns><see langword="true"/> if the conversion succeeded; <see langword="false"/> if <typeparamref name="TOther"/> isn't supported.</returns>
	/// <remarks>Follows <see cref="PreciseNumber.TryConvertFromChecked{TOther}(TOther, out PreciseNumber)"/>.</remarks>
	public static bool TryConvertFromChecked<TOther>(TOther value, out SignificantNumber result)
		where TOther : INumberBase<TOther>
	{
		if (TryUnwrap(value, out result))
		{
			return true;
		}

		bool converted = PreciseNumber.TryConvertFromChecked(value, out PreciseNumber preciseResult);
		result = new(preciseResult);
		return converted;
	}

	/// <summary>
	/// Attempts to convert a value of type <typeparamref name="TOther"/> to a <see cref="SignificantNumber"/> using a saturating conversion.
	/// </summary>
	/// <typeparam name="TOther">The type of the value to convert.</typeparam>
	/// <param name="value">The value to convert.</param>
	/// <param name="result">When this method returns, contains the converted number if the conversion succeeded, or zero if it failed.</param>
	/// <returns><see langword="true"/> if the conversion succeeded; <see langword="false"/> if <typeparamref name="TOther"/> isn't supported.</returns>
	/// <remarks>Follows <see cref="PreciseNumber.TryConvertFromSaturating{TOther}(TOther, out PreciseNumber)"/>.</remarks>
	public static bool TryConvertFromSaturating<TOther>(TOther value, out SignificantNumber result)
		where TOther : INumberBase<TOther>
	{
		if (TryUnwrap(value, out result))
		{
			return true;
		}

		bool converted = PreciseNumber.TryConvertFromSaturating(value, out PreciseNumber preciseResult);
		result = new(preciseResult);
		return converted;
	}

	/// <summary>
	/// Attempts to convert a value of type <typeparamref name="TOther"/> to a <see cref="SignificantNumber"/> using a truncating conversion.
	/// </summary>
	/// <typeparam name="TOther">The type of the value to convert.</typeparam>
	/// <param name="value">The value to convert.</param>
	/// <param name="result">When this method returns, contains the converted number if the conversion succeeded, or zero if it failed.</param>
	/// <returns><see langword="true"/> if the conversion succeeded; <see langword="false"/> if <typeparamref name="TOther"/> isn't supported.</returns>
	/// <remarks>Follows <see cref="PreciseNumber.TryConvertFromTruncating{TOther}(TOther, out PreciseNumber)"/>.</remarks>
	public static bool TryConvertFromTruncating<TOther>(TOther value, out SignificantNumber result)
		where TOther : INumberBase<TOther>
	{
		if (TryUnwrap(value, out result))
		{
			return true;
		}

		bool converted = PreciseNumber.TryConvertFromTruncating(value, out PreciseNumber preciseResult);
		result = new(preciseResult);
		return converted;
	}

	/// <summary>
	/// Attempts to convert a <see cref="SignificantNumber"/> to a value of type <typeparamref name="TOther"/> using a checked conversion.
	/// </summary>
	/// <typeparam name="TOther">The type to convert to.</typeparam>
	/// <param name="value">The <see cref="SignificantNumber"/> to convert.</param>
	/// <param name="result">When this method returns, contains the converted value if the conversion succeeded.</param>
	/// <returns><see langword="true"/> if the conversion succeeded; <see langword="false"/> if <typeparamref name="TOther"/> isn't supported.</returns>
	/// <remarks>Follows <see cref="PreciseNumber.TryConvertToChecked{TOther}(PreciseNumber, out TOther)"/>.</remarks>
	public static bool TryConvertToChecked<TOther>(SignificantNumber value, [MaybeNullWhen(false)] out TOther result)
		where TOther : INumberBase<TOther> =>
		TryWrap(value, out result) || PreciseNumber.TryConvertToChecked(value.Value, out result);

	/// <summary>
	/// Attempts to convert a <see cref="SignificantNumber"/> to a value of type <typeparamref name="TOther"/> using a saturating conversion.
	/// </summary>
	/// <typeparam name="TOther">The type to convert to.</typeparam>
	/// <param name="value">The <see cref="SignificantNumber"/> to convert.</param>
	/// <param name="result">When this method returns, contains the converted value if the conversion succeeded.</param>
	/// <returns><see langword="true"/> if the conversion succeeded; <see langword="false"/> if <typeparamref name="TOther"/> isn't supported.</returns>
	/// <remarks>Follows <see cref="PreciseNumber.TryConvertToSaturating{TOther}(PreciseNumber, out TOther)"/>.</remarks>
	public static bool TryConvertToSaturating<TOther>(SignificantNumber value, [MaybeNullWhen(false)] out TOther result)
		where TOther : INumberBase<TOther> =>
		TryWrap(value, out result) || PreciseNumber.TryConvertToSaturating(value.Value, out result);

	/// <summary>
	/// Attempts to convert a <see cref="SignificantNumber"/> to a value of type <typeparamref name="TOther"/> using a truncating conversion.
	/// </summary>
	/// <typeparam name="TOther">The type to convert to.</typeparam>
	/// <param name="value">The <see cref="SignificantNumber"/> to convert.</param>
	/// <param name="result">When this method returns, contains the converted value if the conversion succeeded.</param>
	/// <returns><see langword="true"/> if the conversion succeeded; <see langword="false"/> if <typeparamref name="TOther"/> isn't supported.</returns>
	/// <remarks>Follows <see cref="PreciseNumber.TryConvertToTruncating{TOther}(PreciseNumber, out TOther)"/>.</remarks>
	public static bool TryConvertToTruncating<TOther>(SignificantNumber value, [MaybeNullWhen(false)] out TOther result)
		where TOther : INumberBase<TOther> =>
		TryWrap(value, out result) || PreciseNumber.TryConvertToTruncating(value.Value, out result);

	/// <summary>
	/// Converts a <see cref="SignificantNumber"/> or <see cref="PreciseNumber"/> without going through
	/// <see cref="PreciseNumber"/>'s conversions, which don't recognize <see cref="SignificantNumber"/>.
	/// </summary>
	/// <typeparam name="TOther">The type of the value to convert.</typeparam>
	/// <param name="value">The value to convert.</param>
	/// <param name="result">When this method returns, contains the converted number, or zero for any other type.</param>
	/// <returns><see langword="true"/> when <typeparamref name="TOther"/> is one of the two types.</returns>
	private static bool TryUnwrap<TOther>(TOther value, out SignificantNumber result)
	{
		if (typeof(TOther) == typeof(SignificantNumber))
		{
			result = (SignificantNumber)(object)value!;
			return true;
		}

		if (typeof(TOther) == typeof(PreciseNumber))
		{
			result = new((PreciseNumber)(object)value!);
			return true;
		}

		result = default;
		return false;
	}

	/// <summary>
	/// Converts to a <see cref="SignificantNumber"/> or <see cref="PreciseNumber"/> without going through
	/// <see cref="PreciseNumber"/>'s conversions, which don't recognize <see cref="SignificantNumber"/>.
	/// </summary>
	/// <typeparam name="TOther">The type to convert to.</typeparam>
	/// <param name="value">The value to convert.</param>
	/// <param name="result">When this method returns, contains the converted value when <typeparamref name="TOther"/> is one of the two types.</param>
	/// <returns><see langword="true"/> when <typeparamref name="TOther"/> is one of the two types.</returns>
	private static bool TryWrap<TOther>(SignificantNumber value, [MaybeNullWhen(false)] out TOther result)
	{
		if (typeof(TOther) == typeof(PreciseNumber))
		{
			result = (TOther)(object)value.Value;
			return true;
		}

		if (typeof(TOther) == typeof(SignificantNumber))
		{
			result = (TOther)(object)value;
			return true;
		}

		result = default;
		return false;
	}
}
