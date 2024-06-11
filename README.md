# SignificantNumber

The `SignificantNumber` class represents a number with a significand and an exponent, enabling high-precision arithmetic operations that comply with calculation rules for significant figures. It provides a robust set of functionalities for mathematical computations and formatting.

## Features

- High-precision arithmetic operations (addition, subtraction, multiplication, division)
- Support for significant figures and exponents
- Integration with .NET numerical interfaces
- Comprehensive error handling and validation

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
  - [Creating a SignificantNumber](#creating-a-significantnumber)
  - [Arithmetic Operations](#arithmetic-operations)
  - [Comparison Operations](#comparison-operations)
  - [Formatting and Parsing](#formatting-and-parsing)
  - [Extension Methods](#extension-methods)
- [Precision](#precision)
  - [Significand and Exponent](#significand-and-exponent)
  - [Precision Handling](#precision-handling)
  - [Example of Precision](#example-of-precision)
- [API Reference](#api-reference)
- [Contributing](#contributing)
- [License](#license)

## Installation

To install the `SignificantNumber` library, you can use the .NET CLI:

```sh
dotnet add package SignificantNumber
```

Or, add the package reference directly in your project file:

```xml
<PackageReference Include="SignificantNumber" Version="1.0.0" />
```

## Usage

### Creating a SignificantNumber

You can create a `SignificantNumber` from various numeric types using the `ToSignificantNumber` extension method:

```csharp
using ktsu.io.SignificantNumber;

double floatingPointValue = 123.45;
SignificantNumber significantNumberFromFloat = floatingPointValue.ToSignificantNumber();
significantNumberFromFloat = 537.89.ToSignificantNumber();

int integerValue = 12345;
SignificantNumber significantNumberFromInt = integerValue.ToSignificantNumber();
significantNumberFromInt = 98765.ToSignificantNumber();
```

### Arithmetic Operations

You can perform various arithmetic operations on `SignificantNumber` instances:

```csharp
var result1 = number1 + number2;
var result2 = number1 - number2;
var result3 = number1 * number2;
var result4 = number1 / number2;
```

### Comparison Operations

You can compare `SignificantNumber` instances using comparison operators:

```csharp
bool isEqual = number1 == number2;
bool isGreater = number1 > number2;
bool isLessOrEqual = number1 <= number2;
```

### Formatting and Parsing

You can format a `SignificantNumber` as a string:

```csharp
string formatted = number1.ToString("G", CultureInfo.InvariantCulture);
Console.WriteLine(formatted);  // Outputs the formatted number
```

Parsing is not supported and will throw `NotSupportedException`:

```csharp
try
{
    var parsedNumber = SignificantNumber.Parse("123.45", CultureInfo.InvariantCulture);
}
catch (NotSupportedException ex)
{
    Console.WriteLine(ex.Message);
}
```

Instead you should parse the number as another numeric type and convert it to a `SignificantNumber`:

```csharp
double parsedDouble = double.Parse("123.45", CultureInfo.InvariantCulture);
SignificantNumber significantNumberFromDouble = parsedDouble.ToSignificantNumber();
```

### Extension Methods

#### `ToSignificantNumber`

Converts various numeric types to a `SignificantNumber`.

#### Usage:

```csharp
public static SignificantNumber ToSignificantNumber<TInput>(this INumber<TInput> input)
    where TInput : INumber<TInput>
```

#### Parameters:

- `input`: The input number to convert.

#### Returns:

- `SignificantNumber`: The converted `SignificantNumber`.

#### Example:

```csharp
double floatingPointValue = 123.45;
SignificantNumber significantNumberFromFloat = floatingPointValue.ToSignificantNumber();

int integerValue = 12345;
SignificantNumber significantNumberFromInt = integerValue.ToSignificantNumber();
```

## Precision

The `SignificantNumber` class is designed to handle high-precision arithmetic operations with significant figures and exponents.

Here's how it ensures precision:

### Significand and Exponent

A `SignificantNumber` consists of two main components:
- **Significand**: This is the significant part of the number, stored as a `BigInteger` to accommodate a wide range of values with high precision.
- **Exponent**: This is the exponent part of the number, which scales the significand by a power of ten.

### Precision Handling

- **Maximum Significant Digits**: When converting from a floating-point number to a `SignificantNumber` maximum number of significant digits is limited to 7 for `float` values, and 16 for `double` values.
- **Trailing Zero Removal**: The class automatically sanitizes the significand by removing trailing zeros, ensuring that the number is stored in its most compact and precise form.
- **Rounding**: You can round a `SignificantNumber` to a specified number of decimal digits, ensuring that you can control the precision of your calculations.

### Example of Precision

Consider the number `123.456000`:
- When stored as a `SignificantNumber`, it will be represented as `123456e-3` after removing the trailing zeros and adjusting the exponent accordingly.
- This ensures that the number is represented with the exact precision required for your calculations, without unnecessary trailing zeros.

By using the `SignificantNumber` class, you can perform high-precision arithmetic operations and maintain control over the significant figures and exponent, ensuring accurate and efficient mathematical computations.

## API Reference

### Properties

- `static SignificantNumber NegativeOne` - Gets the value -1 for the type.
- `static SignificantNumber One` - Gets the value 1 for the type.
- `static SignificantNumber Zero` - Gets the value 0 for the type.
- `static int Radix` - Gets the radix, or base, for the type.
- `static SignificantNumber AdditiveIdentity` - Gets the additive identity of the current type.
- `static SignificantNumber MultiplicativeIdentity` - Gets the multiplicative identity of the current type.

### Methods

- `SignificantNumber(int exponent, BigInteger significand, bool sanitize = true)` - Initializes a new instance of the `SignificantNumber` struct.
- `bool Equals(SignificantNumber other)` - Determines whether the specified object is equal to the current object.
- `int CompareTo(object? obj)` - Compares the current instance with another object.
- `int CompareTo(SignificantNumber other)` - Compares the current instance with another significant number.
- `int CompareTo<TInput>(TInput other) where TInput : INumber<TInput>` - Compares the current instance with another number.
- `SignificantNumber Abs()` - Returns the absolute value of the current instance.
- `SignificantNumber Round(int decimalDigits)` - Rounds the current instance to the specified number of decimal digits.
- `SignificantNumber Clamp<TNumber>(TNumber min, TNumber max) where TNumber : INumber<TNumber>` - Clamps the specified value between the minimum and maximum values.
- `string ToString(string? format, IFormatProvider? formatProvider)` - Converts the current instance to its equivalent string representation using the specified format and format provider.
- `bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)` - Attempts to format the current instance into the provided span.

### Static Methods

- `static SignificantNumber Abs(SignificantNumber value)` - Returns the absolute value of a `SignificantNumber`.
- `static bool IsCanonical(SignificantNumber value)` - Determines whether the specified value is canonical.
- `static bool IsComplexNumber(SignificantNumber value)` - Determines whether the specified value is a complex number.
- `static bool IsEvenInteger(SignificantNumber value)` - Determines whether the specified value is an even integer.
- `static bool IsFinite(SignificantNumber value)` - Determines whether the specified value is finite.
- `static bool IsImaginaryNumber(SignificantNumber value)` - Determines whether the specified value is an imaginary number.
- `static bool IsInfinity(SignificantNumber value)` - Determines whether the specified value is infinite.
- `static bool IsInteger(SignificantNumber value)` - Determines whether the specified value is an integer.
- `static bool IsNaN(SignificantNumber value)` - Determines whether the specified value is NaN.
- `static bool IsNegative(SignificantNumber value)` - Determines whether the specified value is negative.
- `static bool IsNegativeInfinity(SignificantNumber value)` - Determines whether the specified value is negative infinity.
- `static bool IsNormal(SignificantNumber value)` - Determines whether the specified value is normal.
- `static bool IsOddInteger(SignificantNumber value)` - Determines whether the specified value is an odd integer.
- `static bool IsPositive(SignificantNumber value)` - Determines whether the specified value is positive.
- `static bool IsPositiveInfinity(SignificantNumber value)` - Determines whether the specified value is positive infinity.
- `static bool IsRealNumber(SignificantNumber value)` - Determines whether the specified value is a real number.
- `static bool IsSubnormal(SignificantNumber value)` - Determines whether the specified value is subnormal.
- `static bool IsZero(SignificantNumber value)` - Determines whether the specified value is zero.
- `static SignificantNumber MaxMagnitude(SignificantNumber x, SignificantNumber y)` - Returns the larger of the magnitudes of two significant numbers.
- `static SignificantNumber MaxMagnitudeNumber(SignificantNumber x, SignificantNumber y)` - Returns the larger of the magnitudes of two significant numbers.
- `static SignificantNumber MinMagnitude(SignificantNumber x, SignificantNumber y)` - Returns the smaller of the magnitudes of two significant numbers.
- `static SignificantNumber MinMagnitudeNumber(SignificantNumber x, SignificantNumber y)` - Returns the smaller of the magnitudes of two significant numbers.
- `static SignificantNumber Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)` - Parses a significant number from a span of characters using the specified style and format provider.
- `static SignificantNumber Parse(string s, NumberStyles style, IFormatProvider? provider)` - Parses a significant number from a string using the specified style and format provider.
- `static SignificantNumber Parse(string s, IFormatProvider? provider)` - Parses a significant number from a string using the specified format provider.
- `static SignificantNumber Parse(ReadOnlySpan<char> s, IFormatProvider? provider)` - Parses a significant number from a span of characters using the specified format provider.
- `static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result)` - Tries to parse a significant number from a span of characters using the specified style and format provider.
- `static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result)` - Tries to parse a significant number from a string using the specified style and format provider.
- `static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result)` - Tries to parse a significant number from a string using the specified format provider.
- `static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out SignificantNumber result)` - Tries to parse a significant number from a span of characters using the specified format provider.
- `static bool TryConvertFromChecked<TOther>(TOther value, out SignificantNumber result) where TOther : INumberBase<TOther>` - Tries to convert a number to a significant number using checked conversion.
- `static bool TryConvertFrom

Saturating<TOther>(TOther value, out SignificantNumber result) where TOther : INumberBase<TOther>` - Tries to convert a number to a significant number using saturating conversion.
- `static bool TryConvertFromTruncating<TOther>(TOther value, out SignificantNumber result) where TOther : INumberBase<TOther>` - Tries to convert a number to a significant number using truncating conversion.
- `static bool TryConvertToChecked<TOther>(SignificantNumber value, out TOther result) where TOther : INumberBase<TOther>` - Tries to convert a significant number to another number using checked conversion.
- `static bool TryConvertToSaturating<TOther>(SignificantNumber value, out TOther result) where TOther : INumberBase<TOther>` - Tries to convert a significant number to another number using saturating conversion.
- `static bool TryConvertToTruncating<TOther>(SignificantNumber value, out TOther result) where TOther : INumberBase<TOther>` - Tries to convert a significant number to another number using truncating conversion.
- `static void AssertExponentsMatch(SignificantNumber left, SignificantNumber right)` - Asserts that the exponents of two significant numbers match.
- `static bool DoesImplementGenericInterface(Type type, Type genericInterface)` - Determines whether a type implements a specified generic interface.
- `static void AssertDoesImplementGenericInterface(Type type, Type genericInterface)` - Asserts that a type implements a specified generic interface.

### Operators

- `static SignificantNumber operator -(SignificantNumber value)` - Negates a significant number.
- `static SignificantNumber operator -(SignificantNumber left, SignificantNumber right)` - Subtracts one significant number from another.
- `static bool operator !=(SignificantNumber left, SignificantNumber right)` - Determines whether two significant numbers are not equal.
- `static SignificantNumber operator *(SignificantNumber left, SignificantNumber right)` - Multiplies two significant numbers.
- `static SignificantNumber operator /(SignificantNumber left, SignificantNumber right)` - Divides one significant number by another.
- `static SignificantNumber operator +(SignificantNumber value)` - Returns the unary plus of a significant number.
- `static SignificantNumber operator +(SignificantNumber left, SignificantNumber right)` - Adds two significant numbers.
- `static bool operator ==(SignificantNumber left, SignificantNumber right)` - Determines whether two significant numbers are equal.
- `static bool operator >(SignificantNumber left, SignificantNumber right)` - Determines whether one significant number is greater than another.
- `static bool operator <(SignificantNumber left, SignificantNumber right)` - Determines whether one significant number is less than another.
- `static bool operator >=(SignificantNumber left, SignificantNumber right)` - Determines whether one significant number is greater than or equal to another.
- `static bool operator <=(SignificantNumber left, SignificantNumber right)` - Determines whether one significant number is less than or equal to another.
- `static SignificantNumber operator %(SignificantNumber left, SignificantNumber right)` - Computes the modulus of two significant numbers (not supported).
- `static SignificantNumber operator --(SignificantNumber value)` - Decrements a significant number (not supported).
- `static SignificantNumber operator ++(SignificantNumber value)` - Increments a significant number (not supported).

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
