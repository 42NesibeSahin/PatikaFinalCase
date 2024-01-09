using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Application.Interfaces.Services;
using WebAPI.Domain.Entities;

namespace WebAPI.Persistence.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
		private readonly IMapper _mapper;

		public AccountService(IAccountRepository accountRepository ,IMapper mapper)
        {
            _accountRepository = accountRepository;
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}


		public async Task<AccountDto> CreateAccountAsync(AccountEkleDto ekleDto)
		{
			var account = _mapper.Map<Account>(ekleDto);
			account.AccountNumber = await GenerateUniqueAccountNumberAsync();

			await _accountRepository.AddAsync(account);
			await _accountRepository.Save();

			return _mapper.Map<AccountDto>(account);
		}


		public async Task<AccountDto> GetByIDAsync(int id)
		{
			var account = await _accountRepository.GetWhereQuery(x => x.Id == id).FirstOrDefaultAsync();
			if (account == null)
			{
				throw new KeyNotFoundException("Account not found");
			}

			return _mapper.Map<AccountDto>(account);
		}


		public async Task<AccountDto> ViewBalance(string accountNumber)
		{
			var account = await _accountRepository.GetWhereQuery(x => x.AccountNumber == accountNumber).FirstOrDefaultAsync();

			if (account == null)
			{
				throw new KeyNotFoundException("Account not found");
			}
			return _mapper.Map<AccountDto>(account);
		}


		public async Task UpdateBalanceAsync(int accountId, AccountEkleDtoPut putChanges)
		{
			if (accountId == 0)
			{
				throw new ArgumentException("Bad Request: Account not found");
			}

			if (putChanges.Balance < 0)
			{
				throw new InvalidOperationException("The balance cannot be negative.");
			}

			var account = await _accountRepository.GetByIDAsync(accountId);
			if (account == null)
			{
				throw new KeyNotFoundException("Account not found");
			}

			_mapper.Map(putChanges, account); // Update the account entity with the changes
			await _accountRepository.UpdateAsync(account);

			await _accountRepository.Save();
		}


		private async Task<string> GenerateUniqueAccountNumberAsync()
		{
			var random = new Random();
			string accountNumber;

			do
			{
				var accountNumberBuilder = new StringBuilder();
				for (int i = 0; i < 16; i++)
				{
					if (i > 0 && i % 4 == 0)
						accountNumberBuilder.Append(" ");  // Her 4 hane sonrası boşluk ekle

					accountNumberBuilder.Append(random.Next(0, 10)); // 0 ile 9 arasında rastgele sayılar ekler
				}

				accountNumber = accountNumberBuilder.ToString();

				// Bu hesap numarası ile eşleşen herhangi bir kayıt var mı kontrol et
			}
			while (await _accountRepository.GetWhereQuery(a => a.AccountNumber == accountNumber).AnyAsync());

			return accountNumber;
		}

	}
}

