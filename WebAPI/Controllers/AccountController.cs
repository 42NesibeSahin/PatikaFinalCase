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
	[Route("api/account")]
	//[Authorize]
	public class AccountController : ControllerBase
	{

		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
		}


		[AllowAnonymous]                           // yalnız bu istek için yetki kullanılmamması istendiğinde 
		[Authorize(Roles ="")]                     // yalnız bu istek için seçili yetkili kullanılmamması istendiğinde 
		[HttpPost("createAccount")]
		public async Task<ActionResult<AccountDto>> Add([FromBody] AccountEkleDto ekleDto)
		{
			try
			{
				return await _accountService.CreateAccountAsync(ekleDto);
				
			}
			catch (Exception ex)
			{
				return BadRequest($"Bad Request: {ex.Message}");
			}
		}


		[HttpGet("{id}")]
		public async Task<ActionResult<AccountDto>> GetByID(int id)
		{
			try
			{
				var accountDto = await _accountService.GetByIDAsync(id);
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

		[HttpGet("viewBalance/{accountNumber}")]
		public async Task<ActionResult<AccountDto>> ViewBalance(string accountNumber)
		{
			try
			{
				var accountDto = await _accountService.ViewBalance(accountNumber);
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

		[HttpPut("updateBalance")]
		[Authorize(Roles = "Admin, Manager")]
		public async Task<IActionResult> UpdateBalance(int accountId, [FromBody] AccountEkleDtoPut putChanges)
		{
			try
			{
				await _accountService.UpdateBalanceAsync(accountId, putChanges);
				return NoContent();
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
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
				return StatusCode(500, "Internal Server Error");
			}
		}

	}
}
