
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Domain.Entities;

namespace WebAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Authorize]
	public class AutoPaymentController : ControllerBase
	{
		//[HttpPost("setupAutoPayment")]
		[HttpPost]
		public IActionResult SetupAutoPayment([FromBody] AutoPayment autoPayment)
		{
			// Otomatik ödeme ayarlama işlemleri
			// Örneğin: Ödeme planı oluşturma, ödeme durumunu "Active" olarak ayarlama, vb.
			return Ok();
		}

		//[HttpDelete("cancelAutoPayment/{autoPaymentId}")]
		[HttpDelete]
		public IActionResult CancelAutoPayment(int autoPaymentId)
		{
			// Otomatik ödeme iptali işlemleri
			// Örneğin: Veritabanındaki ilgili ödeme planını bulma ve durumunu "Cancelled" olarak güncelleme, vb.
			return Ok();
		}
	}
}
