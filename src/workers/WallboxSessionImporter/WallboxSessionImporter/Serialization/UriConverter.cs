// <copyright file="UriConverter.cs" company="Olibos">
// Copyright (c) Olibos. All rights reserved.
// </copyright>

namespace WallboxSessionImporter.Serialization;

using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

public class UriConverter : JsonConverter<Uri?>
{
    private static readonly Uri DummyUrl = new("https://dummy/");

    public override Uri? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.String => new(DummyUrl, reader.GetString() ?? throw new InvalidConstraintException("Uri is missing.")),
            JsonTokenType.StartObject => ReadHref(ref reader) ?? throw new InvalidConstraintException("Uri is missing."),
            _ => throw new JsonException($"Unexpected token type: {reader.TokenType}"),
        };

    public override void Write(Utf8JsonWriter writer, Uri? value, JsonSerializerOptions options)
        => throw new NotSupportedException();

    private static Uri? ReadHref(ref Utf8JsonReader reader)
    {
        Uri? result = null;
        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            var propertyName = reader.GetString();
            reader.Read();

            if (propertyName == "href")
            {
                if (reader.GetString() is { } relativeUri)
                {
                    result = new(DummyUrl, relativeUri);
                }
            }
            else
            {
                reader.Skip();
            }
        }

        return result;
    }
}