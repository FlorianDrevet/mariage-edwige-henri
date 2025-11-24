using FluentValidation;

namespace Mariage.Application.Authentication.Queries.Login.Validators;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
   public LoginQueryValidator()
   {
      RuleFor(x => x.Username).NotEmpty();
      RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
   } 
}