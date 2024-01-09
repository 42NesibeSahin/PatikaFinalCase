
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata;
using WebAPI.Application.DTOs;
using WebAPI.Domain.Entities;

namespace WebAPI.Validations
{
    public class AccountValidation :AbstractValidator<AccountEkleDto>
	{
		public AccountValidation()
		{

			RuleFor(account => account.Balance)
		   .GreaterThanOrEqualTo(0)
		   .WithMessage("Hesap bakiyesi negatif olamaz");



			RuleFor(account => account.MinimumBalance)
				.GreaterThanOrEqualTo(0).WithMessage("Minimum bakiye negatif olamaz")
				.NotNull().WithMessage("Doldurulması zorunlu bir alandır");

		}
	}
}
