using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services;

namespace WebAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Authorize]
	public class TicketController : ControllerBase
	{
		
		private readonly ITicketService _ticketService;

		public TicketController(ITicketService ticketService)
		{
			_ticketService = ticketService ?? throw new ArgumentNullException(nameof(ticketService));
			
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<TicketDto>>> GetAll([FromQuery] string sortField = "Priority", [FromQuery] string sortOrder = "asc")
		{
			try
			{
				var ticketsDto = await _ticketService.GetAll(sortField, sortOrder);
				return Ok(ticketsDto);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal Server Error: {ex.Message}");
			}
		}

		

		


		[HttpPost("createTicket")]
		[Authorize(Roles = "Admin, Manager")]
		public async Task<ActionResult<TicketDto>> CreateTicket([FromBody] TicketEkleDto ekleDto)
		{
			try
			{
				var createdTicketDto = await _ticketService.CreateTicketAsync(ekleDto);
				return CreatedAtAction(nameof(GetByID), new { id = createdTicketDto.ID }, createdTicketDto);
			}
			catch (Exception ex)
			{
				return BadRequest($"Bad Request: {ex.Message}");
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<TicketDto>> GetByID(int id)
		{
			try
			{
				var ticketDto = await _ticketService.GetByIDAsync(id);
				if (ticketDto == null)
				{
					return NotFound("Ticket not found");
				}
				return Ok(ticketDto);
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Ticket not found");
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
		}
		[HttpPut("updateTicket/{ticketId}")]
		[Authorize(Roles = "Admin, Manager")]
		public async Task<IActionResult> UpdateTicket(int ticketId, [FromBody] TicketEkleDto ekleDto)
		{
			try
			{
				await _ticketService.UpdateTicketAsync(ticketId, ekleDto);
				return NoContent();
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Ticket not found");
			}
			catch (DbUpdateConcurrencyException)
			{
				// Optionally, recheck if the ticket exists
				return StatusCode(500, "Internal Server Error");
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
		}
	}
}
