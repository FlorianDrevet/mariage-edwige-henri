using MediatR;
using ErrorOr;
using Mariage.Application.Authentication.Common;
using Mariage.Application.Common.Interfaces.Authentication;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using Mariage.Domain.UserAggregate;

namespace Mariage.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(
    IJwtGenerator jwtGenerator,
    IHashPassword hashPassword,
    IUserRepository userRepository) : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (userRepository.GetUserByUsername(command.Username) is not null)
        {
            return Errors.User.DuplicateEmailError();
        }
        
        var hashedPassword = hashPassword.GetHashedPassword(command.Password);
        var user = User.Create(command.Username, hashedPassword.Item2, hashedPassword.Item1);
        userRepository.AddUser(user);
        
        var token = jwtGenerator.GenerateToken(user);
        return new AuthenticationResult(user, token);
    }
}