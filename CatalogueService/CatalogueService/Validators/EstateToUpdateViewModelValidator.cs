using CatalogueService.DAL.Models.Enums;
using CatalogueService.Utilities.Messages;
using CatalogueService.ViewModels;
using FluentValidation;

namespace CatalogueService.Validators
{
    public class EstateToUpdateViewModelValidator : AbstractValidator<EstateToUpdateViewModel>
    {
        public EstateToUpdateViewModelValidator()
        {
            RuleFor(e => e.Type)
                .IsInEnum().WithMessage(ExceptionMessages.FieldIsInvalidType(nameof(EstateToCreateViewModel.Type), nameof(EstateType)));

            RuleFor(e => e.Address)
                .NotEmpty().WithMessage(ExceptionMessages.FieldIsRequired(nameof(EstateToCreateViewModel.Address)))
                .MaximumLength(100).WithMessage(ExceptionMessages.FieldLimitExceeded(nameof(EstateToCreateViewModel.Address), 100));

            RuleFor(e => e.Area)
                .GreaterThan(0).WithMessage(ExceptionMessages.FieldValueTooSmall(nameof(EstateToCreateViewModel.Area), 0));

            RuleFor(e => e.RoomsCount)
                .GreaterThan((short)0).WithMessage(ExceptionMessages.FieldValueTooSmall(nameof(EstateToCreateViewModel.RoomsCount), 0));

            RuleFor(e => e.Price)
                .GreaterThan(0).WithMessage(ExceptionMessages.FieldValueTooSmall(nameof(EstateToCreateViewModel.Price), 0));
        }
    }
}
