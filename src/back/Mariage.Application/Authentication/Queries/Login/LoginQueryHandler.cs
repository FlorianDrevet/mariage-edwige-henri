using MediatR;
using ErrorOr;
using Mariage.Application.Authentication.Common;
using Mariage.Application.Common.Interfaces.Authentication;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using Mariage.Domain.UserAggregate;

namespace Mariage.Application.Authentication.Queries.Login;

public class LoginQueryHandler(IJwtGenerator jwtGenerator, IUserRepository userRepository, IHashPassword hashPassword):
    IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        if (userRepository.GetUserByUsername(query.Username) is not User user)
        {
            return Errors.Authentication.InvalidUsername();
        }
        
        var hashedPassword = hashPassword.GetHashedPassword(query.Password, user.Salt);
        if (user.Password != hashedPassword)
        {
            return Errors.Authentication.InvalidPassword();
        }
        
        var token = jwtGenerator.GenerateToken(user);
        
        return new AuthenticationResult(user, token);
    }
}