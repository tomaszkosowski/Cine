using FluentValidation;

namespace Cine.Modules.Movies.Application.Movies.CreateMovie
{
    internal class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
    {
        public CreateMovieCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters.");

            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(x => x.Duration)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Must(duration => duration.Minute > 0).WithMessage("{PropertyName} must be greater than 0 minutes.");

            RuleFor(x => x.ReleaseDate)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(x => x.Directors)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Must(people => people.All(IsValidPerson)).WithMessage("{PropertyName} is invalid.");

            RuleFor(x => x.Cast)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Must(people => people.All(IsValidPerson)).WithMessage("{PropertyName} is invalid."); ;
        }

        private bool IsValidPerson((string FirstName, string LastName) person)
        {
            return !string.IsNullOrEmpty(person.FirstName) && !string.IsNullOrEmpty(person.LastName);
        }
    }
}
