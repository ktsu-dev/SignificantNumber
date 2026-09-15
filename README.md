# ktsu.SignificantNumber

![NuGet Version](https://img.shields.io/nuget/v/ktsu.SignificantNumber?logo=nuget&label=stable)
![NuGet Version](https://img.shields.io/nuget/vpre/ktsu.SignificantNumber?logo=nuget&label=dev)
![GitHub commit activity](https://img.shields.io/github/commit-activity/m/ktsu-dev/SignificantNumber?label=commits)
![GitHub branch status](https://img.shields.io/github/checks-status/ktsu-dev/SignificantNumber/main)

[![License](https://img.shields.io/github/license/ktsu-dev/SignificantNumber.svg?label=License&logo=nuget)](LICENSE.md)
[![NuGet Version](https://img.shields.io/nuget/v/ktsu.SignificantNumber?label=Stable&logo=nuget)](https://nuget.org/packages/ktsu.SignificantNumber)
[![NuGet Version](https://img.shields.io/nuget/vpre/ktsu.SignificantNumber?label=Latest&logo=nuget)](https://nuget.org/packages/ktsu.SignificantNumber)
[![NuGet Downloads](https://img.shields.io/nuget/dt/ktsu.SignificantNumber?label=Downloads&logo=nuget)](https://nuget.org/packages/ktsu.SignificantNumber)
[![GitHub commit activity](https://img.shields.io/github/commit-activity/m/ktsu-dev/SignificantNumber?label=Commits&logo=github)](https://github.com/ktsu-dev/SignificantNumber/commits/main)
[![GitHub contributors](https://img.shields.io/github/contributors/ktsu-dev/SignificantNumber?label=Contributors&logo=github)](https://github.com/ktsu-dev/SignificantNumber/graphs/contributors)
[![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/ktsu-dev/SignificantNumber/dotnet.yml?branch=main&label=Build&logo=github)](https://github.com/ktsu-dev/SignificantNumber/actions)

`SignificantNumber` is a numeric value type whose arithmetic follows the rules for significant figures. It holds a [`ktsu.PreciseNumber`](https://github.com/ktsu-dev/PreciseNumber) and rounds every result to the precision its operands justify.

## Features

- Addition and subtraction round to the fewest decimal places among the operands, and multiplication, division, and modulus round to the fewest significant digits
- Operands of exactly -1, 0, or 1 have unlimited precision, so they never limit a result
- A `readonly record struct` whose `default` is zero, holding a `PreciseNumber` that it converts to implicitly
- Implements `INumber<SignificantNumber>`, including `CreateChecked`, `CreateSaturating`, and `CreateTruncating` for every built-in numeric type, `BigInteger`, and `PreciseNumber`

Upgrading from 1.x? See the [2.0 migration guide](docs/migration-guide-2.0.md).

## Table of contents

- [Installation](#installation)
- [Usage](#usage)
  - [Creating a SignificantNumber](#creating-a-significantnumber)
    - [Supported numeric types](#supported-numeric-types)
    - [Examples](#examples)
  - [Arithmetic operations](#arithmetic-operations)
  - [Comparison operations](#comparison-operations)
  - [Formatting and parsing](#formatting-and-parsing)
  - [Extension methods](#extension-methods)
  - [Conversion](#conversion)
- [Precision](#precision)
  - [Significand and exponent](#significand-and-exponent)
  - [Precision handling](#precision-handling)
  - [Example of precision](#example-of-precision)
- [API reference](#api-reference)
- [Contributing](#contributing)
- [License](#license)

## Installation

Install the package with the .NET CLI:

```sh
dotnet add package ktsu.SignificantNumber
```

Or add the package reference directly in your project file:

```xml
<PackageReference Include="ktsu.SignificantNumber" Version="x.x.x" />
```

## Usage

### Creating a SignificantNumber

Create a `SignificantNumber` from any supported numeric type with the `ToSignificantNumber` extension method, from text with `Parse`, or from a `PreciseNumber` with an explicit cast.

#### Supported numeric types

`ToSignificantNumber` converts through `INumber<T>`. These types are supported:

- **Integer types**:
  - `int`
  - `long`
  - `short`
  - `sbyte`
  - `uint`
  - `ulong`
  - `ushort`
  - `byte`
  - `BigInteger`

- **Floating point types**:
  - `double`
  - `float`
  - `Half`
  - `decimal`

- **ktsu types**:
  - `PreciseNumber`
  - `SignificantNumber`

### Examples

```csharp
using System.Numerics;
using ktsu.PreciseNumber;
using ktsu.SignificantNumber;

// Integer types
int intValue = 12345;
SignificantNumber significantNumberFromInt = intValue.ToSignificantNumber();

BigInteger bigIntValue = new BigInteger(9876543210);
SignificantNumber significantNumberFromBigInt = bigIntValue.ToSignificantNumber();

// Floating point types
double doubleValue = 123.45;
SignificantNumber significantNumberFromDouble = doubleValue.ToSignificantNumber();

Half halfValue = (Half)123.45;
SignificantNumber significantNumberFromHalf = halfValue.ToSignificantNumber();

float floatValue = 123.45f;
SignificantNumber significantNumberFromFloat = floatValue.ToSignificantNumber();

decimal decimalValue = 123.45m;
SignificantNumber significantNumberFromDecimal = decimalValue.ToSignificantNumber();

// A PreciseNumber, with an explicit cast because it opts into the significant figure rules
PreciseNumber precise = 12.5.ToPreciseNumber();
SignificantNumber significantNumberFromPrecise = (SignificantNumber)precise;
```

### Arithmetic operations

```csharp
SignificantNumber result1 = number1 + number2;
SignificantNumber result2 = number1 - number2;
SignificantNumber result3 = number1 * number2;
SignificantNumber result4 = number1 / number2;

// Square and cube operations, which return the unrounded PreciseNumber
PreciseNumber squared = number1.Squared();
PreciseNumber cubed = number1.Cubed();

// Power operation
SignificantNumber powerResult = number1.Pow(3.ToPreciseNumber());
```

The operators also accept a `PreciseNumber` on either side, and the result is a `SignificantNumber`.

### Comparison operations

```csharp
bool isEqual = number1 == number2;
bool isGreater = number1 > number2;
bool isLessOrEqual = number1 <= number2;
```

Ordering operators compare values exactly. `SignificantNumber.CompareTo(left, right)` and `CompareTo(SignificantNumber)` compare both numbers at the lower of their significant digit counts.

### Formatting and parsing

Format a `SignificantNumber` as a string:

```csharp
string formatted = number1.ToString("G", CultureInfo.InvariantCulture);
Console.WriteLine(formatted);  // Outputs the formatted number
```

Parse one from text, including scientific notation:

```csharp
SignificantNumber parsed = SignificantNumber.Parse("1.23E4", NumberStyles.Float, CultureInfo.InvariantCulture);

if (SignificantNumber.TryParse("123.45", CultureInfo.InvariantCulture, out SignificantNumber result))
{
    Console.WriteLine(result);
}
```

`TryParse` yields zero when parsing fails.

### Extension methods

#### `ToSignificantNumber`

Converts a supported numeric type to a `SignificantNumber`. An overload takes the number of significant digits to keep.

#### Usage

```csharp
public static SignificantNumber ToSignificantNumber<TInput>(this TInput input)
    where TInput : INumber<TInput>

public static SignificantNumber ToSignificantNumber<TInput>(this TInput input, int significantDigits)
    where TInput : INumber<TInput>
```

#### Parameters

- `input`: The number to convert.
- `significantDigits`: The number of significant digits to keep. It must be greater than zero.

#### Returns

- `SignificantNumber`: The converted number.

#### Example

```csharp
double floatingPointValue = 123.45;
SignificantNumber significantNumberFromFloat = floatingPointValue.ToSignificantNumber();

int integerValue = 12345;
SignificantNumber significantNumberFromInt = integerValue.ToSignificantNumber();

SignificantNumber result = significantNumberFromFloat + significantNumberFromInt;
// result = 12468.45
```

### Conversion

#### `To<TOutput>`

Converts a `SignificantNumber` to the specified numeric type.

#### Usage

```csharp
public TOutput To<TOutput>()
    where TOutput : INumber<TOutput>
```

#### Returns

- `TOutput`: The converted value of the `SignificantNumber`.

#### Example

```csharp
SignificantNumber significantNumber = SignificantNumber.Parse("12345E3", NumberStyles.Float, CultureInfo.InvariantCulture);
double result = significantNumber.To<double>();
Console.WriteLine(result);  // Outputs 12345000
```

Generic code converts the same way through `CreateChecked`, `CreateSaturating`, and `CreateTruncating`:

```csharp
static T ToMeters<T>(T feet) where T : INumber<T> => feet * T.CreateChecked(0.3048);

SignificantNumber meters = ToMeters(10.ToSignificantNumber());
double asDouble = double.CreateChecked(meters);
```

A `SignificantNumber` converts to a `PreciseNumber` implicitly, and `Value` returns the `PreciseNumber` it holds.

## Precision

### Significand and exponent

A `SignificantNumber` holds a `PreciseNumber`, which stores two components:

- **Significand**: The significant digits of the number, stored as a `BigInteger`.
- **Exponent**: The power of ten that scales the significand.

`Significand`, `Exponent`, and `SignificantDigits` are available directly on `SignificantNumber`.

### Precision handling

- **Floating point input**: A `float` keeps up to 8 significant digits, and a `double` up to 16.
- **Trailing zero removal**: Trailing zeros move from the significand into the exponent, so every value is stored in its most compact form.
- **Rounding**: `Round` rounds to a number of decimal digits, and `ReduceSignificance` to a number of significant digits.

### Example of precision

Consider the number `123.456000`:

- As a `SignificantNumber`, it's stored as `123456e-3` after removing the trailing zeros and adjusting the exponent.
- Adding `1.2` to it rounds the sum to one decimal place, giving `124.7`, because `1.2` has the fewest decimal places.

## API reference

### Properties

- `PreciseNumber Value` - Gets the `PreciseNumber` the number holds.
- `int Exponent`, `BigInteger Significand`, and `int SignificantDigits` - Get the components of the held value.
- `static SignificantNumber NegativeOne`, `One`, and `Zero` - Get -1, 1, and 0. `Zero` is also `default`.
- `static SignificantNumber E`, `Pi`, and `Tau` - Get the mathematical constants.
- `static int Radix` - Gets the radix, or base, for the type.
- `static SignificantNumber AdditiveIdentity` - Gets the additive identity of the type.
- `static SignificantNumber MultiplicativeIdentity` - Gets the multiplicative identity of the type.

### Methods

- `bool Equals(SignificantNumber other)` - Determines whether two numbers have the same significand and exponent.
- `int CompareTo(object? obj)` - Compares the current instance with another object.
- `int CompareTo(SignificantNumber other)` - Compares the current instance with another significant number at the lower of their significant digit counts.
- `int CompareTo<TInput>(TInput other) where TInput : INumber<TInput>` - Compares the value of the current instance with another number.
- `PreciseNumber Abs()` - Returns the absolute value of the current instance.
- `PreciseNumber Round(int decimalDigits)` - Rounds the current instance to the specified number of decimal digits.
- `PreciseNumber ReduceSignificance(int significantDigits)` - Reduces the current instance to the specified number of significant digits.
- `PreciseNumber Clamp<TNumber>(TNumber min, TNumber max) where TNumber : INumber<TNumber>` - Clamps the current instance between the minimum and maximum values.
- `SignificantNumber Pow(PreciseNumber power)` - Raises the current instance to a power.
- `PreciseNumber ToPreciseNumber()` - Returns the `PreciseNumber` the number holds.
- `string ToString(string? format, IFormatProvider? formatProvider)` - Converts the current instance to a string using the specified format and format provider.
- `bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)` - Attempts to format the current instance into the provided span.
- `TOutput To<TOutput>() where TOutput : INumber<TOutput>` - Converts the current significant number to the specified numeric type.

### Static methods

- `static SignificantNumber FromPreciseNumber(PreciseNumber value)` - Creates a significant number that holds a `PreciseNumber`.
- `static SignificantNumber Add`, `Subtract`, `Multiply`, `Divide`, and `Mod(PreciseNumber left, PreciseNumber right)` - Apply the significant figure rules to two numbers.
- `static SignificantNumber Exp(PreciseNumber power)` - Raises e to a power.
- `static SignificantNumber Max`, `Min(SignificantNumber x, SignificantNumber y)`, and `Clamp(SignificantNumber value, SignificantNumber min, SignificantNumber max)` - Compare by value.
- `static SignificantNumber Round(SignificantNumber value, int decimalDigits)` - Rounds a number to the specified number of decimal digits.
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

### Operators

- `static implicit operator PreciseNumber(SignificantNumber value)` - Converts to the held `PreciseNumber`.
- `static explicit operator SignificantNumber(PreciseNumber value)` - Creates a significant number that holds a `PreciseNumber`.
- `static SignificantNumber operator -(SignificantNumber value)` - Negates a significant number.
- `static SignificantNumber operator +`, `-`, `*`, `/`, and `%` - Apply the significant figure rules. Each also accepts a `PreciseNumber` on either side.
- `static SignificantNumber operator +(SignificantNumber value)` - Returns the unary plus of a significant number.
- `static SignificantNumber operator ++` and `--` - Increment and decrement by one.
- `static bool operator ==` and `!=` - Determine whether two numbers are equal, including against a `PreciseNumber`.
- `static bool operator >`, `<`, `>=`, and `<=` - Compare two numbers, including against a `PreciseNumber`.

## Contributing

Contributions are welcome. Submit a pull request or open an issue.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE.md) file for details.
