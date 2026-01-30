// <copyright file="Response.cs" company="Olibos">
// Copyright (c) Olibos. All rights reserved.
// </copyright>
#pragma warning disable SA1402

namespace WallboxSessionImporter.Models;

using System.Text.Json.Serialization;

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
    string Self,
    [property: JsonPropertyName("last")]
    string? Last,
    [property: JsonPropertyName("next")]
    string? Next);

#pragma warning restore SA1402
