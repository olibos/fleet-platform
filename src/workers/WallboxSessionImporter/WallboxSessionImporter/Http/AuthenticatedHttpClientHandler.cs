// <copyright file="AuthenticatedHttpClientHandler.cs" company="Olibos">
// Copyright (c) Olibos. All rights reserved.
// </copyright>

namespace WallboxSessionImporter.Http;

using System.Net.Http.Headers;
using WallboxSessionImporter.Services;

public class AuthenticatedHttpClientHandler(WallboxAuthentication authentication) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await authentication.GetToken(cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        return await base.SendAsync(request, cancellationToken);
    }
}