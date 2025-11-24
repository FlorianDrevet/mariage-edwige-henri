using Mariage.Domain.UserAggregate;

namespace Mariage.Application.Authentication.Common;

public record AuthenticationResult(
    User User,
    string Token);