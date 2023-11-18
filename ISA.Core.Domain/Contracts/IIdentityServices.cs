﻿using ISA.Core.Domain.Entities.Token;

namespace ISA.Core.Domain.Contracts;

public interface IIdentityServices
{
    public Task RegisterUserAsync(Guid id, string email, string password, string userRole);

    public Task<AuthenticationTokens> LoginAsync(string email, string password, string role);

}
