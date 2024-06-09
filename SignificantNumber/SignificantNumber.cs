[assembly: CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.ComVisible(false)]
namespace ktsu.io.SignificantNumber;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

[DebuggerDisplay("{Significand}e{Exponent}")]
public readonly struct SignificantNumber
	: INumber<SignificantNumber>
{
	internal const int MaxDecimalPlaces = 15;
	internal const int MinDecimalPlaces = 1;
	private const string FormatSpecifier = "e15";

	private SignificantNumber(int exponent, BigInteger significand, bool sanitize = true)
	{
		const int ten = 10;

		if (sanitize)
		{
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

	public static SignificantNumber NegativeOne => new(0, -1);

	public static SignificantNumber One => new(0, 1);

	public static SignificantNumber Zero => new(0, 0);

	internal int Exponent { get; }

	internal BigInteger Significand { get; }

	internal int SignificantDigits { get; }

	private static CultureInfo InvariantCulture { get; } = CultureInfo.InvariantCulture;

	private const int BinaryRadix = 2;
	public static int Radix => BinaryRadix;

	public static SignificantNumber AdditiveIdentity => Zero;

	public static SignificantNumber MultiplicativeIdentity => One;

	static SignificantNumber INumberBase<SignificantNumber>.One => One;

	static int INumberBase<SignificantNumber>.Radix => Radix;

	static SignificantNumber INumberBase<SignificantNumber>.Zero => Zero;

	static SignificantNumber IAdditiveIdentity<SignificantNumber, SignificantNumber>.AdditiveIdentity => AdditiveIdentity;

	static SignificantNumber IMultiplicativeIdentity<SignificantNumber, SignificantNumber>.MultiplicativeIdentity => MultiplicativeIdentity;

	public override bool Equals(object? obj) => obj is SignificantNumber number && this == number;

	public bool Equals(SignificantNumber other) => this == other;

	public override int GetHashCode() => HashCode.Combine(Exponent, Significand);

	public override string ToString() => ToString(this, null, null);

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

		Debug.Assert(Array.Exists(typeof(TFloat).GetInterfaces(), i => i.Name.StartsWith("IFloatingPoint", StringComparison.Ordinal)), $"{typeof(TFloat).Name} does not implement IFloatingPoint");

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

		Debug.Assert(Array.Exists(typeof(TInteger).GetInterfaces(), i => i.Name.StartsWith("IBinaryInteger", StringComparison.Ordinal)), $"{typeof(TInteger).Name} does not implement IBinaryInteger");

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

		const int sizeOfSignificand = 17;
		const int sizeOfExponent = 5;

		Span<char> inputSpan = stackalloc char[sizeOfSignificand + sizeOfExponent];
		_ = input.TryFormat(inputSpan, out int charsWritten, "", CultureInfo.InvariantCulture);
		ReadOnlySpan<char> integerComponent = inputSpan;
		while (integerComponent.Length > 1 && integerComponent[^1] == '0')
		{
			integerComponent = integerComponent[..^1];
			++exponentValue;
		}

		var significandValue = BigInteger.Parse(integerComponent, InvariantCulture);

		return new(exponentValue, significandValue);
	}

	private static BigInteger CreateRepeatingDigits(int digit, int numberOfRepeats)
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

	private bool HasInfinitePrecision => Exponent == 0 && (Significand == BigInteger.One || Significand == BigInteger.Zero || Significand == BigInteger.MinusOne);

	private static int LowestDecimalDigits(SignificantNumber left, SignificantNumber right)
	{
		int leftDecimalDigits = left.CountDecimalDigits();
		int rightDecimalDigits = right.CountDecimalDigits();

		leftDecimalDigits = left.HasInfinitePrecision ? rightDecimalDigits : leftDecimalDigits;
		rightDecimalDigits = right.HasInfinitePrecision ? leftDecimalDigits : rightDecimalDigits;

		return leftDecimalDigits < rightDecimalDigits
			? leftDecimalDigits
			: rightDecimalDigits;
	}

	private static int LowestSignificantDigits(SignificantNumber left, SignificantNumber right)
	{
		int leftSignificantDigits = left.SignificantDigits;
		int rightSignificantDigits = right.SignificantDigits;

		leftSignificantDigits = left.HasInfinitePrecision ? rightSignificantDigits : leftSignificantDigits;
		rightSignificantDigits = right.HasInfinitePrecision ? leftSignificantDigits : rightSignificantDigits;

		return leftSignificantDigits < rightSignificantDigits
		? leftSignificantDigits
		: rightSignificantDigits;
	}

	private int CountDecimalDigits() =>
		Exponent > 0
		? 0
		: int.Abs(Exponent);

	private SignificantNumber ReduceSignificance(int significantDigits)
	{
		int significantDifference = significantDigits < SignificantDigits
			? SignificantDigits - significantDigits
			: 0;
		int newExponent = Exponent - int.CopySign(significantDifference, Exponent);
		var roundingFactor = BigInteger.CopySign(CreateRepeatingDigits(5, significantDifference), Significand);
		var newSignificand = (Significand + roundingFactor) / BigInteger.Pow(10, significantDifference);
		return new(newExponent, newSignificand);
	}

	private static void MakeCommonized(ref SignificantNumber left, ref SignificantNumber right) =>
		_ = MakeCommonizedAndGetExponent(ref left, ref right);

	private static int MakeCommonizedAndGetExponent(ref SignificantNumber left, ref SignificantNumber right)
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

	public int CompareTo(object? obj) =>
		obj switch
		{
			null => 1,
			SignificantNumber number => CompareTo(number),
			_ => throw new ArgumentException(null, nameof(obj)),
		};

	public int CompareTo(SignificantNumber other)
	{
		int greaterOrEqual = this > other ? 1 : 0;
		return this < other ? -1 : greaterOrEqual;
	}

	public static SignificantNumber Abs(SignificantNumber value) => value.Significand < 0 ? -value : value;
	public static bool IsCanonical(SignificantNumber _) => true;
	public static bool IsComplexNumber(SignificantNumber value) => !IsRealNumber(value);
	public static bool IsEvenInteger(SignificantNumber value) => IsInteger(value) && value.Significand.IsEven;
	public static bool IsFinite(SignificantNumber _) => true;
	public static bool IsImaginaryNumber(SignificantNumber value) => !IsRealNumber(value);
	public static bool IsInfinity(SignificantNumber value) => !IsFinite(value);
	public static bool IsInteger(SignificantNumber value) => value.Exponent >= 0;
	public static bool IsNaN(SignificantNumber _) => false;
	public static bool IsNegative(SignificantNumber value) => !IsPositive(value);
	public static bool IsNegativeInfinity(SignificantNumber value) => IsNegative(value);
	public static bool IsNormal(SignificantNumber _) => true;
	public static bool IsOddInteger(SignificantNumber value) => IsInteger(value) && !value.Significand.IsEven;
	public static bool IsPositive(SignificantNumber value) => value.Significand >= 0;
	public static bool IsPositiveInfinity(SignificantNumber value) => IsInfinity(value) && IsPositive(value);
	public static bool IsRealNumber(SignificantNumber _) => true;
	public static bool IsSubnormal(SignificantNumber value) => !IsNormal(value);
	public static bool IsZero(SignificantNumber value) => value.Significand == 0;
	public static SignificantNumber MaxMagnitude(SignificantNumber x, SignificantNumber y) => x.Abs() > y.Abs() ? x : y;
	public static SignificantNumber MaxMagnitudeNumber(SignificantNumber x, SignificantNumber y) => MaxMagnitude(x, y);
	public static SignificantNumber MinMagnitude(SignificantNumber x, SignificantNumber y) => x.Abs() < y.Abs() ? x : y;
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
		Debug.Assert(success, $"Destination buffer is too small:{Environment.NewLine}{destination.Length} when needed {output.Length}{Environment.NewLine}{SignificantDigits}:{Significand}e{Exponent}");

		charsWritten = output.Length;
		return true;
	}

	int IComparable.CompareTo(object? obj) => CompareTo(obj);
	int IComparable<SignificantNumber>.CompareTo(SignificantNumber other) => CompareTo(other);
	static SignificantNumber INumberBase<SignificantNumber>.Abs(SignificantNumber value) => Abs(value);
	static bool INumberBase<SignificantNumber>.IsCanonical(SignificantNumber value) => IsCanonical(value);
	static bool INumberBase<SignificantNumber>.IsComplexNumber(SignificantNumber value) => IsComplexNumber(value);
	static bool INumberBase<SignificantNumber>.IsEvenInteger(SignificantNumber value) => IsEvenInteger(value);
	static bool INumberBase<SignificantNumber>.IsFinite(SignificantNumber value) => IsFinite(value);
	static bool INumberBase<SignificantNumber>.IsImaginaryNumber(SignificantNumber value) => IsImaginaryNumber(value);
	static bool INumberBase<SignificantNumber>.IsInfinity(SignificantNumber value) => IsInfinity(value);
	static bool INumberBase<SignificantNumber>.IsInteger(SignificantNumber value) => IsInteger(value);
	static bool INumberBase<SignificantNumber>.IsNaN(SignificantNumber value) => IsNaN(value);
	static bool INumberBase<SignificantNumber>.IsNegative(SignificantNumber value) => IsNegative(value);
	static bool INumberBase<SignificantNumber>.IsNegativeInfinity(SignificantNumber value) => IsNegativeInfinity(value);
	static bool INumberBase<SignificantNumber>.IsNormal(SignificantNumber value) => IsNormal(value);
	static bool INumberBase<SignificantNumber>.IsOddInteger(SignificantNumber value) => IsOddInteger(value);
	static bool INumberBase<SignificantNumber>.IsPositive(SignificantNumber value) => IsPositive(value);
	static bool INumberBase<SignificantNumber>.IsPositiveInfinity(SignificantNumber value) => IsPositiveInfinity(value);
	static bool INumberBase<SignificantNumber>.IsRealNumber(SignificantNumber value) => IsRealNumber(value);
	static bool INumberBase<SignificantNumber>.IsSubnormal(SignificantNumber value) => IsSubnormal(value);
	static bool INumberBase<SignificantNumber>.IsZero(SignificantNumber value) => IsZero(value);
	static SignificantNumber INumberBase<SignificantNumber>.MaxMagnitude(SignificantNumber x, SignificantNumber y) => MaxMagnitude(x, y);
	static SignificantNumber INumberBase<SignificantNumber>.MaxMagnitudeNumber(SignificantNumber x, SignificantNumber y) => MaxMagnitudeNumber(x, y);
	static SignificantNumber INumberBase<SignificantNumber>.MinMagnitude(SignificantNumber x, SignificantNumber y) => MinMagnitude(x, y);
	static SignificantNumber INumberBase<SignificantNumber>.MinMagnitudeNumber(SignificantNumber x, SignificantNumber y) => MinMagnitudeNumber(x, y);
	static SignificantNumber INumberBase<SignificantNumber>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => Parse(s, style, provider);
	static SignificantNumber INumberBase<SignificantNumber>.Parse(string s, NumberStyles style, IFormatProvider? provider) => Parse(s, style, provider);
	static bool INumberBase<SignificantNumber>.TryConvertFromChecked<TOther>(TOther value, out SignificantNumber result) => throw new NotSupportedException();
	static bool INumberBase<SignificantNumber>.TryConvertFromSaturating<TOther>(TOther value, out SignificantNumber result) => throw new NotSupportedException();
	static bool INumberBase<SignificantNumber>.TryConvertFromTruncating<TOther>(TOther value, out SignificantNumber result) => throw new NotSupportedException();
	static bool INumberBase<SignificantNumber>.TryConvertToChecked<TOther>(SignificantNumber value, out TOther result) => throw new NotSupportedException();
	static bool INumberBase<SignificantNumber>.TryConvertToSaturating<TOther>(SignificantNumber value, out TOther result) => throw new NotSupportedException();
	static bool INumberBase<SignificantNumber>.TryConvertToTruncating<TOther>(SignificantNumber value, out TOther result) => throw new NotSupportedException();
	static bool INumberBase<SignificantNumber>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out SignificantNumber result) => TryParse(s, style, provider, out result);
	static bool INumberBase<SignificantNumber>.TryParse(string? s, NumberStyles style, IFormatProvider? provider, out SignificantNumber result) => TryParse(s, style, provider, out result);
	bool IEquatable<SignificantNumber>.Equals(SignificantNumber other) => Equals(other);
	bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormat(destination, out charsWritten, format, provider);
	string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString(format, formatProvider);
	static SignificantNumber ISpanParsable<SignificantNumber>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s, provider);
	static bool ISpanParsable<SignificantNumber>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out SignificantNumber result) => TryParse(s, provider, out result);
	static SignificantNumber IParsable<SignificantNumber>.Parse(string s, IFormatProvider? provider) => Parse(s, provider);
	static bool IParsable<SignificantNumber>.TryParse(string? s, IFormatProvider? provider, out SignificantNumber result) => TryParse(s, provider, out result);

	private static void AssertExponentsMatch(SignificantNumber left, SignificantNumber right) =>
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

		var newSignificand = left.Significand * right.Significand / BigInteger.Pow(10, int.Abs(commonExponent));
		return new SignificantNumber(commonExponent, newSignificand).ReduceSignificance(significantDigits);
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
	static bool IComparisonOperators<SignificantNumber, SignificantNumber, bool>.operator >(SignificantNumber left, SignificantNumber right) => left > right;
	static bool IComparisonOperators<SignificantNumber, SignificantNumber, bool>.operator >=(SignificantNumber left, SignificantNumber right) => left >= right;
	static bool IComparisonOperators<SignificantNumber, SignificantNumber, bool>.operator <(SignificantNumber left, SignificantNumber right) => left < right;
	static bool IComparisonOperators<SignificantNumber, SignificantNumber, bool>.operator <=(SignificantNumber left, SignificantNumber right) => left <= right;
	static SignificantNumber IModulusOperators<SignificantNumber, SignificantNumber, SignificantNumber>.operator %(SignificantNumber left, SignificantNumber right) => left % right;
	static SignificantNumber IAdditionOperators<SignificantNumber, SignificantNumber, SignificantNumber>.operator +(SignificantNumber left, SignificantNumber right) => left + right;
	static SignificantNumber IDecrementOperators<SignificantNumber>.operator --(SignificantNumber value) => throw new NotSupportedException();
	static SignificantNumber IDivisionOperators<SignificantNumber, SignificantNumber, SignificantNumber>.operator /(SignificantNumber left, SignificantNumber right) => left / right;
	static bool IEqualityOperators<SignificantNumber, SignificantNumber, bool>.operator ==(SignificantNumber left, SignificantNumber right) => left == right;
	static bool IEqualityOperators<SignificantNumber, SignificantNumber, bool>.operator !=(SignificantNumber left, SignificantNumber right) => left != right;
	static SignificantNumber IIncrementOperators<SignificantNumber>.operator ++(SignificantNumber value) => throw new NotSupportedException();
	static SignificantNumber IMultiplyOperators<SignificantNumber, SignificantNumber, SignificantNumber>.operator *(SignificantNumber left, SignificantNumber right) => left * right;
	static SignificantNumber ISubtractionOperators<SignificantNumber, SignificantNumber, SignificantNumber>.operator -(SignificantNumber left, SignificantNumber right) => left - right;
	static SignificantNumber IUnaryNegationOperators<SignificantNumber, SignificantNumber>.operator -(SignificantNumber value) => -value;
	static SignificantNumber IUnaryPlusOperators<SignificantNumber, SignificantNumber>.operator +(SignificantNumber value) => +value;
}
