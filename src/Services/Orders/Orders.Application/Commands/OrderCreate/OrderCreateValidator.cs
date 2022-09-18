using FluentValidation;

namespace Orders.Application.Commands.OrderCreate;

public class OrderCreateValidator : AbstractValidator<OrderCreateCommand> {
    public OrderCreateValidator() {
        RuleFor(x => x.SellerUserName)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.ProductId)
            .NotEmpty();
    }
}