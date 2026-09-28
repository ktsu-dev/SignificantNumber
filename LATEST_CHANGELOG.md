## v2.0.13 (patch)

Changes since v2.0.12:

- Move Pow's logarithm path into a helper to keep its complexity within bounds ([@Claude](https://github.com/Claude))
- Cover an exact operand on either side of a multiplication or division ([@Claude](https://github.com/Claude))
- Count the result's integer digits without a floating-point comparison ([@Claude](https://github.com/Claude))
- Compute Pow and Exp beyond the range of a double [patch] ([@Claude](https://github.com/Claude))
- Keep the remainder of % smaller than the divisor [patch] ([@Claude](https://github.com/Claude))
- Stop multiplying by zero from throwing when the zero is on the right [patch] ([@Claude](https://github.com/Claude))

