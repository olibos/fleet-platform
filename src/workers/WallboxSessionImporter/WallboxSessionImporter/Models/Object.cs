// <copyright file="Object.cs" company="Olibos">
// Copyright (c) Olibos. All rights reserved.
// </copyright>

namespace WallboxSessionImporter.Models;

using System.Text.Json.Serialization;

public class Object<TAttribute>
{
    [JsonPropertyName("attributes")]
    public required TAttribute Attributes { get; set; }
}