using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Application.Interfaces.Services;

namespace WebAPI.Persistence.Services
{
    public class TransactionService : ITransactionService
    {
		private readonly IMapper _mapper;
		private readonly ITransactionRepository _transactionRepository;
		private readonly IAccountRepository _accountRepository;

		public TransactionService(IAccountRepository accountRepository, ITransactionRepository transactionRepository, IMapper mapper)
		{
			_transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
			_accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<TransactionDto> AddDeposit(TransactionEkleDto ekleDto)
		{
			
			var transaction = _mapper.Map<WebAPI.Domain.Entities.Transaction>(ekleDto);
			transaction.Date = DateTime.Now;
			transaction.Type = "deposit";
			//var transaction = _mapper.Map<Transaction>(ekleDto);
			await _transactionRepository.AddAsync(transaction);
			return _mapper.Map<TransactionDto>(transaction);
			
		}

		public async Task<TransactionDto> GetByID(int id)
		{
			var transaction = await _transactionRepository.GetWhereQuery(x => x.Id == id).FirstOrDefaultAsync();
			if (transaction == null)
			{
				throw new KeyNotFoundException("Transaction not found");
			}
			return _mapper.Map<TransactionDto>(transaction);
		}

		public async Task<TransactionDto> AddWithdraw(TransactionEkleDto ekleDto)
		{
			var account = await _accountRepository.GetByIDAsync(ekleDto.AccountID);
			if (account == null)
			{
				throw new KeyNotFoundException("Account not found");
			}

			if (ekleDto.Amount > account.Balance)
			{
				throw new InvalidOperationException("Insufficient funds for the withdrawal");
			}

			account.Balance -= ekleDto.Amount;
			await _accountRepository.UpdateAsync(account);
			
			//var withdrawalTransaction = _mapper.Map<Transaction>(ekleDto);
			var withdrawalTransaction = _mapper.Map<WebAPI.Domain.Entities.Transaction>(ekleDto);
			withdrawalTransaction.Type = "withdraw";
			withdrawalTransaction.Date = DateTime.UtcNow;
			await _transactionRepository.AddAsync(withdrawalTransaction);

			return _mapper.Map<TransactionDto>(withdrawalTransaction);
		}

		public async Task<AccountDto> GetByIDAccount(int id)
		{
			var account = await _accountRepository.GetWhereQuery(x => x.Id == id).FirstOrDefaultAsync();
			if (account == null)
			{
				throw new KeyNotFoundException("Account not found");
			}
			return _mapper.Map<AccountDto>(account);
		}

	}
}
