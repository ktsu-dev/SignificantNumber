# ktsu.SignificanNumber

## Changelist for Version 1.2.1
- Support for SignificantNumber exponents in the `Pow` and `Exp` methods

## Changelist for Version 1.2.0
- Added `Exp(int power)` method: Returns the result of raising e to the specified power.

## Changelist for Version 1.1.4
- Fixed incorrect result when rounding values with less precision than requested

## Changelist for Version 1.1.3
- Fixed Incorrect Exponent Handling during divisions

## Changelist for Version 1.1.2
- Fixed Incorrect Exponent Handling for Integers in SignificantNumber Conversion

## Changelist for Version 1.1.1

- **Enhanced `To<TOutput>()` method**:
  - Improved the conversion method to directly return the current instance when converting to `SignificantNumber`, enhancing performance and reducing unnecessary computation for this specific conversion case.
  - This additionally prevents an unintended `NotSupportedException` when attempting to convert a `SignificantNumber` to itself.

## Changelist for Version 1.1.0

### New Features:

- **Arithmetic Operations:**
  - Added `Squared()` method: Returns the square of the current significant number.
  - Added `Cubed()` method: Returns the cube of the current significant number.
  - Added `Pow(int power)` method: Returns the result of raising the current significant number to the specified power.

### Documentation:

- **README.md:**
  - Updated installation instructions to use a version placeholder.
  - Added examples for the new `Squared`, `Cubed`, and `Pow` methods under the Arithmetic Operations section.

### Unit Tests:

- Added unit tests to ensure the correctness of the new methods:
  - `Squared_ShouldReturnCorrectValue()`: Tests the `Squared` method.
  - `Cubed_ShouldReturnCorrectValue()`: Tests the `Cubed` method.
  - `Pow_ShouldReturnCorrectValue()`: Tests the `Pow` method with a positive power.
  - `Pow_ZeroPower_ShouldReturnOne()`: Tests the `Pow` method with zero as the power.
  - `Pow_NegativePower_ShouldReturnCorrectValue()`: Tests the `Pow` method with a negative power.

### Bug Fixes:

- Fixed a bug in the `operator /` method to correctly handle division by zero by throwing a `DivideByZeroException`.
- Fixed a bug in the `operator *` method to handle cases where the result's exponent is zero.
- Fixed a bug in the `operator /` method to correctly handle cases where the left and right significant numbers are equal by returning `One`.
- Corrected the exception type in the `To` method's unit test from `InvalidOperationException` to `OverflowException`.
