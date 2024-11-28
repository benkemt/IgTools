﻿using Ig.Api.Client.Model;

namespace Ig.Api.Client;

public interface IAuthService
{
    Task<Result> LoginAsync(string username, string password);

    OAuthToken? GetToken();
    //Task<Result<OAuthToken>> GetTokenAsync(AuthenticationRequest request);
}