using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Application.Interfaces.Services;

namespace WebAPI.Persistence.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;

        public LoanService(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task CreateAccountAsync(UserDto accountDto)
        {
            // Convert DTO to Domain Entity and apply business logic
            //var account = new Account { /* Set properties from DTO */ };
            //await _accountRepository.AddAsync(account);
            //await _unitOfWork.CommitAsync();
        }
    }
}
