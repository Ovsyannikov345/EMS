using CatalogueService.DAL.Models.Enums;
using CatalogueService.Utilities.Messages;
using CatalogueService.ViewModels;
using FluentValidation;

namespace CatalogueService.Validators
{
    public class EstateFilterToCreateViewModelValidator : AbstractValidator<EstateFilterToCreateViewModel>
    {
        public EstateFilterToCreateViewModelValidator()
        {
            RuleFor(e => e.EstateTypes)
                .IsInEnum().WithMessage(ExceptionMessages.FieldIsInvalidType(nameof(EstateFilterToCreateViewModel.EstateTypes), nameof(EstateType)));

            RuleFor(e => e.MinArea)
                .GreaterThan(0).WithMessage(ExceptionMessages.FieldValueTooSmall(nameof(EstateFilterToCreateViewModel.MinArea), 0));

            RuleFor(e => e.MaxArea)
                .GreaterThanOrEqualTo(e => e.MinArea).WithMessage(e => ExceptionMessages.FieldValueTooSmall(nameof(EstateFilterToCreateViewModel.MaxArea), e.MinArea));

            RuleFor(e => e.MinRoomsCount)
                .GreaterThan((short)0).WithMessage(ExceptionMessages.FieldValueTooSmall(nameof(EstateFilterToCreateViewModel.MinRoomsCount), 0));

            RuleFor(e => e.MaxRoomsCount)
                .GreaterThanOrEqualTo(e => e.MinRoomsCount).WithMessage(e => ExceptionMessages.FieldValueTooSmall(nameof(EstateFilterToCreateViewModel.MaxRoomsCount), e.MinRoomsCount));

            RuleFor(e => e.MinPrice)
                .GreaterThan(0).WithMessage(ExceptionMessages.FieldValueTooSmall(nameof(EstateFilterToCreateViewModel.MinPrice), 0));

            RuleFor(e => e.MaxPrice)
                .GreaterThanOrEqualTo(e => e.MinPrice).WithMessage(e => ExceptionMessages.FieldValueTooSmall(nameof(EstateFilterToCreateViewModel.MaxPrice), e.MinPrice));
        }
    }
}
