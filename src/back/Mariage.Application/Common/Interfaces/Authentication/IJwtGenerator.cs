using Mariage.Domain.UserAggregate;

namespace Mariage.Application.Common.Interfaces.Authentication;

public interface IJwtGenerator
{
    string GenerateToken(User user);
}