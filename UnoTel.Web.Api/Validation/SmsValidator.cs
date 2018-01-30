using FluentValidation;
using UnoTel.Web.Api.DTO.V1;

namespace UnoTel.Web.Api.Validation
{
    public class SmsValidator : AbstractValidator<Sms>
    {
        public SmsValidator()
        {
            RuleFor(x => x.RecipientNumber)
                .InclusiveBetween(10000000, 99999999);
            RuleFor(x => x.Text)
                .Length(1, 960);
        }
    }
}
