using FluentValidation;

namespace Discount.Features.Discount.DTOs
{
    public class UpdateDiscountRequest
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
    }

    public class UpdateDiscountRequestValidator : AbstractValidator<UpdateDiscountRequest>
    {
        public UpdateDiscountRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.ProductName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
        }
    }
}
