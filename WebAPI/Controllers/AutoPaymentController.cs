
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
		[HttpPost("setupAutoPayment")]
		public IActionResult SetupAutoPayment([FromBody] AutoPayment autoPayment)
		{
			return Ok();
		}

		[HttpDelete("cancelAutoPayment/{autoPaymentId}")]
		public IActionResult CancelAutoPayment(int autoPaymentId)
		{
			return Ok();
		}
	}
}
