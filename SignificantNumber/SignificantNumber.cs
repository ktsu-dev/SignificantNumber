namespace ktsu.io.SignificantNumber;

using System.Globalization;
using System.Numerics;

public readonly struct SignificantNumber
	: IAdditionOperators<SignificantNumber, SignificantNumber, SignificantNumber>
	, ISubtractionOperators<SignificantNumber, SignificantNumber, SignificantNumber>
	, IMultiplyOperators<SignificantNumber, SignificantNumber, SignificantNumber>
	//, IDivisionOperators<SignificantNumber, SignificantNumber, SignificantNumber>
	, IUnaryPlusOperators<SignificantNumber, SignificantNumber>
	, IUnaryNegationOperators<SignificantNumber, SignificantNumber>
	, IEqualityOperators<SignificantNumber, SignificantNumber, bool>
{
	internal const int MaxDecimalPlaces = 15;
	internal const int MinDecimalPlaces = 1;
	private const string FormatSpecifier = "e";

	internal sbyte SignificantDigits { get; }
	internal sbyte Exponent { get; }
	internal BigInteger Significand { get; }

	public static SignificantNumber Zero => new(0, 0, 0);
	public static SignificantNumber One => new(MaxDecimalPlaces + 1, MaxDecimalPlaces, BigInteger.Pow(10, MaxDecimalPlaces));

	private SignificantNumber(sbyte significantDigits, sbyte exponent, BigInteger significand)
	{
		if (significand == 0)
		{
			SignificantDigits = MaxDecimalPlaces + 1;
			Exponent = MaxDecimalPlaces;
			Significand = 0;
			return;
		}

		SignificantDigits = significantDigits;
		Exponent = exponent;
		Significand = significand;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S1244:Floating point numbers should not be tested for equality", Justification = "<Pending>")]
	internal static SignificantNumber CreateFromFloatingPoint<TFloat>(TFloat input)
		where TFloat : IFloatingPoint<TFloat>
	{
		bool isOne = TFloat.Abs(input) == TFloat.One;
		bool isZero = TFloat.IsZero(input);

		if (isZero)
		{
			return Zero;
		}

		if (isOne)
		{
			return One;
		}

		string str = input.ToString($"{FormatSpecifier}{MaxDecimalPlaces}", CultureInfo.InvariantCulture);
		int indexOfE = str.IndexOf('e');
		var significandStr = str.AsSpan(0, indexOfE);
		var exponentStr = str.AsSpan(indexOfE + 1, str.Length - indexOfE - 1);
		sbyte exponentValue = sbyte.Parse(exponentStr, CultureInfo.InvariantCulture);

		while (significandStr.Length > 2 && significandStr[^1] == '0')
		{
			significandStr = significandStr[..^1];
		}

		string[] components = significandStr.ToString().Split('.');
		if (components.Length < 2)
		{
			throw new FormatException();
		}

		var integerComponent = components[0].AsSpan();
		var fractionalComponent = components[1].AsSpan();
		sbyte fractionalLength = (sbyte)fractionalComponent.Length;
		exponentValue -= fractionalLength;

		if (fractionalLength == 0)
		{
			while (integerComponent.Length > 2 && integerComponent[^1] == '0')
			{
				integerComponent = integerComponent[..^1];
				++exponentValue;
			}
		}

		string significandStrWithoutDecimal = $"{integerComponent}{fractionalComponent}";
		string significandStrWithoutSymbols = significandStrWithoutDecimal.ToString().Replace("-", "");
		sbyte significantDigits = (sbyte)significandStrWithoutSymbols.Length;
		var significandValue = BigInteger.Parse(significandStrWithoutDecimal, CultureInfo.InvariantCulture);
		return new(significantDigits, exponentValue, significandValue);
	}

	internal static SignificantNumber CreateFromInteger<TInteger>(TInteger input)
		where TInteger : IBinaryInteger<TInteger>
	{
		bool isOne = TInteger.Abs(input) == TInteger.One;
		bool isZero = TInteger.IsZero(input);

		if (isZero)
		{
			return Zero;
		}

		if (isOne)
		{
			return One;
		}

		sbyte exponentValue = 0;
		var integerComponent = input.ToString().AsSpan();
		while (integerComponent.Length > 1 && integerComponent[^1] == '0')
		{
			integerComponent = integerComponent[..^1];
			++exponentValue;
		}

		var significandValue = BigInteger.Parse(integerComponent, CultureInfo.InvariantCulture);

		return new((sbyte)integerComponent.Length, exponentValue, significandValue);
	}

	internal SignificantNumber WithLowestSignificantDigits(SignificantNumber other)
	{
		sbyte smallestSignificantDigits = SignificantDigits < other.SignificantDigits ? SignificantDigits : other.SignificantDigits;
		sbyte reducingExponent = (sbyte)(SignificantDigits - smallestSignificantDigits);
		sbyte newExponent = (sbyte)(Exponent + reducingExponent);
		var newSignificand = Significand / BigInteger.Pow(10, reducingExponent);
		return new(smallestSignificantDigits, newExponent, newSignificand);
	}

	internal SignificantNumber WithLowestExponent(SignificantNumber other)
	{
		sbyte smallestExponent = sbyte.Abs(Exponent) < sbyte.Abs(other.Exponent) ? Exponent : other.Exponent;
		sbyte reducingExponent = (sbyte)int.Abs(Exponent - smallestExponent);
		var newSignificand = Significand / BigInteger.Pow(10, reducingExponent);
		return new(SignificantDigits, smallestExponent, newSignificand);
	}

	public static SignificantNumber operator *(SignificantNumber left, SignificantNumber right)
	{
		var leftSignificant = left.WithLowestSignificantDigits(right);
		var rightSignificant = right.WithLowestSignificantDigits(left);
		var leftCommon = leftSignificant.WithLowestExponent(rightSignificant);
		var rightCommon = rightSignificant.WithLowestExponent(leftSignificant);
		var newSignificand = leftCommon.Significand * rightCommon.Significand / BigInteger.Pow(10, sbyte.Abs(leftCommon.Exponent));
		return new(leftCommon.SignificantDigits, leftCommon.Exponent, newSignificand);
	}

	public static SignificantNumber operator +(SignificantNumber left, SignificantNumber right)
	{
		var leftSignificant = left.WithLowestSignificantDigits(right);
		var rightSignificant = right.WithLowestSignificantDigits(left);
		var leftCommon = leftSignificant.WithLowestExponent(rightSignificant);
		var rightCommon = rightSignificant.WithLowestExponent(leftSignificant);
		return new(leftCommon.SignificantDigits, leftCommon.Exponent, leftCommon.Significand + rightCommon.Significand);
	}

	public static SignificantNumber operator -(SignificantNumber left, SignificantNumber right)
	{
		var leftSignificant = left.WithLowestSignificantDigits(right);
		var rightSignificant = right.WithLowestSignificantDigits(left);
		var leftCommon = leftSignificant.WithLowestExponent(rightSignificant);
		var rightCommon = rightSignificant.WithLowestExponent(leftSignificant);
		return new(leftCommon.SignificantDigits, leftCommon.Exponent, leftCommon.Significand - rightCommon.Significand);
	}

	public static SignificantNumber operator +(SignificantNumber value) => value;

	public static SignificantNumber operator -(SignificantNumber value)
	{
		return value == Zero
			? Zero
			: new(value.SignificantDigits, value.Exponent, -value.Significand);
	}

	public static bool operator ==(SignificantNumber left, SignificantNumber right)
	{
		var leftSignificant = left.WithLowestSignificantDigits(right);
		var rightSignificant = right.WithLowestSignificantDigits(left);
		var leftCommon = leftSignificant.WithLowestExponent(rightSignificant);
		var rightCommon = rightSignificant.WithLowestExponent(leftSignificant);
		return leftCommon.Significand == rightCommon.Significand;
	}

	public static bool operator !=(SignificantNumber left, SignificantNumber right) => !(left == right);

	public override bool Equals(object? obj) => obj is SignificantNumber number && this == number;

	public override int GetHashCode() => HashCode.Combine(Exponent, Significand);

	public override string ToString()
	{
		string significandStr = Significand.ToString(CultureInfo.InvariantCulture);
		if (Exponent == 0)
		{
			return significandStr;
		}

		if (Exponent > 0)
		{
			return $"{significandStr}{new string('0', Exponent)}";
		}

		int absExponent = -Exponent;
		return absExponent >= significandStr.Length
			? $"0.{new string('0', absExponent - significandStr.Length)}{significandStr}"
			: $"{significandStr[..^absExponent]}.{significandStr[^absExponent..]}";
	}

	public string ToString(string format) => format == "G" ? ToString() : throw new FormatException();
}
