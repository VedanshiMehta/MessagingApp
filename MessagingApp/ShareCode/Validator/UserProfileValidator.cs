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
    class UserProfileValidator : AbstractValidator<UserProfileDataModel>
    {
        private List<ValidationFailure> _errors;
        public UserProfileValidator() 
        { 
           RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Enter Username.")
                .Matches(@"^(?!.*\S[MG]O\b)[a-zA-Z]+(?: [a-zA-Z]+)*$").WithMessage("Enter valid Username.");

        }
        public override ValidationResult Validate(ValidationContext<UserProfileDataModel> context)
        {
            var validate =  base.Validate(context);
            _errors = validate.Errors;
            return validate;
        }
        public string GetErrorMessage([CallerMemberName] string name = "")
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
