using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services;

namespace WebAPI.Controllers
{

	[ApiController]
	[Route("[controller]")]
	[Authorize]
	public class TransactionController : ControllerBase
	{

		private readonly ITransactionService _transactionService;
		private readonly IAccountService _accountService;


		public TransactionController(ITransactionService transactionService, IAccountService accountService)
		{
			_transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
			_accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
			
		}


		// Para yatırma işlemleri
		[HttpPost("deposit")]
		public async Task<ActionResult<TransactionDto>> AddDeposit([FromBody] TransactionEkleDto ekleDto)
		{
			try
			{
				var transactionDto = await _transactionService.AddDeposit(ekleDto);
				
				return CreatedAtAction(nameof(GetByID), new { id = transactionDto.ID }, transactionDto);
			}
			catch (Exception ex)
			{
				return BadRequest($"Bad Request: {ex.Message}");
			}
		}


		[HttpGet("{id}")]
		public async Task<ActionResult<TransactionDto>> GetByID(int id)
		{
			try
			{
				var transactionDto = await _transactionService.GetByID(id);
				return Ok(transactionDto);
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Transaction not found");
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
		}

		// Para çekme işlemleri
		[HttpPost("withdraw")]
		public async Task<ActionResult<TransactionDto>> AddWithdraw([FromBody] TransactionEkleDto ekleDto)
		{
			try
			{
				var withdrawalTransactionDto = await _transactionService.AddWithdraw(ekleDto);
				return CreatedAtAction(nameof(GetByID), new { id = withdrawalTransactionDto.ID }, withdrawalTransactionDto);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (DbUpdateConcurrencyException)
			{
				return Conflict("Concurrency conflict occurred while updating account balance");
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
		}


		[HttpGet("{accountId}")]
		public async Task<ActionResult<AccountDto>> GetByIDAccount(int accountId)
		{
			try
			{
				var accountDto = await _accountService.GetByIDAsync(accountId);
				return Ok(accountDto);
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Account not found");
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
		}



		//public async Task<IActionResult> UpdateAccount(int accountId, AccountEkleDto updateDto)
		//{
		//	var account = await _accountRepository.GetByIDAsync(accountId);
		//	if (account == null)
		//	{
		//		return NotFound();
		//	}

		//	// Apply updates to account...
		//	account.Property = updateDto.Property; // Example property update

		//	await _accountRepository.UpdateAsync(account);

		//	return NoContent();
		//}


	}
}
