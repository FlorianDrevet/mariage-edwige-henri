using MediatR;
using ErrorOr;
using Mariage.Application.Authentication.Common;

namespace Mariage.Application.Authentication.Queries.Login;

public record LoginQuery(string Username, string Password) : IRequest<ErrorOr<AuthenticationResult>>;