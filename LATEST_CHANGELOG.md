## v3.0.0 (major)

Changes since v2.0.0:

- Use Assert.AreSequenceEqual and Assert.Contains in the new comparison tests ([@Claude](https://github.com/Claude))
- Compare exact values in IComparable so sorting is a total order [major] ([@Claude](https://github.com/Claude))
- Round sums and differences to tens and hundreds when an operand is that coarse [patch] ([@Claude](https://github.com/Claude))
- Move CI onto the shared ci-shared.yml pipeline ([@Claude](https://github.com/Claude))
- Move Pow's logarithm path into a helper to keep its complexity within bounds ([@Claude](https://github.com/Claude))
- Cover an exact operand on either side of a multiplication or division ([@Claude](https://github.com/Claude))
- Count the result's integer digits without a floating-point comparison ([@Claude](https://github.com/Claude))
- Compute Pow and Exp beyond the range of a double [patch] ([@Claude](https://github.com/Claude))
- Keep the remainder of % smaller than the divisor [patch] ([@Claude](https://github.com/Claude))
- Stop multiplying by zero from throwing when the zero is on the right [patch] ([@Claude](https://github.com/Claude))
- Keep the base's precision when Pow or Exp takes an integer exponent [patch] ([@Claude](https://github.com/Claude))
- Keep the sign of a negative base in Pow, and reject undefined powers [patch] ([@Claude](https://github.com/Claude))
- Seed the abstraction-cost pair across every release [patch] ([@Claude](https://github.com/Claude))
- Measure this type against a bare double, and chart it per release [patch] ([@Claude](https://github.com/Claude))
- Seed the chart with every release the suite can measure [patch] ([@Claude](https://github.com/Claude))
- Chart performance per release, and show it in the README [patch] ([@Claude](https://github.com/Claude))
- Gate Dependabot auto-merge on CI actually being green ([@Claude](https://github.com/Claude))

