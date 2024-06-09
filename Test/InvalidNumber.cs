namespace ktsu.io.SignificantNumber.Test;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

public partial class Tests
{
	public sealed class InvalidNumber : INumber<InvalidNumber>
	{
		public static InvalidNumber One => throw new NotImplementedException();

		public static int Radix => throw new NotImplementedException();

		public static InvalidNumber Zero => throw new NotImplementedException();

		public static InvalidNumber AdditiveIdentity => throw new NotImplementedException();

		public static InvalidNumber MultiplicativeIdentity => throw new NotImplementedException();

		static InvalidNumber INumberBase<InvalidNumber>.One => throw new NotImplementedException();

		static int INumberBase<InvalidNumber>.Radix => throw new NotImplementedException();

		static InvalidNumber INumberBase<InvalidNumber>.Zero => throw new NotImplementedException();

		static InvalidNumber IAdditiveIdentity<InvalidNumber, InvalidNumber>.AdditiveIdentity => throw new NotImplementedException();

		static InvalidNumber IMultiplicativeIdentity<InvalidNumber, InvalidNumber>.MultiplicativeIdentity => throw new NotImplementedException();

		public override bool Equals(object obj) => ReferenceEquals(this, obj) || (obj is null ? false : throw new NotImplementedException());

		public override int GetHashCode() => throw new NotImplementedException();

		public int CompareTo(object? obj) => throw new NotImplementedException();
		public int CompareTo(InvalidNumber? other) => throw new NotImplementedException();
		public static InvalidNumber Abs(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsCanonical(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsComplexNumber(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsEvenInteger(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsFinite(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsImaginaryNumber(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsInfinity(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsInteger(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsNaN(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsNegative(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsNegativeInfinity(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsNormal(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsOddInteger(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsPositive(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsPositiveInfinity(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsRealNumber(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsSubnormal(InvalidNumber value) => throw new NotImplementedException();
		public static bool IsZero(InvalidNumber value) => throw new NotImplementedException();
		public static InvalidNumber MaxMagnitude(InvalidNumber x, InvalidNumber y) => throw new NotImplementedException();
		public static InvalidNumber MaxMagnitudeNumber(InvalidNumber x, InvalidNumber y) => throw new NotImplementedException();
		public static InvalidNumber MinMagnitude(InvalidNumber x, InvalidNumber y) => throw new NotImplementedException();
		public static InvalidNumber MinMagnitudeNumber(InvalidNumber x, InvalidNumber y) => throw new NotImplementedException();
		public static InvalidNumber Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => throw new NotImplementedException();
		public static InvalidNumber Parse(string s, NumberStyles style, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out InvalidNumber result) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out InvalidNumber result) => throw new NotImplementedException();
		public bool Equals(InvalidNumber? other) => throw new NotImplementedException();
		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotImplementedException();
		public string ToString(string? format, IFormatProvider? formatProvider) => throw new NotImplementedException();
		public static InvalidNumber Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out InvalidNumber result) => throw new NotImplementedException();
		public static InvalidNumber Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out InvalidNumber result) => throw new NotImplementedException();
		int IComparable.CompareTo(object? obj) => throw new NotImplementedException();
		int IComparable<InvalidNumber>.CompareTo(InvalidNumber? other) => throw new NotImplementedException();
		static InvalidNumber INumberBase<InvalidNumber>.Abs(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsCanonical(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsComplexNumber(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsEvenInteger(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsFinite(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsImaginaryNumber(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsInfinity(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsInteger(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsNaN(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsNegative(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsNegativeInfinity(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsNormal(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsOddInteger(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsPositive(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsPositiveInfinity(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsRealNumber(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsSubnormal(InvalidNumber value) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.IsZero(InvalidNumber value) => throw new NotImplementedException();
		static InvalidNumber INumberBase<InvalidNumber>.MaxMagnitude(InvalidNumber x, InvalidNumber y) => throw new NotImplementedException();
		static InvalidNumber INumberBase<InvalidNumber>.MaxMagnitudeNumber(InvalidNumber x, InvalidNumber y) => throw new NotImplementedException();
		static InvalidNumber INumberBase<InvalidNumber>.MinMagnitude(InvalidNumber x, InvalidNumber y) => throw new NotImplementedException();
		static InvalidNumber INumberBase<InvalidNumber>.MinMagnitudeNumber(InvalidNumber x, InvalidNumber y) => throw new NotImplementedException();
		static InvalidNumber INumberBase<InvalidNumber>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => throw new NotImplementedException();
		static InvalidNumber INumberBase<InvalidNumber>.Parse(string s, NumberStyles style, IFormatProvider? provider) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.TryConvertFromChecked<TOther>(TOther value, out InvalidNumber result) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.TryConvertFromSaturating<TOther>(TOther value, out InvalidNumber result) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.TryConvertFromTruncating<TOther>(TOther value, out InvalidNumber result) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.TryConvertToChecked<TOther>(InvalidNumber value, out TOther result) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.TryConvertToSaturating<TOther>(InvalidNumber value, out TOther result) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.TryConvertToTruncating<TOther>(InvalidNumber value, out TOther result) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out InvalidNumber result) => throw new NotImplementedException();
		static bool INumberBase<InvalidNumber>.TryParse(string? s, NumberStyles style, IFormatProvider? provider, out InvalidNumber result) => throw new NotImplementedException();
		bool IEquatable<InvalidNumber>.Equals(InvalidNumber? other) => throw new NotImplementedException();
		bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotImplementedException();
		string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => throw new NotImplementedException();
		static InvalidNumber ISpanParsable<InvalidNumber>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => throw new NotImplementedException();
		static bool ISpanParsable<InvalidNumber>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out InvalidNumber result) => throw new NotImplementedException();
		static InvalidNumber IParsable<InvalidNumber>.Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		static bool IParsable<InvalidNumber>.TryParse(string? s, IFormatProvider? provider, out InvalidNumber result) => throw new NotImplementedException();

		public static bool operator ==(InvalidNumber left, InvalidNumber right) => left is null ? right is null : left.Equals(right);

		public static bool operator !=(InvalidNumber left, InvalidNumber right) => !(left == right);

		public static bool operator <(InvalidNumber left, InvalidNumber right) => left is null ? right is not null : left.CompareTo(right) < 0;

		public static bool operator <=(InvalidNumber left, InvalidNumber right) => left is null || left.CompareTo(right) <= 0;

		public static bool operator >(InvalidNumber left, InvalidNumber right) => left is not null && left.CompareTo(right) > 0;

		public static bool operator >=(InvalidNumber left, InvalidNumber right) => left is null ? right is null : left.CompareTo(right) >= 0;

		public static InvalidNumber operator %(InvalidNumber left, InvalidNumber right) => throw new NotImplementedException();
		public static InvalidNumber operator +(InvalidNumber left, InvalidNumber right) => throw new NotImplementedException();
		public static InvalidNumber operator --(InvalidNumber value) => throw new NotImplementedException();
		public static InvalidNumber operator /(InvalidNumber left, InvalidNumber right) => throw new NotImplementedException();
		public static InvalidNumber operator ++(InvalidNumber value) => throw new NotImplementedException();
		public static InvalidNumber operator *(InvalidNumber left, InvalidNumber right) => throw new NotImplementedException();
		public static InvalidNumber operator -(InvalidNumber left, InvalidNumber right) => throw new NotImplementedException();
		public static InvalidNumber operator -(InvalidNumber value) => throw new NotImplementedException();
		public static InvalidNumber operator +(InvalidNumber value) => throw new NotImplementedException();
		static bool IComparisonOperators<InvalidNumber, InvalidNumber, bool>.operator >(InvalidNumber left, InvalidNumber right) => throw new NotImplementedException();
		static bool IComparisonOperators<InvalidNumber, InvalidNumber, bool>.operator >=(InvalidNumber left, InvalidNumber right) => throw new NotImplementedException();
		static bool IComparisonOperators<InvalidNumber, InvalidNumber, bool>.operator <(InvalidNumber left, InvalidNumber right) => throw new NotImplementedException();
		static bool IComparisonOperators<InvalidNumber, InvalidNumber, bool>.operator <=(InvalidNumber left, InvalidNumber right) => throw new NotImplementedException();
		static InvalidNumber IModulusOperators<InvalidNumber, InvalidNumber, InvalidNumber>.operator %(InvalidNumber left, InvalidNumber right) => throw new NotImplementedException();
		static InvalidNumber IAdditionOperators<InvalidNumber, InvalidNumber, InvalidNumber>.operator +(InvalidNumber left, InvalidNumber right) => throw new NotImplementedException();
		static InvalidNumber IDecrementOperators<InvalidNumber>.operator --(InvalidNumber value) => throw new NotImplementedException();
		static InvalidNumber IDivisionOperators<InvalidNumber, InvalidNumber, InvalidNumber>.operator /(InvalidNumber left, InvalidNumber right) => throw new NotImplementedException();
		static bool IEqualityOperators<InvalidNumber, InvalidNumber, bool>.operator ==(InvalidNumber? left, InvalidNumber? right) => throw new NotImplementedException();
		static bool IEqualityOperators<InvalidNumber, InvalidNumber, bool>.operator !=(InvalidNumber? left, InvalidNumber? right) => throw new NotImplementedException();
		static InvalidNumber IIncrementOperators<InvalidNumber>.operator ++(InvalidNumber value) => throw new NotImplementedException();
		static InvalidNumber IMultiplyOperators<InvalidNumber, InvalidNumber, InvalidNumber>.operator *(InvalidNumber left, InvalidNumber right) => throw new NotImplementedException();
		static InvalidNumber ISubtractionOperators<InvalidNumber, InvalidNumber, InvalidNumber>.operator -(InvalidNumber left, InvalidNumber right) => throw new NotImplementedException();
		static InvalidNumber IUnaryNegationOperators<InvalidNumber, InvalidNumber>.operator -(InvalidNumber value) => throw new NotImplementedException();
		static InvalidNumber IUnaryPlusOperators<InvalidNumber, InvalidNumber>.operator +(InvalidNumber value) => throw new NotImplementedException();
	}
}
