using Catalog.API.Application.Features.AlbumTypes.Commands.UpdateAlbumType;
using FluentValidation;

namespace Catalog.API.Application.Features.AlbumTypes.Validators
{
    public class UpdateAlbumTypeValidator : AbstractValidator<UpdateAlbumTypeCommand>
    {
        public UpdateAlbumTypeValidator()
        {
            RuleFor(at => at.updateAlbumTypeDTO.Name)
                .NotNull()
                .WithMessage("The album type's name is required");

            RuleFor(at => at.updateAlbumTypeDTO.Name)
                .NotEmpty()
                .WithMessage("The album type's name must not be empty");

            RuleFor(at => at.updateAlbumTypeDTO.Name)
                .Length(2, 20)
                .WithMessage("The album type's name must be between 2 and 20 symbols long");
        }
    }
}
