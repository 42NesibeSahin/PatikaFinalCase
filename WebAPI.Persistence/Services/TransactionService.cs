using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WebAPI.Application.DTOs;
using WebAPI.Application.Helpers.UserHelpers;
using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Application.Interfaces.Services;
using WebAPI.Domain.Entities;
using WebAPI.Persistence.Context;

namespace WebAPI.Persistence.Services
{
	public class TransactionService : ITransactionService
	{
		private readonly IMapper _mapper;
		private readonly ITransactionRepository _transactionRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly IUserService _userService;

		public TransactionService(IAccountRepository accountRepository, ITransactionRepository transactionRepository, IMapper mapper, IUserService userService)
		{
			_transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
			_accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_userService = userService ?? throw new ArgumentNullException(nameof(userService));
		}

		public async Task<TransactionDto> AddDeposit(TransactionEkleDto ekleDto)
		{
			TransactionDto transactionDto = new TransactionDto();
			var strategy = _accountRepository.CreateExecutionStrategy();
			await strategy.ExecuteAsync(async () =>
			{
				using (var transaction = await _accountRepository.BeginTransactionAsync())
				{
					if (!await CurrentUserAmounLimitControl(ekleDto.Amount))
					{
						throw new KeyNotFoundException("Yüksek miktarlı işlemler sadece admin tarafından onaylanarak yapılabilir");
					}

					var account = await _accountRepository.GetByIdLockedAsync(ekleDto.AccountID);
					if (account == null)
					{
						throw new KeyNotFoundException("Hesap bulunamadı");
					}

					account.Balance += ekleDto.Amount;
					await _accountRepository.UpdateAsync(account);

					var withdrawalTransaction = _mapper.Map<Transaction>(ekleDto);
					withdrawalTransaction.Type = nameof(TransactionTypes.deposit);
					withdrawalTransaction.Date = DateTime.UtcNow;
					await _transactionRepository.AddAsync(withdrawalTransaction);
					await _transactionRepository.Save();

					await _accountRepository.TransactionCommitAsync(transaction);

					transactionDto = _mapper.Map<TransactionDto>(withdrawalTransaction);
				};

			});
			return transactionDto;
		}

		public async Task<TransactionDto> AddWithdraw(TransactionEkleDto ekleDto)
		{
			TransactionDto transactionDto = new TransactionDto();
			var strategy = _accountRepository.CreateExecutionStrategy();
			await strategy.ExecuteAsync(async () =>
			{
				using (var transaction = await _accountRepository.BeginTransactionAsync())
				{

					if (!await CurrentUserAmounLimitControl(ekleDto.Amount))
					{
						throw new KeyNotFoundException("Yüksek miktarlı işlemler sadece admin tarafından onaylanarak yapılabilir");
					}

					var account = await _accountRepository.GetByIdLockedAsync(ekleDto.AccountID);
					if (account == null)
					{
						throw new KeyNotFoundException("Hesap bulunamadı");
					}

					if (ekleDto.Amount > account.Balance)
					{
						throw new InvalidOperationException("Yeterli bakiye yok");
					}

					account.Balance -= ekleDto.Amount;
					await _accountRepository.UpdateAsync(account);

					var withdrawalTransaction = _mapper.Map<Transaction>(ekleDto);
					withdrawalTransaction.Type = nameof(TransactionTypes.withdraw);
					withdrawalTransaction.Date = DateTime.UtcNow;
					await _transactionRepository.AddAsync(withdrawalTransaction);
					await _transactionRepository.Save();

					await _accountRepository.TransactionCommitAsync(transaction);

					transactionDto = _mapper.Map<TransactionDto>(withdrawalTransaction);
				};

			});

			return transactionDto;
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

		public async Task<AccountDto> GetByIDAccount(int id)
		{
			var account = await _accountRepository.GetWhereQuery(x => x.Id == id).FirstOrDefaultAsync();
			if (account == null)
			{
				throw new KeyNotFoundException("Account not found");
			}
			return _mapper.Map<AccountDto>(account);
		}


		public async Task<bool> CurrentUserAmounLimitControl(decimal amount)
		{
			var userRoles = await _userService.GetCurrentUserRolesAsync();

			if (amount > 10000 && !userRoles.Contains(nameof(UserRoleEnum.admin)))
			{
				return false;
			}
			return true;
		}


	}

	public enum TransactionTypes
	{
		[Display(Name = "deposit")]
		deposit = 1,
		[Display(Name = "withdraw")]
		withdraw = 2,
	}
}
