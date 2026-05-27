using ErrorOr;
using Mariage.Application.Authentication.Common;
using Mariage.Application.Common.Interfaces.Authentication;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IJwtGenerator jwtGenerator,
    IUserRepository userRepository) : IRequestHandler<RefreshTokenCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var principal = jwtGenerator.GetPrincipalFromExpiredToken(command.Token);
        if (principal is null)
        {
            return Error.Unauthorized(code: "Auth.InvalidToken", description: "Invalid access token.");
        }

        var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                          ?? principal.FindFirst("sub");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userIdGuid))
        {
            return Error.Unauthorized(code: "Auth.InvalidToken", description: "Invalid access token.");
        }

        var user = userRepository.GetUserById(UserId.Create(userIdGuid));
        if (user is null)
        {
            return Error.Unauthorized(code: "Auth.InvalidToken", description: "User not found.");
        }

        if (user.RefreshToken != command.RefreshToken ||
            user.RefreshTokenExpiryTime is null ||
            user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Error.Unauthorized(code: "Auth.InvalidRefreshToken", description: "Invalid or expired refresh token.");
        }

        var newAccessToken = jwtGenerator.GenerateToken(user);
        var newRefreshToken = jwtGenerator.GenerateRefreshToken();

        user.SetRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(30));
        userRepository.UpdateUser(user);

        return new AuthenticationResult(user, newAccessToken, newRefreshToken);
    }
}
