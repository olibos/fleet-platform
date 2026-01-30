// <copyright file="WallboxJsonSerializerContext.cs" company="Olibos">
// Copyright (c) Olibos. All rights reserved.
// </copyright>

namespace WallboxSessionImporter.Serialization;

using System.Text.Json.Serialization;
using WallboxSessionImporter.Models;

[JsonSourceGenerationOptions(
    WriteIndented = false,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(Response<Object<WallboxToken>>))]
[JsonSerializable(typeof(Response<List<Object<Group>>>))]
[JsonSerializable(typeof(Response<List<Object<Session>>>))]
public partial class WallboxJsonSerializerContext : JsonSerializerContext
{
}