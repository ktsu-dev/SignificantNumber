[assembly: CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.ComVisible(false)]
namespace ktsu.io.SignificantNumber;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;


/// <summary>
/// Represents a significant number.
/// </summary>
[DebuggerDisplay("{Significand}e{Exponent}")]
public readonly struct SignificantNumber
	: INumber<SignificantNumber>
{
	private const int Base10 = 10;

	/// <summary>
	/// Initializes a new instance of the <see cref="SignificantNumber"/> struct.
	/// </summary>
	/// <param name="exponent">The exponent of the number.</param>
	/// <param name="significand">The significand of the number.</param>
	/// <param name="sanitize">If true, trailing zeros in the significand will be removed.</param>
	internal SignificantNumber(int exponent, BigInteger significand, bool sanitize = true)
	{
		if (sanitize)
		{
			if (significand == 0)
			{
				Exponent = 0;
				Significand = 0;
				SignificantDigits = 0;
				return;
			}

			// remove trailing zeros
			while (significand != 0 && significand % Base10 == 0)
			{
				significand /= Base10;
				exponent++;
			}
		}

		// count digits
		int significantDigits = 0;
		var number = significand;
		while (number != 0)
		{
			significantDigits++;
			number /= Base10;
		}

		SignificantDigits = significantDigits;
		Exponent = exponent;
		Significand = significand;
	}

	/// <summary>
	/// Gets the value -1 for the type.
	/// </summary>
	public static SignificantNumber NegativeOne => new(0, -1);

	/// <inheritdoc/>
	public static SignificantNumber One => new(0, 1);

	/// <inheritdoc/>
	public static SignificantNumber Zero => new(0, 0);

	/// <summary>
	/// Gets the exponent of the significant number.
	/// </summary>
	internal int Exponent { get; }

	/// <summary>
	/// Gets the significand of the significant number.
	/// </summary>
	internal BigInteger Significand { get; }

	/// <summary>
	/// Gets the number of significant digits in the significant number.
	/// </summary>
	internal int SignificantDigits { get; }

	private static CultureInfo InvariantCulture { get; } = CultureInfo.InvariantCulture;

	private const int BinaryRadix = 2;

	/// <inheritdoc/>
	public static int Radix => BinaryRadix;

	/// <inheritdoc/>
	public static SignificantNumber AdditiveIdentity => Zero;

	/// <inheritdoc/>
	public static SignificantNumber MultiplicativeIdentity => One;

	/// <inheritdoc/>
	public override bool Equals(object? obj) => obj is SignificantNumber number && this == number;

	/// <summary>
	/// Determines whether the specified object is equal to the current object.
	/// </summary>
	/// <param name="other">The object to compare with the current object.</param>
	/// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
	public bool Equals(SignificantNumber other) => this == other;

	/// <inheritdoc/>
	public override int GetHashCode() => HashCode.Combine(Exponent, Significand);

	/// <inheritdoc/>
	public override string ToString() => ToString(this, null, null);

	/// <inheritdoc/>
	public string ToString(IFormatProvider? formatProvider) => ToString(this, null, formatProvider);

	/// <inheritdoc/>
	public string ToString(string format) => ToString(this, format, null);

	/// <summary>
	/// Converts the current instance to its equivalent string representation using the specified format and format provider.
	/// </summary>
	/// <param name="number">The significant number to convert.</param>
	/// <param name="format">A numeric format string.</param>
	/// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
	/// <returns>A string representation of the current instance.</returns>
	public static string ToString(SignificantNumber number, string? format, IFormatProvider? formatProvider)
	{
		int desiredAlloc = int.Abs(number.Exponent) + number.SignificantDigits + 2; // +2 is for negative symbol and decimal symbol
		int stackAlloc = Math.Min(desiredAlloc, 128);
		Span<char> buffer = stackAlloc == desiredAlloc
			? stackalloc char[stackAlloc]
			: new char[desiredAlloc];

		return number.TryFormat(buffer, out int charsWritten, format.AsSpan(), formatProvider)
			? buffer[..charsWritten].ToString()
			: string.Empty;
	}

	/// <inheritdoc/>
	public string ToString(string? format, IFormatProvider? formatProvider) => ToString(this, format, formatProvider);

	/// <summary>
	/// Returns the absolute value of the current instance.
	/// </summary>
	/// <returns>The absolute value of the current instance.</returns>
	public SignificantNumber Abs() => Abs(this);

	/// <summary>
	/// Rounds the current instance to the specified number of decimal digits.
	/// </summary>
	/// <param name="decimalDigits">The number of decimal digits to round to.</param>
	/// <returns>A new instance of <see cref="SignificantNumber"/> rounded to the specified number of decimal digits.</returns>
	public SignificantNumber Round(int decimalDigits)
	{
		int currentDecimalDigits = CountDecimalDigits();
		int decimalDifference = int.Abs(decimalDigits - currentDecimalDigits);
		if (decimalDifference > 0)
		{
			var roundingFactor = BigInteger.CopySign(CreateRepeatingDigits(5, decimalDifference), Significand);
			var newSignificand = (Significand + roundingFactor) / BigInteger.Pow(Base10, decimalDifference);
			int newExponent = Exponent - int.CopySign(decimalDifference, Exponent);
			return new SignificantNumber(newExponent, newSignificand);
		}

		return this;
	}

	/// <summary>
	/// Clamps the specified value between the minimum and maximum values.
	/// </summary>
	/// <typeparam name="TNumber">The type of the value to clamp.</typeparam>
	/// <param name="min">The minimum value.</param>
	/// <param name="max">The maximum value.</param>
	/// <returns>The clamped value.</returns>
	public SignificantNumber Clamp<TNumber>(TNumber min, TNumber max)
		where TNumber : INumber<TNumber>
	{
		var sigMin = min.ToSignificantNumber();
		var sigMax = max.ToSignificantNumber();
		var clampedToMax = this > sigMax ? sigMax : this;
		return this < sigMin ? sigMin : clampedToMax;
	}

	/// <summary>
	/// Creates a significant number from a floating point value.
	/// </summary>
	/// <typeparam name="TFloat">The type of the floating point value.</typeparam>
	/// <param name="input">The floating point value.</param>
	/// <returns>A significant number representing the floating point value.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the input is infinite or NaN.</exception>
	internal static SignificantNumber CreateFromFloatingPoint<TFloat>(TFloat input)
		where TFloat : INumber<TFloat>
	{
		ArgumentNullException.ThrowIfNull(input);

		if (TFloat.IsInfinity(input))
		{
			throw new ArgumentOutOfRangeException(nameof(input), "Infinite values are not supported");
		}

		if (TFloat.IsNaN(input))
		{
			throw new ArgumentOutOfRangeException(nameof(input), "NaN values are not supported");
		}

		AssertDoesImplementGenericInterface(typeof(TFloat), typeof(IFloatingPoint<>));

		bool isOne = input == TFloat.One;
		bool isNegativeOne = input == -TFloat.One;
		bool isZero = TFloat.IsZero(input);

		if (isZero)
		{
			return Zero;
		}

		if (isOne)
		{
			return One;
		}

		if (isNegativeOne)
		{
			return NegativeOne;
		}

		string format = GetStringFormatForFloatType<TFloat>();

		string significandString = input.ToString(format, InvariantCulture).ToUpperInvariant();
		var significandSpan = significandString.AsSpan();

		int exponentValue = 0;
		if (significandString.Contains('E', StringComparison.OrdinalIgnoreCase))
		{
			string[] expComponents = significandString.Split('E');
			Debug.Assert(expComponents.Length == 2, $"Unexpected format: {significandString}");
			significandSpan = expComponents[0].AsSpan();
			exponentValue = int.Parse(expComponents[1], InvariantCulture);
		}

		while (significandSpan.Length > 2 && significandSpan[^1] == '0')
		{
			significandSpan = significandSpan[..^1];
		}

		string[] components = significandSpan.ToString().Split('.');
		Debug.Assert(components.Length <= 2, $"Invalid format: {significandSpan}");

		var integerComponent = components[0].AsSpan();
		var fractionalComponent =
			components.Length == 2
			? components[1].AsSpan()
			: "0".AsSpan();
		int fractionalLength = fractionalComponent.Length;
		exponentValue -= fractionalLength;

		Debug.Assert(fractionalLength != 0 || integerComponent.TrimStart("-").Length == 1, $"Unexpected format: {integerComponent}.{fractionalComponent}");

		string significandStrWithoutDecimal = $"{integerComponent}{fractionalComponent}";
		var significandValue = BigInteger.Parse(significandStrWithoutDecimal, InvariantCulture);
		return new(exponentValue, significandValue);
	}

	internal static string GetStringFormatForFloatType<TFloat>()
		where TFloat : INumber<TFloat>
	{
		return typeof(TFloat) switch
		{
			_ when typeof(TFloat) == typeof(float) => "E7",
			_ when typeof(TFloat) == typeof(double) => "E15",
			_ => "R",
		};
	}

	/// <summary>
	/// Creates a significant number from an integer value.
	/// </summary>
	/// <typeparam name="TInteger">The type of the integer value.</typeparam>
	/// <param name="input">The integer value.</param>
	/// <returns>A significant number representing the integer value.</returns>
	internal static SignificantNumber CreateFromInteger<TInteger>(TInteger input)
		where TInteger : INumber<TInteger>
	{
		ArgumentNullException.ThrowIfNull(input);
		AssertDoesImplementGenericInterface(typeof(TInteger), typeof(IBinaryInteger<>));

		bool isOne = input == TInteger.One;
		bool isNegativeOne = TInteger.IsNegative(input) && input == -TInteger.One;
		bool isZero = TInteger.IsZero(input);

		if (isZero)
		{
			return Zero;
		}

		if (isOne)
		{
			return One;
		}

		if (isNegativeOne)
		{
			return NegativeOne;
		}

		int exponentValue = 0;
		var significandValue = BigInteger.CreateChecked(input);
		while (significandValue != 0 && significandValue % Base10 == 0)
		{
			significandValue /= Base10;
			exponentValue++;
		}

		return new(exponentValue, significandValue);
	}

	/// <summary>
	/// Creates a repeating digit sequence of a specified length.
	/// </summary>
	/// <param name="digit">The digit to repeat.</param>
	/// <param name="numberOfRepeats">The number of times to repeat the digit.</param>
	/// <returns>A <see cref="BigInteger"/> representing the repeating digit sequence.</returns>
	internal static BigInteger CreateRepeatingDigits(int digit, int numberOfRepeats)
	{
		if (numberOfRepeats <= 0)
		{
			return 0;
		}

		BigInteger repeatingDigit = digit;
		for (int i = 1; i < numberOfRepeats; i++)
		{
			repeatingDigit = (repeatingDigit * Base10) + digit;
		}

		return repeatingDigit;
	}

	/// <summary>
	/// Gets a value indicating whether the current instance has infinite precision.
	/// </summary>
	internal bool HasInfinitePrecision =>
		Exponent == 0
		&& (Significand == BigInteger.One || Significand == BigInteger.Zero || Significand == BigInteger.MinusOne);

	/// <summary>
	/// Gets the lower of the decimal digit counts of two significant numbers.
	/// </summary>
	/// <param name="left">The first significant number.</param>
	/// <param name="right">The second significant number.</param>
	/// <returns>The lower of the decimal digit counts of the two significant numbers.</returns>
	internal static int LowestDecimalDigits(SignificantNumber left, SignificantNumber right)
	{
		int leftDecimalDigits = left.CountDecimalDigits();
		int rightDecimalDigits = right.CountDecimalDigits();

		leftDecimalDigits = left.HasInfinitePrecision ? rightDecimalDigits : leftDecimalDigits;
		rightDecimalDigits = right.HasInfinitePrecision ? leftDecimalDigits : rightDecimalDigits;

		return leftDecimalDigits < rightDecimalDigits
			? leftDecimalDigits
			: rightDecimalDigits;
	}

	/// <summary>
	/// Gets the lower of the significant digit counts of two significant numbers.
	/// </summary>
	/// <param name="left">The first significant number.</param>
	/// <param name="right">The second significant number.</param>
	/// <returns>The lower of the significant digit counts of the two significant numbers.</returns>
	internal static int LowestSignificantDigits(SignificantNumber left, SignificantNumber right)
	{
		int leftSignificantDigits = left.SignificantDigits;
		int rightSignificantDigits = right.SignificantDigits;

		leftSignificantDigits = left.HasInfinitePrecision ? rightSignificantDigits : leftSignificantDigits;
		rightSignificantDigits = right.HasInfinitePrecision ? leftSignificantDigits : rightSignificantDigits;

		return leftSignificantDigits < rightSignificantDigits
		? leftSignificantDigits
		: rightSignificantDigits;
	}

	/// <summary>
	/// Counts the number of decimal digits in the current instance.
	/// </summary>
	/// <returns>The number of decimal digits in the current instance.</returns>
	internal int CountDecimalDigits() =>
		Exponent > 0
		? 0
		: int.Abs(Exponent);

	/// <summary>
	/// Reduces the significance of the current instance to a specified number of significant digits.
	/// </summary>
	/// <param name="significantDigits">The number of significant digits to reduce to.</param>
	/// <returns>A new instance of <see cref="SignificantNumber"/> reduced to the specified number of significant digits.</returns>
	internal SignificantNumber ReduceSignificance(int significantDigits)
	{
		int significantDifference = significantDigits < SignificantDigits
			? SignificantDigits - significantDigits
			: 0;

		if (significantDifference == 0)
		{
			return this;
		}

		int newExponent = Exponent == 0
			? significantDifference
			: Exponent + significantDifference;
		var roundingFactor = BigInteger.CopySign(CreateRepeatingDigits(5, significantDifference), Significand);
		var newSignificand = (Significand + roundingFactor) / BigInteger.Pow(Base10, significantDifference);
		return new(newExponent, newSignificand);
	}

	/// <summary>
	/// Makes two significant numbers have a common exponent.
	/// </summary>
	/// <param name="left">The first significant number.</param>
	/// <param name="right">The second significant number.</param>
	internal static void MakeCommonized(ref SignificantNumber left, ref SignificantNumber right) =>
		_ = MakeCommonizedAndGetExponent(ref left, ref right);

	/// <summary>
	/// Makes two significant numbers have a common exponent and returns the common exponent.
	/// </summary>
	/// <param name="left">The first significant number.</param>
	/// <param name="right">The second significant number.</param>
	/// <returns>The common exponent.</returns>
	internal static int MakeCommonizedAndGetExponent(ref SignificantNumber left, ref SignificantNumber right)
	{
		int smallestExponent = left.Exponent < right.Exponent ? left.Exponent : right.Exponent;
		int exponentDifferenceLeft = Math.Abs(left.Exponent - smallestExponent);
		int exponentDifferenceRight = Math.Abs(right.Exponent - smallestExponent);
		var newSignificandLeft = left.Significand * BigInteger.Pow(Base10, exponentDifferenceLeft);
		var newSignificandRight = right.Significand * BigInteger.Pow(Base10, exponentDifferenceRight);

		left = new(smallestExponent, newSignificandLeft, sanitize: false);
		right = new(smallestExponent, newSignificandRight, sanitize: false);

		return smallestExponent;
	}

	/// <inheritdoc/>
	public int CompareTo(object? obj)
	{
		return obj is SignificantNumber significantNumber
			? CompareTo(significantNumber)
			: throw new NotSupportedException();
	}

	/// <summary>
	/// Compares the current instance with another significant number.
	/// </summary>
	/// <param name="other">The significant number to compare with the current instance.</param>
	/// <returns>A value indicating whether the current instance is less than, equal to, or greater than the other instance.</returns>
	public int CompareTo(SignificantNumber other)
	{
		int greaterOrEqual = this > other ? 1 : 0;
		return this < other ? -1 : greaterOrEqual;
	}

	/// <summary>
	/// Compares the current instance with another number.
	/// </summary>
	/// <typeparam name="TInput">The type of the other number.</typeparam>
	/// <param name="other">The number to compare with the current instance.</param>
	/// <returns>A value indicating whether the current instance is less than, equal to, or greater than the other number.</returns>
	public int CompareTo<TInput>(TInput other)
		where TInput : INumber<TInput>
	{
		var significantOther = other.ToSignificantNumber();
		int greaterOrEqual = this > significantOther ? 1 : 0;
		return this < significantOther ? -1 : greaterOrEqual;
	}

	/// <inheritdoc/>
	public static SignificantNumber Abs(SignificantNumber value) => value.Significand < 0 ? -value : value;

	/// <inheritdoc/>
	public static bool IsCanonical(SignificantNumber value) => true;

	/// <inheritdoc/>
	public static bool IsComplexNumber(SignificantNumber value) => !IsRealNumber(value);

	/// <inheritdoc/>
	public static bool IsEvenInteger(SignificantNumber value) => IsInteger(value) && value.Significand.IsEven;

	/// <inheritdoc/>
	public static bool IsFinite(SignificantNumber value) => true;

	/// <inheritdoc/>
	public static bool IsImaginaryNumber(SignificantNumber value) => !IsRealNumber(value);

	/// <inheritdoc/>
	public static bool IsInfinity(SignificantNumber value) => !IsFinite(value);

	/// <inheritdoc/>
	public static bool IsInteger(SignificantNumber value) => value.Exponent >= 0;

	/// <inheritdoc/>
	public static bool IsNaN(SignificantNumber value) => false;

	/// <inheritdoc/>
	public static bool IsNegative(SignificantNumber value) => !IsPositive(value);

	/// <inheritdoc/>
	public static bool IsNegativeInfinity(SignificantNumber value) => IsInfinity(value) && IsNegative(value);

	/// <summary>
	/// Determines whether the specified value is normal.
	/// </summary>
	/// <param name="value">The significant number.</param>
	/// <returns><c>true</c> if the specified value is normal; otherwise, <c>false</c>.</returns>
	public static bool IsNormal(SignificantNumber value) => true;

	/// <inheritdoc/>
	public static bool IsOddInteger(SignificantNumber value) => IsInteger(value) && !value.Significand.IsEven;

	/// <inheritdoc/>
	public static bool IsPositive(SignificantNumber value) => value.Significand >= 0;

	/// <inheritdoc/>
	public static bool IsPositiveInfinity(SignificantNumber value) => IsInfinity(value) && IsPositive(value);

	/// <inheritdoc/>
	public static bool IsRealNumber(SignificantNumber value) => true;

	/// <inheritdoc/>
	public static bool IsSubnormal(SignificantNumber value) => !IsNormal(value);

	/// <inheritdoc/>
	public static bool IsZero(SignificantNumber value) => value.Significand == 0;

	/// <inheritdoc/>
	public static SignificantNumber MaxMagnitude(SignificantNumber x, SignificantNumber y) => x.Abs() >= y.Abs() ? x : y;

	/// <inheritdoc/>
	public static SignificantNumber MaxMagnitudeNumber(SignificantNumber x, SignificantNumber y) => MaxMagnitude(x, y);

	/// <inheritdoc/>
	public static SignificantNumber MinMagnitude(SignificantNumber x, SignificantNumber y) => x.Abs() <= y.Abs() ? x : y;

	/// <inheritdoc/>
	public static SignificantNumber MinMagnitudeNumber(SignificantNumber x, SignificantNumber y) => MinMagnitude(x, y);

	/// <inheritdoc/>
	public static SignificantNumber Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => throw new NotSupportedException();

	/// <inheritdoc/>
	public static SignificantNumber Parse(string s, NumberStyles style, IFormatProvider? provider) => throw new NotSupportedException();

	/// <inheritdoc/>
	public static SignificantNumber Parse(string s, IFormatProvider? provider) => throw new NotSupportedException();

	/// <inheritdoc/>
	public static SignificantNumber Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => throw new NotSupportedException();

	/// <inheritdoc/>
	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result) => throw new NotSupportedException();

	/// <inheritdoc/>
	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result) => throw new NotSupportedException();

	/// <inheritdoc/>
	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result) => throw new NotSupportedException();

	/// <inheritdoc/>
	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result) => throw new NotSupportedException();

	/// <inheritdoc/>
	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		int requiredLength = SignificantDigits + Exponent + 2;

		if (destination.Length < requiredLength)
		{
			charsWritten = 0;
			return false;
		}

		if (!format.IsEmpty && !format.Equals("G", StringComparison.OrdinalIgnoreCase))
		{
			throw new FormatException();
		}

		destination.Clear();

		string output;

		provider ??= InvariantCulture;
		var numberFormat = NumberFormatInfo.GetInstance(provider);

		if (this == Zero)
		{
			output = "0";
		}
		else if (this == One)
		{
			output = "1";
		}
		else if (this == NegativeOne)
		{
			output = $"{numberFormat.NegativeSign}1";
		}
		else
		{

			string sign = Significand < 0 ? numberFormat.NegativeSign : string.Empty;
			string significandStr = BigInteger.Abs(Significand).ToString(InvariantCulture);

			if (Exponent == 0)
			{
				output = $"{sign}{significandStr}";
			}
			else if (Exponent > 0)
			{
				int desiredAlloc = Exponent;
				int stackAlloc = Math.Min(desiredAlloc, 128);
				Span<char> trainlingZeroes = stackAlloc == desiredAlloc
					? stackalloc char[stackAlloc]
					: new char[desiredAlloc];

				trainlingZeroes.Fill('0');
				output = $"{sign}{significandStr}{trainlingZeroes}";
			}
			else
			{

				int absExponent = -Exponent;

				string integralComponent = absExponent >= significandStr.Length
					? "0"
					: significandStr[..^absExponent];

				int desiredAlloc = Math.Max(absExponent - significandStr.Length, 0);
				int stackAlloc = Math.Min(desiredAlloc, 128);
				Span<char> fractionalZeroes = stackAlloc == desiredAlloc
					? stackalloc char[stackAlloc]
					: new char[desiredAlloc];

				fractionalZeroes.Fill('0');

				string fractionalComponent = absExponent >= significandStr.Length
					? $"{fractionalZeroes}{BigInteger.Abs(Significand)}"
					: significandStr[^absExponent..];

				output = $"{sign}{integralComponent}{numberFormat.NumberDecimalSeparator}{fractionalComponent}";
			}
		}

		bool success = output.TryCopyTo(destination);

		charsWritten = success ? output.Length : 0;
		return success;
	}

	/// <inheritdoc/>
	public static bool TryConvertFromChecked<TOther>(TOther value, out SignificantNumber result)
		where TOther : INumberBase<TOther>
		=> throw new NotSupportedException();

	/// <inheritdoc/>
	public static bool TryConvertFromSaturating<TOther>(TOther value, out SignificantNumber result)
		where TOther : INumberBase<TOther>
		=> throw new NotSupportedException();

	/// <inheritdoc/>
	public static bool TryConvertFromTruncating<TOther>(TOther value, out SignificantNumber result)
		where TOther : INumberBase<TOther>
		=> throw new NotSupportedException();

	/// <inheritdoc/>
	public static bool TryConvertToChecked<TOther>(SignificantNumber value, out TOther result)
		where TOther : INumberBase<TOther>
		=> throw new NotSupportedException();

	/// <inheritdoc/>
	public static bool TryConvertToSaturating<TOther>(SignificantNumber value, out TOther result)
		where TOther : INumberBase<TOther>
		=> throw new NotSupportedException();

	/// <inheritdoc/>
	public static bool TryConvertToTruncating<TOther>(SignificantNumber value, out TOther result)
		where TOther : INumberBase<TOther>
		=> throw new NotSupportedException();

	/// <summary>
	/// Asserts that the exponents of two significant numbers match.
	/// </summary>
	/// <param name="left">The first significant number.</param>
	/// <param name="right">The second significant number.</param>
	internal static void AssertExponentsMatch(SignificantNumber left, SignificantNumber right) =>
		Debug.Assert(left.Exponent == right.Exponent, $"{nameof(AssertExponentsMatch)}: {left.Exponent} == {right.Exponent}");

	/// <summary>
	/// Negates a significant number.
	/// </summary>
	/// <param name="value">The significant number to negate.</param>
	/// <returns>The negated significant number.</returns>
	public static SignificantNumber Negate(SignificantNumber value) => -value;

	/// <summary>
	/// Subtracts one significant number from another.
	/// </summary>
	/// <param name="left">The significant number to subtract from.</param>
	/// <param name="right">The significant number to subtract.</param>
	/// <returns>The result of the subtraction.</returns>
	public static SignificantNumber Subtract(SignificantNumber left, SignificantNumber right) => left - right;

	/// <summary>
	/// Adds two significant numbers.
	/// </summary>
	/// <param name="left">The first significant number to add.</param>
	/// <param name="right">The second significant number to add.</param>
	/// <returns>The result of the addition.</returns>
	public static SignificantNumber Add(SignificantNumber left, SignificantNumber right) => left + right;

	/// <summary>
	/// Multiplies two significant numbers.
	/// </summary>
	/// <param name="left">The first significant number to multiply.</param>
	/// <param name="right">The second significant number to multiply.</param>
	/// <returns>The result of the multiplication.</returns>
	public static SignificantNumber Multiply(SignificantNumber left, SignificantNumber right) => left * right;

	/// <summary>
	/// Divides one significant number by another.
	/// </summary>
	/// <param name="left">The significant number to divide.</param>
	/// <param name="right">The significant number to divide by.</param>
	/// <returns>The result of the division.</returns>
	public static SignificantNumber Divide(SignificantNumber left, SignificantNumber right) => left / right;

	/// <summary>
	/// Increments a significant number.
	/// </summary>
	/// <param name="value">The significant number to increment.</param>
	/// <returns>The incremented significant number.</returns>
	/// <exception cref="NotSupportedException">Incrementing is not supported.</exception>
	public static SignificantNumber Increment(SignificantNumber value) => throw new NotSupportedException();

	/// <summary>
	/// Decrements a significant number.
	/// </summary>
	/// <param name="value">The significant number to decrement.</param>
	/// <returns>The decremented significant number.</returns>
	/// <exception cref="NotSupportedException">Decrementing is not supported.</exception>
	public static SignificantNumber Decrement(SignificantNumber value) => throw new NotSupportedException();

	/// <summary>
	/// Returns the unary plus of a significant number.
	/// </summary>
	/// <param name="value">The significant number.</param>
	/// <returns>The unary plus of the significant number.</returns>
	public static SignificantNumber Plus(SignificantNumber value) => +value;

	/// <summary>
	/// Computes the modulus of two significant numbers.
	/// </summary>
	/// <param name="left">The significant number to divide.</param>
	/// <param name="right">The significant number to divide by.</param>
	/// <returns>The modulus of the two significant numbers.</returns>
	/// <exception cref="NotSupportedException">Modulus operation is not supported.</exception>
	public static SignificantNumber Mod(SignificantNumber left, SignificantNumber right) => throw new NotSupportedException();

	/// <summary>
	/// Determines whether one significant number is greater than another.
	/// </summary>
	/// <param name="left">The first significant number.</param>
	/// <param name="right">The second significant number.</param>
	/// <returns><c>true</c> if the first significant number is greater than the second; otherwise, <c>false</c>.</returns>
	public static bool GreaterThan(SignificantNumber left, SignificantNumber right) => left > right;

	/// <summary>
	/// Determines whether one significant number is greater than or equal to another.
	/// </summary>
	/// <param name="left">The first significant number.</param>
	/// <param name="right">The second significant number.</param>
	/// <returns><c>true</c> if the first significant number is greater than or equal to the second; otherwise, <c>false</c>.</returns>
	public static bool GreaterThanOrEqual(SignificantNumber left, SignificantNumber right) => left >= right;

	/// <summary>
	/// Determines whether one significant number is less than another.
	/// </summary>
	/// <param name="left">The first significant number.</param>
	/// <param name="right">The second significant number.</param>
	/// <returns><c>true</c> if the first significant number is less than the second; otherwise, <c>false</c>.</returns>
	public static bool LessThan(SignificantNumber left, SignificantNumber right) => left < right;

	/// <summary>
	/// Determines whether one significant number is less than or equal to another.
	/// </summary>
	/// <param name="left">The first significant number.</param>
	/// <param name="right">The second significant number.</param>
	/// <returns><c>true</c> if the first significant number is less than or equal to the second; otherwise, <c>false</c>.</returns>
	public static bool LessThanOrEqual(SignificantNumber left, SignificantNumber right) => left <= right;

	/// <summary>
	/// Determines whether two significant numbers are equal.
	/// </summary>
	/// <param name="left">The first significant number.</param>
	/// <param name="right">The second significant number.</param>
	/// <returns><c>true</c> if the two significant numbers are equal; otherwise, <c>false</c>.</returns>
	public static bool Equal(SignificantNumber left, SignificantNumber right) => left == right;

	/// <summary>
	/// Determines whether two significant numbers are not equal.
	/// </summary>
	/// <param name="left">The first significant number.</param>
	/// <param name="right">The second significant number.</param>
	/// <returns><c>true</c> if the two significant numbers are not equal; otherwise, <c>false</c>.</returns>
	public static bool NotEqual(SignificantNumber left, SignificantNumber right) => left != right;

	/// <summary>
	/// Returns the larger of two significant numbers.
	/// </summary>
	/// <param name="x">The first significant number.</param>
	/// <param name="y">The second significant number.</param>
	/// <returns>The larger of the two significant numbers.</returns>
	public static SignificantNumber Max(SignificantNumber x, SignificantNumber y) => x > y ? x : y;

	/// <summary>
	/// Returns the smaller of two significant numbers.
	/// </summary>
	/// <param name="x">The first significant number.</param>
	/// <param name="y">The second significant number.</param>
	/// <returns>The smaller of the two significant numbers.</returns>
	public static SignificantNumber Min(SignificantNumber x, SignificantNumber y) => x < y ? x : y;

	/// <summary>
	/// Clamps a significant number to the specified minimum and maximum values.
	/// </summary>
	/// <param name="value">The significant number to clamp.</param>
	/// <param name="min">The minimum value.</param>
	/// <param name="max">The maximum value.</param>
	/// <returns>The clamped significant number.</returns>
	public static SignificantNumber Clamp(SignificantNumber value, SignificantNumber min, SignificantNumber max) => value.Clamp(min, max);

	/// <summary>
	/// Rounds a significant number to the specified number of decimal digits.
	/// </summary>
	/// <param name="value">The significant number to round.</param>
	/// <param name="decimalDigits">The number of decimal digits to round to.</param>
	/// <returns>The rounded significant number.</returns>
	public static SignificantNumber Round(SignificantNumber value, int decimalDigits) => value.Round(decimalDigits);

	/// <inheritdoc/>
	public static SignificantNumber operator -(SignificantNumber value)
	{
		return value == Zero
			? value
			: new(value.Exponent, -value.Significand);
	}

	/// <inheritdoc/>
	public static SignificantNumber operator -(SignificantNumber left, SignificantNumber right)
	{
		int decimalDigits = LowestDecimalDigits(left, right);
		int commonExponent = MakeCommonizedAndGetExponent(ref left, ref right);
		AssertExponentsMatch(left, left);

		var newSignificand = left.Significand - right.Significand;
		return new SignificantNumber(commonExponent, newSignificand).Round(decimalDigits);
	}

	/// <inheritdoc/>
	public static bool operator !=(SignificantNumber left, SignificantNumber right) => !(left == right);

	/// <inheritdoc/>
	public static SignificantNumber operator *(SignificantNumber left, SignificantNumber right)
	{
		int significantDigits = LowestSignificantDigits(left, right);
		int commonExponent = MakeCommonizedAndGetExponent(ref left, ref right);
		AssertExponentsMatch(left, right);

		var newSignificand = left.Significand * right.Significand;
		int newExponent = commonExponent * 2;
		return new SignificantNumber(newExponent, newSignificand).ReduceSignificance(significantDigits);
	}

	/// <inheritdoc/>
	public static SignificantNumber operator /(SignificantNumber left, SignificantNumber right)
	{
		int significantDigits = LowestSignificantDigits(left, right);
		int commonExponent = MakeCommonizedAndGetExponent(ref left, ref right);
		AssertExponentsMatch(left, right);

		var newSignificand = left.Significand * BigInteger.Pow(Base10, int.Abs(commonExponent)) / right.Significand;
		return new SignificantNumber(commonExponent, newSignificand).ReduceSignificance(significantDigits);
	}

	/// <inheritdoc/>
	public static SignificantNumber operator +(SignificantNumber value) => value;

	/// <inheritdoc/>
	public static SignificantNumber operator +(SignificantNumber left, SignificantNumber right)
	{
		int decimalDigits = LowestDecimalDigits(left, right);
		int commonExponent = MakeCommonizedAndGetExponent(ref left, ref right);
		AssertExponentsMatch(left, right);

		var newSignificand = left.Significand + right.Significand;
		return new SignificantNumber(commonExponent, newSignificand).Round(decimalDigits);
	}

	/// <inheritdoc/>
	public static bool operator ==(SignificantNumber left, SignificantNumber right)
	{
		int decimalDigits = LowestDecimalDigits(left, right);
		MakeCommonized(ref left, ref right);
		AssertExponentsMatch(left, right);
		var leftSignificant = left.Round(decimalDigits);
		var rightSignificant = right.Round(decimalDigits);
		MakeCommonized(ref leftSignificant, ref rightSignificant);
		AssertExponentsMatch(leftSignificant, rightSignificant);
		return leftSignificant.Significand == rightSignificant.Significand;
	}

	/// <inheritdoc/>
	public static bool operator >(SignificantNumber left, SignificantNumber right)
	{
		int decimalDigits = LowestDecimalDigits(left, right);
		MakeCommonized(ref left, ref right);
		AssertExponentsMatch(left, right);
		var leftSignificant = left.Round(decimalDigits);
		var rightSignificant = right.Round(decimalDigits);
		MakeCommonized(ref leftSignificant, ref rightSignificant);
		AssertExponentsMatch(leftSignificant, rightSignificant);
		return leftSignificant.Significand > rightSignificant.Significand;
	}

	/// <inheritdoc/>
	public static bool operator <(SignificantNumber left, SignificantNumber right)
	{
		int decimalDigits = LowestDecimalDigits(left, right);
		MakeCommonized(ref left, ref right);
		AssertExponentsMatch(left, right);
		var leftSignificant = left.Round(decimalDigits);
		var rightSignificant = right.Round(decimalDigits);
		MakeCommonized(ref leftSignificant, ref rightSignificant);
		AssertExponentsMatch(leftSignificant, rightSignificant);
		return leftSignificant.Significand < rightSignificant.Significand;
	}

	/// <inheritdoc/>
	public static bool operator >=(SignificantNumber left, SignificantNumber right)
	{
		int decimalDigits = LowestDecimalDigits(left, right);
		MakeCommonized(ref left, ref right);
		AssertExponentsMatch(left, right);
		var leftSignificant = left.Round(decimalDigits);
		var rightSignificant = right.Round(decimalDigits);
		MakeCommonized(ref leftSignificant, ref rightSignificant);
		AssertExponentsMatch(leftSignificant, rightSignificant);
		return leftSignificant.Significand >= rightSignificant.Significand;
	}

	/// <inheritdoc/>
	public static bool operator <=(SignificantNumber left, SignificantNumber right)
	{
		int decimalDigits = LowestDecimalDigits(left, right);
		MakeCommonized(ref left, ref right);
		AssertExponentsMatch(left, right);
		var leftSignificant = left.Round(decimalDigits);
		var rightSignificant = right.Round(decimalDigits);
		MakeCommonized(ref leftSignificant, ref rightSignificant);
		AssertExponentsMatch(leftSignificant, rightSignificant);
		return leftSignificant.Significand <= rightSignificant.Significand;
	}

	/// <inheritdoc/>
	public static SignificantNumber operator %(SignificantNumber left, SignificantNumber right) => throw new NotSupportedException();

	/// <inheritdoc/>
	public static SignificantNumber operator --(SignificantNumber value) => throw new NotSupportedException();

	/// <inheritdoc/>
	public static SignificantNumber operator ++(SignificantNumber value) => throw new NotSupportedException();

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
	/// Converts the current significant number to the specified numeric type.
	/// </summary>
	/// <typeparam name="TOutput">The type to convert to. Must implement <see cref="INumber{TOutput}"/>.</typeparam>
	/// <returns>The converted value of the significant number as type <typeparamref name="TOutput"/>.</returns>
	/// <exception cref="OverflowException">
	/// Thrown if the conversion cannot be performed. This may occur if the target type cannot represent
	/// the value of the significant number.
	/// </exception>
	public TOutput To<TOutput>()
		where TOutput : INumber<TOutput> =>
		TOutput.CreateChecked(Significand) * TOutput.CreateChecked(Math.Pow(Base10, Exponent));

}
