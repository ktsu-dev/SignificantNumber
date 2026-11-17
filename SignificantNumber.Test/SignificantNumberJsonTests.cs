// Copyright (c) 2023-2026 ktsu-dev contributors

namespace SignificantNumber.Test;

using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using ktsu.SignificantNumber;

/// <summary>
/// Covers reading and writing <see cref="SignificantNumber"/> with System.Text.Json (#159).
/// </summary>
[TestClass]
public class SignificantNumberJsonTests
{
	private sealed record Reading(string Name, SignificantNumber Value);

	private static SignificantNumber Parse(string text) =>
		SignificantNumber.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);

	private static void AssertSame(SignificantNumber expected, SignificantNumber actual)
	{
		Assert.AreEqual(expected, actual);
		Assert.AreEqual(expected.SignificantDigits, actual.SignificantDigits);
		Assert.AreEqual(expected.Exponent, actual.Exponent);
	}

	[TestMethod]
	public void Serialize_WritesTheValueAsAString()
	{
		Assert.AreEqual("\"9.81\"", JsonSerializer.Serialize(Parse("9.81")));
	}

	[TestMethod]
	public void Serialize_DifferentValues_WriteDifferentJson()
	{
		Assert.AreNotEqual(JsonSerializer.Serialize(Parse("1.25")), JsonSerializer.Serialize(Parse("7.25")));
	}

	[TestMethod]
	public void RoundTrip_InsideARecord_KeepsTheValue()
	{
		Reading reading = new("g", Parse("9.81"));

		string json = JsonSerializer.Serialize(reading);
		Reading back = JsonSerializer.Deserialize<Reading>(json)!;

		Assert.AreEqual("{\"Name\":\"g\",\"Value\":\"9.81\"}", json);
		Assert.AreEqual(reading, back);
		AssertSame(reading.Value, back.Value);
	}

	[TestMethod]
	[DataRow("9.81")]
	[DataRow("1200")]
	[DataRow("1.2E3")]
	[DataRow("1E400")]
	[DataRow("1E-400")]
	[DataRow("-3.14159")]
	[DataRow("-1E-400")]
	[DataRow("0")]
	[DataRow("1")]
	[DataRow("-1")]
	[DataRow("123456789012345678901234567890.123456789")]
	public void RoundTrip_KeepsValueAndSignificantDigits(string text)
	{
		SignificantNumber value = Parse(text);

		SignificantNumber back = JsonSerializer.Deserialize<SignificantNumber>(JsonSerializer.Serialize(value));

		AssertSame(value, back);
	}

	[TestMethod]
	public void RoundTrip_Default_IsZero()
	{
		SignificantNumber back = JsonSerializer.Deserialize<SignificantNumber>(JsonSerializer.Serialize(default(SignificantNumber)));

		AssertSame(SignificantNumber.Zero, back);
	}

	[TestMethod]
	[DataRow("1.25")]
	[DataRow("1.20")]
	[DataRow("1200")]
	[DataRow("-4.5e-3")]
	[DataRow("1E400")]
	public void Deserialize_JsonNumber_ReadsTheTextAsWritten(string text)
	{
		AssertSame(Parse(text), JsonSerializer.Deserialize<SignificantNumber>(text));
	}

	[TestMethod]
	[DataRow("1.25")]
	[DataRow("1.20")]
	[DataRow("-4.5e-3")]
	public void Deserialize_JsonString_ParsesInvariantCulture(string text)
	{
		AssertSame(Parse(text), JsonSerializer.Deserialize<SignificantNumber>($"\"{text}\""));
	}

	[TestMethod]
	public void Serialize_IgnoresTheCurrentCulture()
	{
		CultureInfo original = CultureInfo.CurrentCulture;
		try
		{
			// Built from the invariant culture, like de-DE, so the test doesn't depend on the host's culture data.
			CultureInfo commaDecimal = (CultureInfo)CultureInfo.InvariantCulture.Clone();
			commaDecimal.NumberFormat.NumberDecimalSeparator = ",";
			commaDecimal.NumberFormat.NumberGroupSeparator = ".";
			CultureInfo.CurrentCulture = commaDecimal;

			string json = JsonSerializer.Serialize(Parse("9.81"));

			Assert.AreEqual("\"9.81\"", json);
			AssertSame(Parse("9.81"), JsonSerializer.Deserialize<SignificantNumber>(json));
		}
		finally
		{
			CultureInfo.CurrentCulture = original;
		}
	}

	[TestMethod]
	[DataRow("\"abc\"")]
	[DataRow("\"\"")]
	[DataRow("true")]
	[DataRow("{}")]
	[DataRow("[]")]
	public void Deserialize_NotANumber_ThrowsJsonException(string json)
	{
		Assert.ThrowsExactly<JsonException>(() => JsonSerializer.Deserialize<SignificantNumber>(json));
	}

	[TestMethod]
	public void Nullable_RoundTripsNullAndValue()
	{
		Assert.AreEqual("null", JsonSerializer.Serialize<SignificantNumber?>(null));
		Assert.IsNull(JsonSerializer.Deserialize<SignificantNumber?>("null"));
		Assert.AreEqual(Parse("2.5"), JsonSerializer.Deserialize<SignificantNumber?>("\"2.5\""));
	}

	[TestMethod]
	public void DictionaryKey_RoundTrips()
	{
		Dictionary<SignificantNumber, string> map = new() { [Parse("9.81")] = "g", [Parse("1E-400")] = "tiny" };

		string json = JsonSerializer.Serialize(map);
		Dictionary<SignificantNumber, string> back = JsonSerializer.Deserialize<Dictionary<SignificantNumber, string>>(json)!;

		Assert.HasCount(2, back);
		Assert.AreEqual("g", back[Parse("9.81")]);
		Assert.AreEqual("tiny", back[Parse("1E-400")]);
	}
}
