using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Volo.Abp.Json.SystemTextJson.JsonConverters;

public class AbpStringToGuidConverter : JsonConverter<Guid>
{
    private JsonSerializerOptions? _writeJsonSerializerOptions;

    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var guidString = reader.GetString();
            string[] formats = { "N", "D", "B", "P", "X" };
            foreach (var format in formats)
            {
                if (Guid.TryParseExact(guidString, format, out var guid))
                {
                    return guid;
                }
            }
        }

        return reader.GetGuid();
    }

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        _writeJsonSerializerOptions ??= JsonSerializerOptionsHelper.Create(options, this);
        var entityConverter = (JsonConverter<Guid>)_writeJsonSerializerOptions.GetConverter(typeof(Guid));

        entityConverter.Write(writer, value, _writeJsonSerializerOptions);
    }
}
