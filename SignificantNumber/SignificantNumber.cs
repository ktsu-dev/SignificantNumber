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
	internal const int MaxDecimalPlaces = 15;
	internal const int MinDecimalPlaces = 1;
	private const string FormatSpecifier = "e15";

	internal SignificantNumber(int exponent, BigInteger significand, bool sanitize = true)
	{
		const int ten = 10;

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
			while (significand != 0 && significand % ten == 0)
			{
				significand /= ten;
				exponent++;
			}
		}

		//count digits
		int significantDigits = 0;
		var number = significand;
		while (number != 0)
		{
			significantDigits++;
			number /= ten;
		}

		SignificantDigits = significantDigits;
		Exponent = exponent;
		Significand = significand;
	}

	/// <summary>
	/// Gets the value -1 for the type.
	/// </summary>
	public static SignificantNumber NegativeOne => new(0, -1);

	/// <summary>
	/// Gets the value 1 for the type.
	/// </summary>
	public static SignificantNumber One => new(0, 1);

	/// <summary>
	/// Gets the value 0 for the type.
	/// </summary>
	public static SignificantNumber Zero => new(0, 0);

	internal int Exponent { get; }

	internal BigInteger Significand { get; }

	internal int SignificantDigits { get; }

	private static CultureInfo InvariantCulture { get; } = CultureInfo.InvariantCulture;

	private const int BinaryRadix = 2;

	/// <summary>
	/// Gets the radix, or base, for the type.
	/// </summary>
	public static int Radix => BinaryRadix;

	/// <summary>
	/// Gets the additive identity of the current type.
	/// </summary>
	public static SignificantNumber AdditiveIdentity => Zero;

	/// <summary>
	/// Gets the multiplicative identity of the current type.
	/// </summary>
	public static SignificantNumber MultiplicativeIdentity => One;

	/// <summary>Determines whether the specified object is equal to the current object.</summary>
	/// <param name="obj">The object to compare with the current object.</param>
	/// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
	public override bool Equals(object? obj) => obj is SignificantNumber number && this == number;

	/// <summary>Determines whether the specified object is equal to the current object.</summary>
	/// <param name="other">The object to compare with the current object.</param>
	/// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
	public bool Equals(SignificantNumber other) => this == other;

	/// <summary>Serves as the default hash function.</summary>
	/// <returns>A hash code for the current object.</returns>
	public override int GetHashCode() => HashCode.Combine(Exponent, Significand);

	/// <summary>Returns a string that represents the current object.</summary>
	/// <returns>A string that represents the current object.</returns>
	public override string ToString() => ToString(this, null, null);

	/// <summary>
	/// Converts the current instance to its equivalent string representation using the specified format provider.
	/// </summary>
	/// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
	/// <returns>A string representation of the current instance.</returns>
	public string ToString(IFormatProvider? formatProvider) => ToString(this, null, formatProvider);

	/// <summary>Returns a string that represents the current object.</summary>
	/// <returns>A string that represents the current object.</returns>
	public string ToString(string format) => ToString(this, format, null);

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

	public string ToString(string? format, IFormatProvider? formatProvider) => ToString(this, format, formatProvider);

	public SignificantNumber Abs() => Abs(this);

	public SignificantNumber Round(int decimalDigits)
	{
		int currentDecimalDigits = CountDecimalDigits();
		int decimalDifference = int.Abs(decimalDigits - currentDecimalDigits);
		if (decimalDifference > 0)
		{
			var roundingFactor = BigInteger.CopySign(CreateRepeatingDigits(5, decimalDifference), Significand);
			var newSignificand = (Significand + roundingFactor) / BigInteger.Pow(10, decimalDifference);
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

		string str = input.ToString(FormatSpecifier, InvariantCulture);
		int indexOfE = str.IndexOf('e');
		Debug.Assert(indexOfE > -1, $"Exponent delimiter not found in: {str}");

		var significandStr = str.AsSpan(0, indexOfE);
		var exponentStr = str.AsSpan(indexOfE + 1, str.Length - indexOfE - 1);
		int exponentValue = int.Parse(exponentStr, InvariantCulture);

		while (significandStr.Length > 2 && significandStr[^1] == '0')
		{
			significandStr = significandStr[..^1];
		}

		string[] components = significandStr.ToString().Split('.');
		Debug.Assert(components.Length == 2, $"Missing decimal separator in: {significandStr}");

		var integerComponent = components[0].AsSpan();
		var fractionalComponent = components[1].AsSpan();
		int fractionalLength = fractionalComponent.Length;
		exponentValue -= fractionalLength;


		Debug.Assert(fractionalLength != 0 || integerComponent.TrimStart("-").Length == 1, $"Unexpected format: {integerComponent}.{fractionalComponent}");

		string significandStrWithoutDecimal = $"{integerComponent}{fractionalComponent}";
		var significandValue = BigInteger.Parse(significandStrWithoutDecimal, InvariantCulture);
		return new(exponentValue, significandValue);
	}

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
		const int ten = 10;
		while (significandValue != 0 && significandValue % ten == 0)
		{
			significandValue /= ten;
			exponentValue++;
		}

		return new(exponentValue, significandValue);
	}

	internal static BigInteger CreateRepeatingDigits(int digit, int numberOfRepeats)
	{
		if (numberOfRepeats <= 0)
		{
			return 0;
		}

		const int ten = 10;

		BigInteger repeatingDigit = digit;
		for (int i = 1; i < numberOfRepeats; i++)
		{
			repeatingDigit = (repeatingDigit * ten) + digit;
		}

		return repeatingDigit;
	}

	internal bool HasInfinitePrecision =>
		Exponent == 0
		&& (Significand == BigInteger.One || Significand == BigInteger.Zero || Significand == BigInteger.MinusOne);

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

	internal int CountDecimalDigits() =>
		Exponent > 0
		? 0
		: int.Abs(Exponent);

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
		var newSignificand = (Significand + roundingFactor) / BigInteger.Pow(10, significantDifference);
		return new(newExponent, newSignificand);
	}

	internal static void MakeCommonized(ref SignificantNumber left, ref SignificantNumber right) =>
		_ = MakeCommonizedAndGetExponent(ref left, ref right);

	internal static int MakeCommonizedAndGetExponent(ref SignificantNumber left, ref SignificantNumber right)
	{
		int smallestExponent = left.Exponent < right.Exponent ? left.Exponent : right.Exponent;
		int exponentDifferenceLeft = Math.Abs(left.Exponent - smallestExponent);
		int exponentDifferenceRight = Math.Abs(right.Exponent - smallestExponent);
		var newSignificandLeft = left.Significand * BigInteger.Pow(10, exponentDifferenceLeft);
		var newSignificandRight = right.Significand * BigInteger.Pow(10, exponentDifferenceRight);

		left = new(smallestExponent, newSignificandLeft, sanitize: false);
		right = new(smallestExponent, newSignificandRight, sanitize: false);

		return smallestExponent;
	}

	public int CompareTo(object? obj)
	{
		return obj is SignificantNumber significantNumber
			? CompareTo(significantNumber)
			: throw new NotSupportedException();
	}

	public int CompareTo(SignificantNumber other)
	{
		int greaterOrEqual = this > other ? 1 : 0;
		return this < other ? -1 : greaterOrEqual;
	}

	public int CompareTo<TInput>(TInput other)
		where TInput : INumber<TInput>
	{
		var significantOther = other.ToSignificantNumber();
		int greaterOrEqual = this > significantOther ? 1 : 0;
		return this < significantOther ? -1 : greaterOrEqual;
	}

	public static SignificantNumber Abs(SignificantNumber value) => value.Significand < 0 ? -value : value;
	public static bool IsCanonical(SignificantNumber value) => true;
	public static bool IsComplexNumber(SignificantNumber value) => !IsRealNumber(value);
	public static bool IsEvenInteger(SignificantNumber value) => IsInteger(value) && value.Significand.IsEven;
	public static bool IsFinite(SignificantNumber value) => true;
	public static bool IsImaginaryNumber(SignificantNumber value) => !IsRealNumber(value);
	public static bool IsInfinity(SignificantNumber value) => !IsFinite(value);
	public static bool IsInteger(SignificantNumber value) => value.Exponent >= 0;
	public static bool IsNaN(SignificantNumber value) => false;
	public static bool IsNegative(SignificantNumber value) => !IsPositive(value);
	public static bool IsNegativeInfinity(SignificantNumber value) => IsInfinity(value) && IsNegative(value);
	public static bool IsNormal(SignificantNumber value) => true;
	public static bool IsOddInteger(SignificantNumber value) => IsInteger(value) && !value.Significand.IsEven;
	public static bool IsPositive(SignificantNumber value) => value.Significand >= 0;
	public static bool IsPositiveInfinity(SignificantNumber value) => IsInfinity(value) && IsPositive(value);
	public static bool IsRealNumber(SignificantNumber value) => true;
	public static bool IsSubnormal(SignificantNumber value) => !IsNormal(value);
	public static bool IsZero(SignificantNumber value) => value.Significand == 0;
	public static SignificantNumber MaxMagnitude(SignificantNumber x, SignificantNumber y) => x.Abs() >= y.Abs() ? x : y;
	public static SignificantNumber MaxMagnitudeNumber(SignificantNumber x, SignificantNumber y) => MaxMagnitude(x, y);
	public static SignificantNumber MinMagnitude(SignificantNumber x, SignificantNumber y) => x.Abs() <= y.Abs() ? x : y;
	public static SignificantNumber MinMagnitudeNumber(SignificantNumber x, SignificantNumber y) => MinMagnitude(x, y);
	public static SignificantNumber Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => throw new NotSupportedException();
	public static SignificantNumber Parse(string s, NumberStyles style, IFormatProvider? provider) => throw new NotSupportedException();
	public static SignificantNumber Parse(string s, IFormatProvider? provider) => throw new NotSupportedException();
	public static SignificantNumber Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => throw new NotSupportedException();
	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result) => throw new NotSupportedException();
	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result) => throw new NotSupportedException();
	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result) => throw new NotSupportedException();
	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result) => throw new NotSupportedException();
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

	public static bool TryConvertFromChecked<TOther>(TOther value, out SignificantNumber result)
		where TOther : INumberBase<TOther>
		=> throw new NotSupportedException();
	public static bool TryConvertFromSaturating<TOther>(TOther value, out SignificantNumber result)
		where TOther : INumberBase<TOther>
		=> throw new NotSupportedException();
	public static bool TryConvertFromTruncating<TOther>(TOther value, out SignificantNumber result)
		where TOther : INumberBase<TOther>
		=> throw new NotSupportedException();
	public static bool TryConvertToChecked<TOther>(SignificantNumber value, out TOther result)
		where TOther : INumberBase<TOther>
		=> throw new NotSupportedException();
	public static bool TryConvertToSaturating<TOther>(SignificantNumber value, out TOther result)
		where TOther : INumberBase<TOther>
		=> throw new NotSupportedException();
	public static bool TryConvertToTruncating<TOther>(SignificantNumber value, out TOther result)
		where TOther : INumberBase<TOther>
		=> throw new NotSupportedException();

	internal static void AssertExponentsMatch(SignificantNumber left, SignificantNumber right) =>
		Debug.Assert(left.Exponent == right.Exponent, $"{nameof(AssertExponentsMatch)}: {left.Exponent} == {right.Exponent}");

	public static SignificantNumber Negate(SignificantNumber value) => -value;
	public static SignificantNumber Subtract(SignificantNumber left, SignificantNumber right) => left - right;
	public static SignificantNumber Add(SignificantNumber left, SignificantNumber right) => left + right;
	public static SignificantNumber Multiply(SignificantNumber left, SignificantNumber right) => left * right;
	public static SignificantNumber Divide(SignificantNumber left, SignificantNumber right) => left / right;
	public static SignificantNumber Increment(SignificantNumber value) => throw new NotSupportedException();
	public static SignificantNumber Decrement(SignificantNumber value) => throw new NotSupportedException();
	public static SignificantNumber Plus(SignificantNumber value) => +value;
	public static SignificantNumber Mod(SignificantNumber left, SignificantNumber right) => throw new NotSupportedException();
	public static bool GreaterThan(SignificantNumber left, SignificantNumber right) => left > right;
	public static bool GreaterThanOrEqual(SignificantNumber left, SignificantNumber right) => left >= right;
	public static bool LessThan(SignificantNumber left, SignificantNumber right) => left < right;
	public static bool LessThanOrEqual(SignificantNumber left, SignificantNumber right) => left <= right;
	public static bool Equal(SignificantNumber left, SignificantNumber right) => left == right;
	public static bool NotEqual(SignificantNumber left, SignificantNumber right) => left != right;
	public static SignificantNumber Max(SignificantNumber x, SignificantNumber y) => x > y ? x : y;
	public static SignificantNumber Min(SignificantNumber x, SignificantNumber y) => x < y ? x : y;
	public static SignificantNumber Clamp(SignificantNumber value, SignificantNumber min, SignificantNumber max) => value.Clamp(min, max);
	public static SignificantNumber Round(SignificantNumber value, int decimalDigits) => value.Round(decimalDigits);

	public static SignificantNumber operator -(SignificantNumber value)
	{
		return value == Zero
			? value
			: new(value.Exponent, -value.Significand);
	}

	public static SignificantNumber operator -(SignificantNumber left, SignificantNumber right)
	{
		int decimalDigits = LowestDecimalDigits(left, right);
		int commonExponent = MakeCommonizedAndGetExponent(ref left, ref right);
		AssertExponentsMatch(left, left);

		var newSignificand = left.Significand - right.Significand;
		return new SignificantNumber(commonExponent, newSignificand).Round(decimalDigits);
	}

	public static bool operator !=(SignificantNumber left, SignificantNumber right) => !(left == right);

	public static SignificantNumber operator *(SignificantNumber left, SignificantNumber right)
	{
		int significantDigits = LowestSignificantDigits(left, right);
		int commonExponent = MakeCommonizedAndGetExponent(ref left, ref right);
		AssertExponentsMatch(left, right);

		var newSignificand = left.Significand * right.Significand;
		int newExponent = commonExponent * 2;
		return new SignificantNumber(newExponent, newSignificand).ReduceSignificance(significantDigits);
	}

	public static SignificantNumber operator /(SignificantNumber left, SignificantNumber right)
	{
		int significantDigits = LowestSignificantDigits(left, right);
		int commonExponent = MakeCommonizedAndGetExponent(ref left, ref right);
		AssertExponentsMatch(left, right);

		var newSignificand = left.Significand * BigInteger.Pow(10, int.Abs(commonExponent)) / right.Significand;
		return new SignificantNumber(commonExponent, newSignificand).ReduceSignificance(significantDigits);
	}

	public static SignificantNumber operator +(SignificantNumber value) => value;

	public static SignificantNumber operator +(SignificantNumber left, SignificantNumber right)
	{
		int decimalDigits = LowestDecimalDigits(left, right);
		int commonExponent = MakeCommonizedAndGetExponent(ref left, ref right);
		AssertExponentsMatch(left, right);

		var newSignificand = left.Significand + right.Significand;
		return new SignificantNumber(commonExponent, newSignificand).Round(decimalDigits);
	}

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

	public static SignificantNumber operator %(SignificantNumber left, SignificantNumber right) => throw new NotSupportedException();
	public static SignificantNumber operator --(SignificantNumber value) => throw new NotSupportedException();
	public static SignificantNumber operator ++(SignificantNumber value) => throw new NotSupportedException();

	internal static void AssertDoesImplementGenericInterface(Type type, Type genericInterface) =>
		Debug.Assert(DoesImplementGenericInterface(type, genericInterface), $"{type.Name} does not implement {genericInterface.Name}");

	internal static bool DoesImplementGenericInterface(Type type, Type genericInterface)
	{
		bool genericInterfaceIsValid = genericInterface.IsInterface && genericInterface.IsGenericType;

		return genericInterfaceIsValid
			? Array.Exists(type.GetInterfaces(), x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterface)
			: throw new ArgumentException($"{genericInterface.Name} is not a generic interface");
	}
}
