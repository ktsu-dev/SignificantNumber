namespace ktsu.io.SignificantNumber;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

public readonly struct SignificantNumber
	: INumber<SignificantNumber>
{
	internal const int MaxDecimalPlaces = 15;
	internal const int MinDecimalPlaces = 1;
	private const string FormatSpecifier = "e15";

	private SignificantNumber(int exponent, BigInteger significand, bool sanitize = true)
	{
		if (sanitize)
		{
			// remove trailing zeros
			while (significand != 0 && significand % 10 == 0)
			{
				significand /= 10;
				exponent++;
			}

			// special case zero and 1 with max precision
			if (significand == 0)
			{
				SignificantDigits = MaxDecimalPlaces + 1;
				Exponent = -MaxDecimalPlaces;
				Significand = 0;
				return;
			}
			else if (significand == 1 && exponent == 0)
			{
				SignificantDigits = MaxDecimalPlaces + 1;
				Exponent = -MaxDecimalPlaces;
				Significand = BigInteger.Pow(10, MaxDecimalPlaces);
				return;
			}
			else if (significand == -1 && exponent == 0)
			{
				SignificantDigits = MaxDecimalPlaces + 1;
				Exponent = -MaxDecimalPlaces;
				Significand = -BigInteger.Pow(10, MaxDecimalPlaces);
				return;
			}
		}

		//count digits
		int significantDigits = 0;
		var number = significand;
		while (number != 0)
		{
			significantDigits++;
			number /= 10;
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

	public static int Radix => 2;

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
		int bufferSize = int.Abs(number.Exponent) + number.SignificantDigits + 2; // +2 is for negative symbol and decimal symbol
		Span<char> buffer = stackalloc char[bufferSize];
		Debug.Assert(buffer.Length >= bufferSize);
		return number.TryFormat(buffer, out int charsWritten, format.AsSpan(), formatProvider)
			? buffer[..charsWritten].ToString()
			: string.Empty;
	}

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

	[SuppressMessage("Major Bug", "S1244:Floating point numbers should not be tested for equality", Justification = "<Pending>")]
	internal static SignificantNumber CreateFromFloatingPoint<TFloat>(TFloat input)
		where TFloat : INumber<TFloat>
	{
		ArgumentNullException.ThrowIfNull(input);

		Debug.Assert(Array.Exists(typeof(TFloat).GetInterfaces(), i => i.Name.StartsWith("IFloatingPoint", StringComparison.Ordinal)));

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
		Debug.Assert(indexOfE > -1);

		var significandStr = str.AsSpan(0, indexOfE);
		var exponentStr = str.AsSpan(indexOfE + 1, str.Length - indexOfE - 1);
		int exponentValue = int.Parse(exponentStr, InvariantCulture);

		while (significandStr.Length > 2 && significandStr[^1] == '0')
		{
			significandStr = significandStr[..^1];
		}

		string[] components = significandStr.ToString().Split('.');
		Debug.Assert(components.Length == 2);

		var integerComponent = components[0].AsSpan();
		var fractionalComponent = components[1].AsSpan();
		int fractionalLength = fractionalComponent.Length;
		exponentValue -= fractionalLength;

		Debug.Assert(fractionalLength != 0 || integerComponent.Length == 1);

		string significandStrWithoutDecimal = $"{integerComponent}{fractionalComponent}";
		var significandValue = BigInteger.Parse(significandStrWithoutDecimal, InvariantCulture);
		return new(exponentValue, significandValue);
	}

	internal static SignificantNumber CreateFromInteger<TInteger>(TInteger input)
		where TInteger : INumber<TInteger>
	{
		ArgumentNullException.ThrowIfNull(input);

		Debug.Assert(Array.Exists(typeof(TInteger).GetInterfaces(), i => i.Name.StartsWith("IBinaryInteger", StringComparison.Ordinal)));

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
		var integerComponent = input.ToString().AsSpan(); //TODO: replace all uses of ToString with TryFormat with stack allocated spans
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

		BigInteger repeatingDigit = digit;
		for (int i = 1; i < numberOfRepeats; i++)
		{
			repeatingDigit = (repeatingDigit * 10) + digit;
		}

		return repeatingDigit;
	}

	private static int LowestDecimalDigits(SignificantNumber left, SignificantNumber right)
	{
		int leftDecimalDigits = left.CountDecimalDigits();
		int rightDecimalDigits = right.CountDecimalDigits();
		return leftDecimalDigits < rightDecimalDigits
		? leftDecimalDigits
		: rightDecimalDigits;
	}

	private static int LowestSignificantDigits(SignificantNumber left, SignificantNumber right) =>
		left.SignificantDigits < right.SignificantDigits
		? left.SignificantDigits
		: right.SignificantDigits;

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

	private SignificantNumber WithCommonExponent(SignificantNumber other)
	{
		int absExponent = int.Abs(Exponent);
		int absOtherExponent = int.Abs(other.Exponent);
		int highestExponent = absExponent > absOtherExponent ? absExponent : absOtherExponent;
		int exponentDifference = highestExponent - absExponent;
		var newSignificand = Significand * BigInteger.Pow(10, exponentDifference);
		int newExponent = Exponent == 0
			? -highestExponent
			: int.CopySign(highestExponent, Exponent);
		return new SignificantNumber(newExponent, newSignificand, sanitize: false);
	}

	[SuppressMessage("Major Code Smell", "S3358:Ternary operators should not be nested", Justification = "<Pending>")]
	public int CompareTo(object? obj) =>
		obj switch
		{
			null => 1,
			SignificantNumber number => CompareTo(number),
			_ => throw new ArgumentException(null, nameof(obj)),
		};

	[SuppressMessage("Major Code Smell", "S3358:Ternary operators should not be nested", Justification = "<Pending>")]
	public int CompareTo(SignificantNumber other) =>
		this < other
		? -1
		: this > other
		? 1
		: 0;

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
	public static SignificantNumber Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => throw new NotImplementedException();
	public static SignificantNumber Parse(string s, NumberStyles style, IFormatProvider? provider) => throw new NotImplementedException();
	public static SignificantNumber Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
	public static SignificantNumber Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => throw new NotImplementedException();
	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result) => throw new NotImplementedException();
	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result) => throw new NotImplementedException();
	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result) => throw new NotImplementedException();
	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result) => throw new NotImplementedException();
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
				//string exponentSymbol = format == "G" ? "E" : "e";
				//int overflowChars = int.Abs(Exponent) + significandStr.Length - MaxDecimalPlaces;
				//output = overflowChars > 0
				//	? $"{sign}{significandStr[..1]}{numberFormat.NumberDecimalSeparator}{significandStr[1..MaxDecimalPlaces]}{exponentSymbol}{Exponent + int.CopySign(overflowChars, Exponent):D3}"
				//	: $"{sign}{significandStr}{new string('0', Exponent)}";
				Span<char> trainlingZeroes = stackalloc char[Exponent];
				trainlingZeroes.Fill('0');
				output = $"{sign}{significandStr}{trainlingZeroes}";
			}
			else
			{

				int absExponent = -Exponent;

				string integralComponent = absExponent >= significandStr.Length
					? "0"
					: significandStr[..^absExponent];

				Span<char> fractionalZeroes = stackalloc char[absExponent - significandStr.Length];
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
	static bool INumberBase<SignificantNumber>.TryConvertFromChecked<TOther>(TOther value, out SignificantNumber result) => throw new NotImplementedException();
	static bool INumberBase<SignificantNumber>.TryConvertFromSaturating<TOther>(TOther value, out SignificantNumber result) => throw new NotImplementedException();
	static bool INumberBase<SignificantNumber>.TryConvertFromTruncating<TOther>(TOther value, out SignificantNumber result) => throw new NotImplementedException();
	static bool INumberBase<SignificantNumber>.TryConvertToChecked<TOther>(SignificantNumber value, out TOther result) => throw new NotImplementedException();
	static bool INumberBase<SignificantNumber>.TryConvertToSaturating<TOther>(SignificantNumber value, out TOther result) => throw new NotImplementedException();
	static bool INumberBase<SignificantNumber>.TryConvertToTruncating<TOther>(SignificantNumber value, out TOther result) => throw new NotImplementedException();
	static bool INumberBase<SignificantNumber>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out SignificantNumber result) => TryParse(s, style, provider, out result);
	static bool INumberBase<SignificantNumber>.TryParse(string? s, NumberStyles style, IFormatProvider? provider, out SignificantNumber result) => TryParse(s, style, provider, out result);
	bool IEquatable<SignificantNumber>.Equals(SignificantNumber other) => Equals(other);
	bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormat(destination, out charsWritten, format, provider);
	string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString(this, format, formatProvider);
	static SignificantNumber ISpanParsable<SignificantNumber>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s, provider);
	static bool ISpanParsable<SignificantNumber>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out SignificantNumber result) => TryParse(s.ToString(), provider, out result);
	static SignificantNumber IParsable<SignificantNumber>.Parse(string s, IFormatProvider? provider) => Parse(s, provider);
	static bool IParsable<SignificantNumber>.TryParse(string? s, IFormatProvider? provider, out SignificantNumber result) => TryParse(s, provider, out result);

	public static SignificantNumber operator -(SignificantNumber value)
	{
		return value == Zero
			? value
			: new(value.Exponent, -value.Significand);
	}

	public static SignificantNumber operator -(SignificantNumber left, SignificantNumber right)
	{
		var leftCommon = left.WithCommonExponent(right);
		var rightCommon = right.WithCommonExponent(left);
		int decimalDigits = LowestDecimalDigits(left, right);
		var newSignificand = leftCommon.Significand - rightCommon.Significand;
		int newExponent = leftCommon.Exponent;
		return new SignificantNumber(newExponent, newSignificand).Round(decimalDigits);
	}

	public static bool operator !=(SignificantNumber left, SignificantNumber right) => !(left == right);

	public static SignificantNumber operator *(SignificantNumber left, SignificantNumber right)
	{
		var leftCommon = left.WithCommonExponent(right);
		var rightCommon = right.WithCommonExponent(left);
		int significantDigits = LowestSignificantDigits(left, right);
		int newExponent = leftCommon.Exponent;
		var newSignificand = leftCommon.Significand * rightCommon.Significand / BigInteger.Pow(10, int.Abs(newExponent));
		return new SignificantNumber(newExponent, newSignificand).ReduceSignificance(significantDigits);
	}

	public static SignificantNumber operator /(SignificantNumber left, SignificantNumber right)
	{
		var leftCommon = left.WithCommonExponent(right);
		var rightCommon = right.WithCommonExponent(left);
		int significantDigits = LowestSignificantDigits(left, right);
		int newExponent = leftCommon.Exponent;
		var newSignificand = leftCommon.Significand * BigInteger.Pow(10, int.Abs(newExponent)) / rightCommon.Significand;
		return new SignificantNumber(newExponent, newSignificand).ReduceSignificance(significantDigits);
	}

	public static SignificantNumber operator +(SignificantNumber value) => value;

	public static SignificantNumber operator +(SignificantNumber left, SignificantNumber right)
	{
		var leftCommon = left.WithCommonExponent(right);
		var rightCommon = right.WithCommonExponent(left);
		int decimalDigits = LowestDecimalDigits(left, right);
		var newSignificand = leftCommon.Significand + rightCommon.Significand;
		int newExponent = leftCommon.Exponent;
		return new SignificantNumber(newExponent, newSignificand).Round(decimalDigits);
	}

	public static bool operator ==(SignificantNumber left, SignificantNumber right)
	{
		var leftCommon = left.WithCommonExponent(right);
		var rightCommon = right.WithCommonExponent(left);
		int decimalDigits = LowestDecimalDigits(left, right);
		var leftSignificant = leftCommon.Round(decimalDigits);
		var rightSignificant = rightCommon.Round(decimalDigits);
		return leftSignificant.Exponent == rightSignificant.Exponent
			&& leftSignificant.Significand == rightSignificant.Significand;
	}

	public static bool operator >(SignificantNumber left, SignificantNumber right) => throw new NotImplementedException();
	public static bool operator >=(SignificantNumber left, SignificantNumber right) => throw new NotImplementedException();
	public static bool operator <(SignificantNumber left, SignificantNumber right) => throw new NotImplementedException();
	public static bool operator <=(SignificantNumber left, SignificantNumber right) => throw new NotImplementedException();
	public static SignificantNumber operator %(SignificantNumber left, SignificantNumber right) => throw new NotImplementedException();
	public static SignificantNumber operator --(SignificantNumber value) => throw new NotImplementedException();
	public static SignificantNumber operator ++(SignificantNumber value) => throw new NotImplementedException();
	static bool IComparisonOperators<SignificantNumber, SignificantNumber, bool>.operator >(SignificantNumber left, SignificantNumber right) => throw new NotImplementedException();
	static bool IComparisonOperators<SignificantNumber, SignificantNumber, bool>.operator >=(SignificantNumber left, SignificantNumber right) => throw new NotImplementedException();
	static bool IComparisonOperators<SignificantNumber, SignificantNumber, bool>.operator <(SignificantNumber left, SignificantNumber right) => throw new NotImplementedException();
	static bool IComparisonOperators<SignificantNumber, SignificantNumber, bool>.operator <=(SignificantNumber left, SignificantNumber right) => throw new NotImplementedException();
	static SignificantNumber IModulusOperators<SignificantNumber, SignificantNumber, SignificantNumber>.operator %(SignificantNumber left, SignificantNumber right) => throw new NotImplementedException();
	static SignificantNumber IAdditionOperators<SignificantNumber, SignificantNumber, SignificantNumber>.operator +(SignificantNumber left, SignificantNumber right) => throw new NotImplementedException();
	static SignificantNumber IDecrementOperators<SignificantNumber>.operator --(SignificantNumber value) => throw new NotImplementedException();
	static SignificantNumber IDivisionOperators<SignificantNumber, SignificantNumber, SignificantNumber>.operator /(SignificantNumber left, SignificantNumber right) => throw new NotImplementedException();
	static bool IEqualityOperators<SignificantNumber, SignificantNumber, bool>.operator ==(SignificantNumber left, SignificantNumber right) => throw new NotImplementedException();
	static bool IEqualityOperators<SignificantNumber, SignificantNumber, bool>.operator !=(SignificantNumber left, SignificantNumber right) => throw new NotImplementedException();
	static SignificantNumber IIncrementOperators<SignificantNumber>.operator ++(SignificantNumber value) => throw new NotImplementedException();
	static SignificantNumber IMultiplyOperators<SignificantNumber, SignificantNumber, SignificantNumber>.operator *(SignificantNumber left, SignificantNumber right) => throw new NotImplementedException();
	static SignificantNumber ISubtractionOperators<SignificantNumber, SignificantNumber, SignificantNumber>.operator -(SignificantNumber left, SignificantNumber right) => throw new NotImplementedException();
	static SignificantNumber IUnaryNegationOperators<SignificantNumber, SignificantNumber>.operator -(SignificantNumber value) => throw new NotImplementedException();
	static SignificantNumber IUnaryPlusOperators<SignificantNumber, SignificantNumber>.operator +(SignificantNumber value) => throw new NotImplementedException();
}
