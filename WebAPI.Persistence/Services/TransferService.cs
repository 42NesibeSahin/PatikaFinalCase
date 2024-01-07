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
    public class TransferService:ITransferService
    {
		private readonly ITransferRepository _transferRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly IMapper _mapper;
		private readonly DbContext _context;

		public TransferService(ITransferRepository transferRepository,IAccountRepository accountRepository, IMapper mapper,DbContext context)
		{
			_transferRepository = transferRepository;
			_accountRepository = accountRepository;
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_context = context;
		}

		public async Task<TransferDto> GetByIDAsync(int id)
		{
			var transfer = await _transferRepository.GetWhereQuery(x => x.Id == id).FirstOrDefaultAsync();
			if (transfer == null)
			{
				throw new KeyNotFoundException("Transfer not found");
			}

			return _mapper.Map<TransferDto>(transfer);
		}

		public async Task<TransferDto> AddTransferAsync(TransferEkleDto ekleDto)
		{
			using (var transaction = await _context.Database.BeginTransactionAsync())
			{
				var senderAccount = await _accountRepository.GetByIDAsync(ekleDto.AccountID);
				var recipientAccount = await _accountRepository.GetWhereQuery(x => x.AccountNumber == ekleDto.ToAccount).FirstOrDefaultAsync();

				if (senderAccount == null || recipientAccount == null)
				{
					throw new KeyNotFoundException("One or both accounts not found");
				}

				if (ekleDto.Amount > senderAccount.Balance)
				{
					throw new InvalidOperationException("Insufficient funds for the transfer");
				}

				senderAccount.Balance -= ekleDto.Amount;
				recipientAccount.Balance += ekleDto.Amount;

				await _accountRepository.UpdateAsync(senderAccount);
				await _accountRepository.UpdateAsync(recipientAccount);

				Transfer transfer = _mapper.Map<Transfer>(ekleDto);
				transfer.TransferDate = DateTime.UtcNow;
				await _transferRepository.AddAsync(transfer);

				await transaction.CommitAsync();

				return _mapper.Map<TransferDto>(transfer);
			}
		}
	}
}
