
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata;
using WebAPI.Domain.Entities;

namespace WebAPI.Validations
{
    public class AccountValidation :AbstractValidator<Account>
	{
		public AccountValidation()
		{

			RuleFor(account => account.Balance)
		   .GreaterThanOrEqualTo(0)
		   .WithMessage("The balance cannot be negative.");

			RuleFor(account => account.MinimumBalance)
				.GreaterThanOrEqualTo(100)
				.WithMessage("Minimum balance cannot be negative and is a required field.");


		}
	}
}
