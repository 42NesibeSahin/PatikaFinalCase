
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
		//[HttpPost("applyForLoan")]
		[HttpPost]
		public IActionResult ApplyForLoan([FromBody] Loan loan)
		{
			// Kredi başvurusu işlemleri
			// Örneğin: Kullanıcı bilgileri ve mali durum değerlendirmesi, vb.
			return Ok();
		}

		//[HttpGet("loanStatus/{loanId}")]
		[HttpGet]
		public IActionResult LoanStatus(int loanId)
		{
			// Kredi durumunu sorgulama işlemleri
			// Örneğin: Veritabanından ilgili kredi bilgisini sorgulama, vb.
			return Ok();
		}
	}
}
