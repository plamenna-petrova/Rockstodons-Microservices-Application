using Catalog.API.Application.Features.AlbumTypes.Commands.UpdateAlbumType;
using FluentValidation;

namespace Catalog.API.Application.Features.AlbumTypes.Validators
{
    public class UpdateAlbumTypeValidator : AbstractValidator<UpdateAlbumTypeCommand>
    {
        public UpdateAlbumTypeValidator()
        {
            RuleFor(at => at.updateAlbumTypeDTO.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(20);
        }
    }
}
