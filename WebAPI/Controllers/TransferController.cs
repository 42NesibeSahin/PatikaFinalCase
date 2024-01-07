using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services;

namespace WebAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Authorize]
	public class TransferController : ControllerBase
	{

		
		private readonly ITransferService _transferService;
		private readonly IAccountService _accountService;
		public TransferController(ITransferService transferService, IAccountService accountService)
		{
			_transferService = transferService ?? throw new ArgumentNullException(nameof(transferService));
			_accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
			
		}



		[HttpPost("transfer")]
		public async Task<ActionResult<TransferDto>> Add([FromBody] TransferEkleDto ekleDto)
		{
			try
			{
				var transferDto = await _transferService.AddTransferAsync(ekleDto);
				return CreatedAtAction(nameof(GetByID), new { id = transferDto.ID }, transferDto);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (DbUpdateConcurrencyException)
			{
				return Conflict("Concurrency conflict occurred while updating accounts");
			}
			catch (Exception ex)
			{
				return BadRequest($"Bad Request: {ex.Message}");
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<TransferDto>> GetByID(int id)
		{
			try
			{
				var transferDto = await _transferService.GetByIDAsync(id);
				return Ok(transferDto);
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Transfer not found");
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
		}
	}
}
