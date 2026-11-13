// Copyright (c) 2023-2026 ktsu-dev contributors

namespace ktsu.SignificantNumber;

using System;
using System.Buffers;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Reads and writes a <see cref="SignificantNumber"/> as JSON.
/// </summary>
/// <remarks>
/// A value is written as a JSON string holding its round-trip ("R") form in the invariant culture, such as
/// <c>"9.81"</c>, so every digit survives and a reader in any language can parse it. A value is read from either a
/// JSON string or a JSON number, parsed from its text with <see cref="NumberStyles.Float"/> in the invariant culture,
/// so a number such as <c>1.20</c> is read as written rather than through <see cref="double"/>.
/// </remarks>
public sealed class SignificantNumberJsonConverter : JsonConverter<SignificantNumber>
{
	private const NumberStyles Styles = NumberStyles.Float;

	/// <inheritdoc/>
	public override SignificantNumber Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		reader.TokenType switch
		{
			JsonTokenType.String => ParseText(reader.GetString()!),
			JsonTokenType.Number => ParseText(GetRawText(ref reader)),
			_ => throw new JsonException(
				$"A {nameof(SignificantNumber)} must be a JSON string or number, but the token is {reader.TokenType}."),
		};

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, SignificantNumber value, JsonSerializerOptions options)
	{
		Ensure.NotNull(writer);
		writer.WriteStringValue(Format(value));
	}

	/// <inheritdoc/>
	public override SignificantNumber ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		ParseText(reader.GetString()!);

	/// <inheritdoc/>
	public override void WriteAsPropertyName(Utf8JsonWriter writer, SignificantNumber value, JsonSerializerOptions options)
	{
		Ensure.NotNull(writer);
		writer.WritePropertyName(Format(value));
	}

	private static string Format(SignificantNumber value) => value.ToString("R", CultureInfo.InvariantCulture);

	private static string GetRawText(ref Utf8JsonReader reader)
	{
		ReadOnlySpan<byte> utf8 = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
		return System.Text.Encoding.UTF8.GetString(utf8);
	}

	private static SignificantNumber ParseText(string text) =>
		SignificantNumber.TryParse(text, Styles, CultureInfo.InvariantCulture, out SignificantNumber result)
			? result
			: throw new JsonException($"\"{text}\" is not a valid {nameof(SignificantNumber)}.");
}
