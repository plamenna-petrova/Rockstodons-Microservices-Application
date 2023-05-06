﻿using Microsoft.AspNetCore.Authentication;

namespace Identity.API.Services.Interfaces
{
    public interface ILoginService<T>
    {
        Task<bool> ValidateCredentials(T user, string password);

        Task<T> FindByUsername(string user);

        Task SignIn(T user);

        Task SignInAsync(T user, AuthenticationProperties authenticationProperties, string authenticatioMethod = null!);
    }
}
