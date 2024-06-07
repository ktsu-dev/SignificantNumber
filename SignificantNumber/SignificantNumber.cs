namespace ktsu.io.SignificantNumber;

using System.Diagnostics;
using System.Globalization;
using System.Numerics;

public readonly struct SignificantNumber
	: IAdditionOperators<SignificantNumber, SignificantNumber, SignificantNumber>
	, ISubtractionOperators<SignificantNumber, SignificantNumber, SignificantNumber>
	, IMultiplyOperators<SignificantNumber, SignificantNumber, SignificantNumber>
	, IDivisionOperators<SignificantNumber, SignificantNumber, SignificantNumber>
	, IUnaryPlusOperators<SignificantNumber, SignificantNumber>
	, IUnaryNegationOperators<SignificantNumber, SignificantNumber>
	, IEqualityOperators<SignificantNumber, SignificantNumber, bool>
{
	internal const int MaxDecimalPlaces = 15;
	internal const int MinDecimalPlaces = 1;
	private const string FormatSpecifier = "e";

	internal int SignificantDigits { get; }
	internal int Exponent { get; }
	internal BigInteger Significand { get; }

	public static SignificantNumber Zero => new(0, 0);
	public static SignificantNumber One => new(0, 1);
	public static SignificantNumber NegativeOne => new(0, -1);

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

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S1244:Floating point numbers should not be tested for equality", Justification = "<Pending>")]
	internal static SignificantNumber CreateFromFloatingPoint<TFloat>(TFloat input)
		where TFloat : IFloatingPoint<TFloat>
	{
		bool isOne = input == TFloat.One;
		bool isNegativeOne = input == TFloat.NegativeOne;
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

		string str = input.ToString($"{FormatSpecifier}{MaxDecimalPlaces}", CultureInfo.InvariantCulture);
		int indexOfE = str.IndexOf('e');
		var significandStr = str.AsSpan(0, indexOfE);
		var exponentStr = str.AsSpan(indexOfE + 1, str.Length - indexOfE - 1);
		int exponentValue = int.Parse(exponentStr, CultureInfo.InvariantCulture);

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
		var significandValue = BigInteger.Parse(significandStrWithoutDecimal, CultureInfo.InvariantCulture);
		return new(exponentValue, significandValue);
	}

	internal static SignificantNumber CreateFromInteger<TInteger>(TInteger input)
		where TInteger : IBinaryInteger<TInteger>
	{
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
		var integerComponent = input.ToString().AsSpan();
		while (integerComponent.Length > 1 && integerComponent[^1] == '0')
		{
			integerComponent = integerComponent[..^1];
			++exponentValue;
		}

		var significandValue = BigInteger.Parse(integerComponent, CultureInfo.InvariantCulture);

		return new(exponentValue, significandValue);
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

	private int CountDecimalDigits() =>
		Exponent > 0
		? 0
		: int.Abs(Exponent);

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

	public SignificantNumber Abs() => Significand < 0 ? -this : this;
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

	public static SignificantNumber operator +(SignificantNumber left, SignificantNumber right)
	{
		var leftCommon = left.WithCommonExponent(right);
		var rightCommon = right.WithCommonExponent(left);
		int decimalDigits = LowestDecimalDigits(left, right);
		var newSignificand = leftCommon.Significand + rightCommon.Significand;
		int newExponent = leftCommon.Exponent;
		return new SignificantNumber(newExponent, newSignificand).Round(decimalDigits);
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

	public static SignificantNumber operator +(SignificantNumber value) => value;

	public static SignificantNumber operator -(SignificantNumber value)
	{
		return value == Zero
			? value
			: new(value.Exponent, -value.Significand);
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

	public static bool operator !=(SignificantNumber left, SignificantNumber right) => !(left == right);

	public override bool Equals(object? obj) => obj is SignificantNumber number && this == number;

	public override int GetHashCode() => HashCode.Combine(Exponent, Significand);

	public override string ToString()
	{
		if (this == Zero)
		{
			return "0";
		}

		if (this == One)
		{
			return "1";
		}

		if (this == NegativeOne)
		{
			return "-1";
		}


		string significandStr = BigInteger.Abs(Significand).ToString(CultureInfo.InvariantCulture);
		if (Exponent == 0)
		{
			return significandStr;
		}

		if (Exponent > 0)
		{
			return $"{significandStr}{new string('0', Exponent)}";
		}

		int absExponent = -Exponent;
		string sign = Significand < 0 ? "-" : string.Empty;

		string integralComponent = absExponent >= significandStr.Length
			? "0"
			: significandStr[..^absExponent];

		string fractionalComponent = absExponent >= significandStr.Length
			? $"{new string('0', absExponent - significandStr.Length)}{BigInteger.Abs(Significand)}"
			: significandStr[^absExponent..];

		string output = $"{sign}{integralComponent}.{fractionalComponent}";
		return output;
	}

	public string ToString(string format) => format.Equals("G", StringComparison.OrdinalIgnoreCase) ? ToString() : throw new FormatException();
}
