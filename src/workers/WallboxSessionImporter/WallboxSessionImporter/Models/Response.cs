// <copyright file="Response.cs" company="Olibos">
// Copyright (c) Olibos. All rights reserved.
// </copyright>

#pragma warning disable SA1402

namespace WallboxSessionImporter.Models;

using System.Text.Json.Serialization;

using WallboxSessionImporter.Serialization;

public class Response<TModel>
{
    [JsonPropertyName("data")]
    public required TModel Data { get; init; }

    [JsonPropertyName("links")]
    public Links? Links { get; init; }

    [JsonPropertyName("meta")]
    public Meta? Meta { get; init; }
}

public record Meta([property:JsonPropertyName("count")] int Count);

public record Links(
    [property: JsonPropertyName("self")]
    [property: JsonConverter(typeof(UriConverter))]
    Uri Self,
    [property: JsonPropertyName("last")]
    [property: JsonConverter(typeof(UriConverter))]
    Uri? Last,
    [property: JsonPropertyName("next")]
    [property: JsonConverter(typeof(UriConverter))]
    Uri? Next);

#pragma warning restore SA1402
