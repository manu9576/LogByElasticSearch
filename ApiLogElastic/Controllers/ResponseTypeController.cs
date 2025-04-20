using Microsoft.AspNetCore.Mvc;

namespace ApiLogElastic.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResponseTypeController : ControllerBase
{
	[HttpGet]
	public IActionResult GetResponseType(ResponseType responseType)
	{
		switch (responseType)
		{
			case ResponseType.Success:
				return Ok(new { Message = "Success" });
			case ResponseType.Failure:
				return BadRequest(new { Message = "Failure" });
			case ResponseType.Warning:
				return StatusCode(299, new { Message = "Warning" });
			case ResponseType.Error:
				return StatusCode(500, new { Message = "Error" });
			case ResponseType.NotFound:
				return NotFound(new { Message = "Not Found" });
			case ResponseType.Exception:
				throw new Exception("Exception occurred");
			default:
				return NotFound(new { Message = "Unknown response type" });
		}
	}
}
