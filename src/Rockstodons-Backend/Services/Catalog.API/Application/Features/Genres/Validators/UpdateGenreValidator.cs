using Catalog.API.Application.Features.Genres.Commands.UpdateGenre;
using FluentValidation;

namespace Catalog.API.Application.Features.Genres.Validators
{
    public class UpdateGenreValidator : AbstractValidator<UpdateGenreCommand>
    {
        public UpdateGenreValidator()
        {
            RuleFor(g => g.updateGenreDTO.Name)
                .NotNull()
                .WithMessage("The genres's name is required");

            RuleFor(g => g.updateGenreDTO.Name)
                .NotEmpty()
                .WithMessage("The genres's name must not be empty");

            RuleFor(g => g.updateGenreDTO.Name)
                .Length(3, 20)
                .WithMessage("The genres's name must be between 3 and 20 symbols long");
        }
    }
}
