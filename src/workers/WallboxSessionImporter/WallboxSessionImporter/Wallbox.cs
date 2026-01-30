// <copyright file="Wallbox.cs" company="Olibos">
// Copyright (c) Olibos. All rights reserved.
// </copyright>

namespace WallboxSessionImporter;

using System.Diagnostics.CodeAnalysis;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
public class Wallbox
{
    public required string UserName { get; set; }

    public required string Password { get; set; }

    public Uri LoginBaseUrl { get; set; } = new("https://user-api.wall-box.com");

    public Uri ApiBaseUrl { get; set; } = new("https://api.wall-box.com");
}