using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Services
{
    public interface IAccountService
    {
		Task<AccountDto> CreateAccountAsync(AccountEkleDto ekleDto);
		Task<AccountDto> GetByIDAsync(int id);
		Task<AccountDto> ViewBalance(string accountNumber);
		Task UpdateBalanceAsync(int accountId, AccountEkleDtoPut putChanges);
	}
}
