using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Services
{
    public interface ITransactionService
    {
		Task<TransactionDto> AddDeposit(TransactionEkleDto ekleDto);

		Task<TransactionDto> GetByID(int id);

		Task<TransactionDto> AddWithdraw(TransactionEkleDto ekleDto);

		Task<AccountDto> GetByIDAccount(int id);

	}
}
