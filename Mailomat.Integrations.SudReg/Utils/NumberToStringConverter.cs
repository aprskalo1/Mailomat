using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mailomat.Integrations.SudReg.Utils;

public class NumberToStringConverter : JsonConverter<string>
{
    public override string Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number => reader.GetInt64().ToString(),
            JsonTokenType.String => reader.GetString()!,
            _ => throw new JsonException($"Cannot convert token of type {reader.TokenType} to string.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        string value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStringValue(value);
    }
}