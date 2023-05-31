using Catalog.API.Application.Features.AlbumTypes.Commands.CreateAlbumType;
using Catalog.API.Application.Features.Genres.Commands.CreateGenre;
using FluentValidation;

namespace Catalog.API.Application.Features.Genres.Validators
{
    public class CreateGenreValidator : AbstractValidator<CreateGenreCommand>
    {
        public CreateGenreValidator()
        {
            RuleFor(at => at.createGenreDTO.Name)
                .NotNull()
                .WithMessage("The genres's name is required");

            RuleFor(at => at.createGenreDTO.Name)
                .NotEmpty()
                .WithMessage("The genres's name must not be empty");

            RuleFor(at => at.createGenreDTO.Name)
                .Length(3, 20)
                .WithMessage("The genres's name must be between 3 and 20 characters long");
        }
    }
}
