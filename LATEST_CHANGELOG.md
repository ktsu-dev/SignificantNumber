## v3.1.0 (minor)

Changes since v3.0.0:

- Write a negative value that rounds to zero without its sign in C, P and custom formats [patch] (#152) (matt-edmondson)
- Round C, P and custom formats once, at the digits the format shows [patch] (#157) (matt-edmondson)
- Read a null format provider as the current culture everywhere [minor] (#158) (matt-edmondson)
- Convert to SignificantNumber in To\<TOutput\> without rebuilding the value [patch] (#146) (matt-edmondson)
- Format C, P and custom patterns through decimal's rounding, and refuse a value double can't hold [patch] (#148) (matt-edmondson)
- Clamp a SignificantNumber between SignificantNumber bounds [patch] (#150) (matt-edmondson)
- Compare a SignificantNumber with a PreciseNumber through Equals [minor] (#144) (matt-edmondson)
- Take the logarithm of a subnormal base from its significand and exponent [patch] (#142) (matt-edmondson)
- Write C, P and custom format strings the way decimal does [patch] (#141) (matt-edmondson)
- Return ±1 from (-1).Pow for an integer power beyond double range [patch] (Claude)
- Cap Pow and Exp digits by the precision of the double exponent [patch] (Claude)

