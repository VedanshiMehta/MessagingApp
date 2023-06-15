using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MessagingApp
{
    public class WelcomeValidator : AbstractValidator<WelcomeDataModel>
    {
        private List<ValidationFailure> _errors;
        public WelcomeValidator()
        {
            RuleFor(x => x.MobileNumber).NotEmpty().WithMessage("Enter mobile number.").
                MinimumLength(13).WithMessage("Enter valid mobile number.");
        }
        public override ValidationResult Validate(ValidationContext<WelcomeDataModel> context)
        {
            var validationResult = base.Validate(context);
            _errors = validationResult.Errors;
            return validationResult;
        }
        public string GetErrorMessage([CallerMemberName]string name="")
        {
            if (_errors.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                return _errors?.FirstOrDefault(x => x.PropertyName == name)?.ErrorMessage;
            }
        }
        public string GetAllErrorMessage()
        {
            if (_errors.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                return _errors?[0].ErrorMessage ?? string.Empty;
            }
        }


    }
}
