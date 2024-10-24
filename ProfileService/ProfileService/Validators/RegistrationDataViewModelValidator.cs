using FluentValidation;
using ProfileService.Utilities.Messages;
using ProfileService.ViewModels;

namespace ProfileService.Validators
{
    public class RegistrationDataViewModelValidator : AbstractValidator<RegistrationDataViewModel>
    {
        public RegistrationDataViewModelValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(ExceptionMessages.FieldIsRequired(nameof(RegistrationDataViewModel.Id)))
                .Matches(@"^auth0[|]{1}.+$").WithMessage(ExceptionMessages.InvalidAuth0Id(nameof(RegistrationDataViewModel.Id)));

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(ExceptionMessages.FieldIsRequired(nameof(RegistrationDataViewModel.FirstName)))
                .MaximumLength(50).WithMessage(ExceptionMessages.FieldLimitExceeded(nameof(RegistrationDataViewModel.FirstName), 50));

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(ExceptionMessages.FieldIsRequired(nameof(RegistrationDataViewModel.LastName)))
                .MaximumLength(50).WithMessage(ExceptionMessages.FieldLimitExceeded(nameof(RegistrationDataViewModel.LastName), 50));

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(ExceptionMessages.FieldIsRequired(nameof(RegistrationDataViewModel.PhoneNumber)))
                .MaximumLength(30).WithMessage(ExceptionMessages.FieldLimitExceeded(nameof(RegistrationDataViewModel.PhoneNumber), 30));

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage(ExceptionMessages.FieldIsRequired(nameof(RegistrationDataViewModel.BirthDate)))
                .LessThanOrEqualTo(DateTime.Today).WithMessage(ExceptionMessages.DateFieldIsFuture(nameof(RegistrationDataViewModel.BirthDate)));
        }
    }
}
