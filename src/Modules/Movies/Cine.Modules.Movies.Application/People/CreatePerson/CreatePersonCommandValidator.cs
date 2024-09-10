using FluentValidation;

namespace Cine.Modules.Movies.Application.People.CreatePerson
{
    internal class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
    {
        public CreatePersonCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
