namespace ktsu.io.SignificantNumber;

using System.Numerics;

public static class SignificantNumberExtensions
{
	public static SignificantNumber ToSignificantNumber(this Half input) =>
		SignificantNumber.CreateFromFloatingPoint(input);
	public static SignificantNumber ToSignificantNumber(this float input) =>
		SignificantNumber.CreateFromFloatingPoint(input);
	public static SignificantNumber ToSignificantNumber(this double input) =>
		SignificantNumber.CreateFromFloatingPoint(input);
	public static SignificantNumber ToSignificantNumber(this decimal input) =>
		SignificantNumber.CreateFromFloatingPoint(input);

	public static SignificantNumber ToSignificantNumber(this sbyte input) =>
		SignificantNumber.CreateFromInteger(input);
	public static SignificantNumber ToSignificantNumber(this byte input) =>
		SignificantNumber.CreateFromInteger(input);
	public static SignificantNumber ToSignificantNumber(this short input) =>
		SignificantNumber.CreateFromInteger(input);
	public static SignificantNumber ToSignificantNumber(this ushort input) =>
		SignificantNumber.CreateFromInteger(input);
	public static SignificantNumber ToSignificantNumber(this int input) =>
		SignificantNumber.CreateFromInteger(input);
	public static SignificantNumber ToSignificantNumber(this uint input) =>
		SignificantNumber.CreateFromInteger(input);
	public static SignificantNumber ToSignificantNumber(this long input) =>
		SignificantNumber.CreateFromInteger(input);
	public static SignificantNumber ToSignificantNumber(this ulong input) =>
		SignificantNumber.CreateFromInteger(input);
	public static SignificantNumber ToSignificantNumber(this BigInteger input) =>
		SignificantNumber.CreateFromInteger(input);
}
