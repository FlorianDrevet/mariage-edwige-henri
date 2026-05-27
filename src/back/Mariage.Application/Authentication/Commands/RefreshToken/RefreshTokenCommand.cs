using ErrorOr;
using Mariage.Application.Authentication.Common;
using MediatR;

namespace Mariage.Application.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(
    string Token,
    string RefreshToken) : IRequest<ErrorOr<AuthenticationResult>>;
