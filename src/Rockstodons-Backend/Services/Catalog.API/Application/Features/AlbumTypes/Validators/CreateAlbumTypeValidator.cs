using Catalog.API.Application.Features.AlbumTypes.Commands.CreateAlbumType;
using FluentValidation;

namespace Catalog.API.Application.Features.AlbumTypes.Validators
{
    public class CreateAlbumTypeValidator : AbstractValidator<CreateAlbumTypeCommand>
    {
        public CreateAlbumTypeValidator()
        {
            RuleFor(at => at.createAlbumTypeDTO.Name)
                .NotNull()
                .WithMessage("The album type's name is required");

            RuleFor(at => at.createAlbumTypeDTO.Name)
                .NotEmpty()
                .WithMessage("The album type's name must not be empty");

            RuleFor(at => at.createAlbumTypeDTO.Name)
                .Length(3, 20)
                .WithMessage("The album type's name must be between 3 and 20 characters long");
        }
    }
}
