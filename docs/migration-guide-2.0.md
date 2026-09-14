# Migrating from SignificantNumber 1.x to 2.0

SignificantNumber 2.0 makes `SignificantNumber` a value type that holds a `PreciseNumber`, and requires `ktsu.PreciseNumber` 2.0. The significant figure rules are unchanged. Addition and subtraction round to the fewest decimal places, and multiplication, division, and modulus round to the fewest significant digits. What changes is how the type relates to `PreciseNumber`, `null`, and generic math.

## Quick checklist

1. Update `ktsu.PreciseNumber` to 2.0 alongside this package, and read its [migration guide](https://github.com/ktsu-dev/PreciseNumber/blob/main/docs/migration-guide-2.0.md).
2. Remove `null` checks and `null` assignments for `SignificantNumber` values.
3. Replace code that treats a `SignificantNumber` as a `PreciseNumber` object, such as `is PreciseNumber` checks or `ReferenceEquals`.
4. Add an explicit cast where a `PreciseNumber` is assigned to a `SignificantNumber`.
5. Check the `TryParse` failure path, which now yields zero instead of `null`.

## Why

`PreciseNumber` 2.0 is a `readonly record struct`, and a struct can't be inherited, so `SignificantNumber` can no longer derive from it. Holding one instead keeps every result inline in its variable, field, or array element, the same benefit `PreciseNumber` 2.0 gets. It also lets `SignificantNumber` satisfy `where T : struct, INumber<T>` and take part in generic math through `CreateChecked`, `CreateSaturating`, and `CreateTruncating`.

## 1. SignificantNumber is a value type

`default(SignificantNumber)` is zero, and equals `SignificantNumber.Zero`. An uninitialized field or array element is a valid number.

```csharp
// Was:
SignificantNumber? total = null;
if (total is null) { total = SignificantNumber.Zero; }

// Now:
SignificantNumber total = default; // zero
```

A `SignificantNumber?` still compiles, but it's now a `Nullable<SignificantNumber>`.

## 2. It holds a PreciseNumber instead of deriving from one

The held value is the new `Value` property. A `SignificantNumber` converts to a `PreciseNumber` implicitly, so passing one where a `PreciseNumber` is expected keeps compiling. The other direction needs an explicit cast, because it chooses the significant figure rules.

```csharp
PreciseNumber precise = 12.5.ToPreciseNumber();

// Was:
SignificantNumber significant = precise.ToSignificantNumber();
PreciseNumber back = significant;

// Now, either of these:
SignificantNumber significant = (SignificantNumber)precise;
SignificantNumber alsoSignificant = precise.ToSignificantNumber();
PreciseNumber back = significant; // still implicit
```

These members used to be inherited and are now declared on `SignificantNumber`, with the same names and return types: `Exponent`, `Significand`, `SignificantDigits`, `Abs()`, `Round(int)`, `ReduceSignificance(int)`, `Clamp`, `Squared()`, `Cubed()`, `To<T>()`, the `ToString` overloads, and `TryFormat`. `NegativeOne`, `E`, `Pi`, and `Tau` are now typed `SignificantNumber`, and so are the static `Max`, `Min`, `Clamp`, and `Round`.

Removed:

| Removed | Replacement |
|---|---|
| The protected constructors taking an exponent and a significand | `SignificantNumber.Parse` with scientific notation, or `(SignificantNumber)precise` |
| Inherited static members that weren't redeclared, such as `PreciseNumber.MakeCommonized` | Call them on `PreciseNumber` with `.Value` |
| `ReferenceEquals` identity, and `AreSame` in tests | Compare values with `==` or `Equals` |

`ToSignificantNumber()` on a value that's already a `SignificantNumber` still returns the same value, but as a copy rather than the same object.

## 3. Signatures that accepted null

| Member | 1.x | 2.0 |
|---|---|---|
| `CompareTo` | `CompareTo(SignificantNumber? other)` returned `1` for `null` | `CompareTo(SignificantNumber other)` |
| `CompareTo(object?)` | Inherited from `PreciseNumber` | Declared here. Returns `1` for `null`, and compares a boxed `SignificantNumber` or `PreciseNumber` with the significant figure rules |
| `TryParse` (four overloads) | `out SignificantNumber? result`, `null` on failure | `out SignificantNumber result`, zero on failure |
| `TryConvertFrom*` | `out SignificantNumber? result`, and always failed because `PreciseNumber` 1.x threw | `out SignificantNumber result`, converting as `PreciseNumber` 2.0 does |

```csharp
// Was:
if (SignificantNumber.TryParse(text, CultureInfo.InvariantCulture, out SignificantNumber? parsed)) { Use(parsed); }

// Now:
if (SignificantNumber.TryParse(text, CultureInfo.InvariantCulture, out SignificantNumber parsed)) { Use(parsed); }
```

## 4. Generic math conversions work

The six `TryConvertFrom*` and `TryConvertTo*` members delegate to `PreciseNumber` 2.0, so they cover every built-in numeric type and `BigInteger` with its checked, saturating, and truncating behavior. They also convert to and from `PreciseNumber` itself.

```csharp
static T ToMeters<T>(T feet) where T : INumber<T> => feet * T.CreateChecked(0.3048);

SignificantNumber meters = ToMeters(10.ToSignificantNumber()); // 3.048, rounded by the multiplication rule
double asDouble = double.CreateChecked(meters);
```

## 5. UTF-8 formatting no longer recurses

1.x declared `IUtf8SpanFormattable.TryFormat` by casting itself to the interface and calling the same method, which recursed until the stack overflowed. 2.0 uses the implementation `INumberBase` provides, which formats through `TryFormat` on characters.
