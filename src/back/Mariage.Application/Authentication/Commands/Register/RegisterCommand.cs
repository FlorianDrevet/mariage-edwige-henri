using MediatR;
using ErrorOr;
using Mariage.Application.Authentication.Common;

namespace Mariage.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string Username,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;