
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Domain.Entities;

namespace WebAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Authorize]
	public class LoanController : ControllerBase
	{
		[HttpPost("applyForLoan")]
		public IActionResult ApplyForLoan([FromBody] Loan loan)
		{
			return Ok();
		}

		[HttpGet("loanStatus/{loanId}")]
	
		public IActionResult LoanStatus(int loanId)
		{
			return Ok();
		}
	}
}
