namespace ktsu.SignificantNumber.Test;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

// a class for testing the exceptions in the SignificantNumberExtensions class
public sealed class InvalidNumber : INumber<InvalidNumber>
{
	public static InvalidNumber One => throw new NotSupportedException();

	public static int Radix => throw new NotSupportedException();

	public static InvalidNumber Zero => throw new NotSupportedException();

	public static InvalidNumber AdditiveIdentity => throw new NotSupportedException();

	public static InvalidNumber MultiplicativeIdentity => throw new NotSupportedException();

	static InvalidNumber INumberBase<InvalidNumber>.One => throw new NotSupportedException();

	static int INumberBase<InvalidNumber>.Radix => throw new NotSupportedException();

	static InvalidNumber INumberBase<InvalidNumber>.Zero => throw new NotSupportedException();

	static InvalidNumber IAdditiveIdentity<InvalidNumber, InvalidNumber>.AdditiveIdentity => throw new NotSupportedException();

	static InvalidNumber IMultiplicativeIdentity<InvalidNumber, InvalidNumber>.MultiplicativeIdentity => throw new NotSupportedException();

	public override bool Equals(object? obj) => false;

	public override int GetHashCode() => 1.GetHashCode();

	public int CompareTo(object? obj) => throw new NotSupportedException();
	public int CompareTo(InvalidNumber? other) => throw new NotSupportedException();
	public static InvalidNumber Abs(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsCanonical(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsComplexNumber(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsEvenInteger(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsFinite(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsImaginaryNumber(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsInfinity(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsInteger(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsNaN(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsNegative(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsNegativeInfinity(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsNormal(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsOddInteger(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsPositive(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsPositiveInfinity(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsRealNumber(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsSubnormal(InvalidNumber value) => throw new NotSupportedException();
	public static bool IsZero(InvalidNumber value) => throw new NotSupportedException();
	public static InvalidNumber MaxMagnitude(InvalidNumber x, InvalidNumber y) => throw new NotSupportedException();
	public static InvalidNumber MaxMagnitudeNumber(InvalidNumber x, InvalidNumber y) => throw new NotSupportedException();
	public static InvalidNumber MinMagnitude(InvalidNumber x, InvalidNumber y) => throw new NotSupportedException();
	public static InvalidNumber MinMagnitudeNumber(InvalidNumber x, InvalidNumber y) => throw new NotSupportedException();
	public static InvalidNumber Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => throw new NotSupportedException();
	public static InvalidNumber Parse(string s, NumberStyles style, IFormatProvider? provider) => throw new NotSupportedException();
	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out InvalidNumber result) => throw new NotSupportedException();
	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out InvalidNumber result) => throw new NotSupportedException();
	public bool Equals(InvalidNumber? _) => this is null;
	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotSupportedException();
	public string ToString(string? format, IFormatProvider? formatProvider) => throw new NotSupportedException();
	public static InvalidNumber Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => throw new NotSupportedException();
	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out InvalidNumber result) => throw new NotSupportedException();
	public static InvalidNumber Parse(string s, IFormatProvider? provider) => throw new NotSupportedException();
	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out InvalidNumber result) => throw new NotSupportedException();
	int IComparable.CompareTo(object? obj) => throw new NotSupportedException();
	int IComparable<InvalidNumber>.CompareTo(InvalidNumber? other) => throw new NotSupportedException();
	static InvalidNumber INumberBase<InvalidNumber>.Abs(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsCanonical(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsComplexNumber(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsEvenInteger(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsFinite(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsImaginaryNumber(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsInfinity(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsInteger(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsNaN(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsNegative(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsNegativeInfinity(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsNormal(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsOddInteger(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsPositive(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsPositiveInfinity(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsRealNumber(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsSubnormal(InvalidNumber value) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.IsZero(InvalidNumber value) => throw new NotSupportedException();
	static InvalidNumber INumberBase<InvalidNumber>.MaxMagnitude(InvalidNumber x, InvalidNumber y) => throw new NotSupportedException();
	static InvalidNumber INumberBase<InvalidNumber>.MaxMagnitudeNumber(InvalidNumber x, InvalidNumber y) => throw new NotSupportedException();
	static InvalidNumber INumberBase<InvalidNumber>.MinMagnitude(InvalidNumber x, InvalidNumber y) => throw new NotSupportedException();
	static InvalidNumber INumberBase<InvalidNumber>.MinMagnitudeNumber(InvalidNumber x, InvalidNumber y) => throw new NotSupportedException();
	static InvalidNumber INumberBase<InvalidNumber>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => throw new NotSupportedException();
	static InvalidNumber INumberBase<InvalidNumber>.Parse(string s, NumberStyles style, IFormatProvider? provider) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.TryConvertFromChecked<TOther>(TOther value, out InvalidNumber result) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.TryConvertFromSaturating<TOther>(TOther value, out InvalidNumber result) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.TryConvertFromTruncating<TOther>(TOther value, out InvalidNumber result) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.TryConvertToChecked<TOther>(InvalidNumber value, out TOther result) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.TryConvertToSaturating<TOther>(InvalidNumber value, out TOther result) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.TryConvertToTruncating<TOther>(InvalidNumber value, out TOther result) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out InvalidNumber result) => throw new NotSupportedException();
	static bool INumberBase<InvalidNumber>.TryParse(string? s, NumberStyles style, IFormatProvider? provider, out InvalidNumber result) => throw new NotSupportedException();
	bool IEquatable<InvalidNumber>.Equals(InvalidNumber? other) => false;
	bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotSupportedException();
	string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => throw new NotSupportedException();
	static InvalidNumber ISpanParsable<InvalidNumber>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => throw new NotSupportedException();
	static bool ISpanParsable<InvalidNumber>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out InvalidNumber result) => throw new NotSupportedException();
	static InvalidNumber IParsable<InvalidNumber>.Parse(string s, IFormatProvider? provider) => throw new NotSupportedException();
	static bool IParsable<InvalidNumber>.TryParse(string? s, IFormatProvider? provider, out InvalidNumber result) => throw new NotSupportedException();

	public static bool operator ==(InvalidNumber left, InvalidNumber right) => left is null ? right is null : left.Equals(right);

	public static bool operator !=(InvalidNumber left, InvalidNumber right) => !(left == right);

	public static bool operator <(InvalidNumber left, InvalidNumber right) => left is null ? right is not null : left.CompareTo(right) < 0;

	public static bool operator <=(InvalidNumber left, InvalidNumber right) => left is null || left.CompareTo(right) <= 0;

	public static bool operator >(InvalidNumber left, InvalidNumber right) => left is not null && left.CompareTo(right) > 0;

	public static bool operator >=(InvalidNumber left, InvalidNumber right) => left is null ? right is null : left.CompareTo(right) >= 0;

	public static InvalidNumber operator %(InvalidNumber left, InvalidNumber right) => throw new NotSupportedException();
	public static InvalidNumber operator +(InvalidNumber left, InvalidNumber right) => throw new NotSupportedException();
	public static InvalidNumber operator --(InvalidNumber value) => throw new NotSupportedException();
	public static InvalidNumber operator /(InvalidNumber left, InvalidNumber right) => throw new NotSupportedException();
	public static InvalidNumber operator ++(InvalidNumber value) => throw new NotSupportedException();
	public static InvalidNumber operator *(InvalidNumber left, InvalidNumber right) => throw new NotSupportedException();
	public static InvalidNumber operator -(InvalidNumber left, InvalidNumber right) => throw new NotSupportedException();
	public static InvalidNumber operator -(InvalidNumber value) => throw new NotSupportedException();
	public static InvalidNumber operator +(InvalidNumber value) => throw new NotSupportedException();
	static bool IComparisonOperators<InvalidNumber, InvalidNumber, bool>.operator >(InvalidNumber left, InvalidNumber right) => false;
	static bool IComparisonOperators<InvalidNumber, InvalidNumber, bool>.operator >=(InvalidNumber left, InvalidNumber right) => false;
	static bool IComparisonOperators<InvalidNumber, InvalidNumber, bool>.operator <(InvalidNumber left, InvalidNumber right) => false;
	static bool IComparisonOperators<InvalidNumber, InvalidNumber, bool>.operator <=(InvalidNumber left, InvalidNumber right) => false;
	static InvalidNumber IModulusOperators<InvalidNumber, InvalidNumber, InvalidNumber>.operator %(InvalidNumber left, InvalidNumber right) => throw new NotSupportedException();
	static InvalidNumber IAdditionOperators<InvalidNumber, InvalidNumber, InvalidNumber>.operator +(InvalidNumber left, InvalidNumber right) => throw new NotSupportedException();
	static InvalidNumber IDecrementOperators<InvalidNumber>.operator --(InvalidNumber value) => throw new NotSupportedException();
	static InvalidNumber IDivisionOperators<InvalidNumber, InvalidNumber, InvalidNumber>.operator /(InvalidNumber left, InvalidNumber right) => throw new NotSupportedException();
	static bool IEqualityOperators<InvalidNumber, InvalidNumber, bool>.operator ==(InvalidNumber? left, InvalidNumber? right) => false;
	static bool IEqualityOperators<InvalidNumber, InvalidNumber, bool>.operator !=(InvalidNumber? left, InvalidNumber? right) => false;
	static InvalidNumber IIncrementOperators<InvalidNumber>.operator ++(InvalidNumber value) => throw new NotSupportedException();
	static InvalidNumber IMultiplyOperators<InvalidNumber, InvalidNumber, InvalidNumber>.operator *(InvalidNumber left, InvalidNumber right) => throw new NotSupportedException();
	static InvalidNumber ISubtractionOperators<InvalidNumber, InvalidNumber, InvalidNumber>.operator -(InvalidNumber left, InvalidNumber right) => throw new NotSupportedException();
	static InvalidNumber IUnaryNegationOperators<InvalidNumber, InvalidNumber>.operator -(InvalidNumber value) => throw new NotSupportedException();
	static InvalidNumber IUnaryPlusOperators<InvalidNumber, InvalidNumber>.operator +(InvalidNumber value) => throw new NotSupportedException();
}
