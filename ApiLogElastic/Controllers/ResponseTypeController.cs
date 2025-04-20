using Microsoft.AspNetCore.Mvc;

namespace ApiLogElastic.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResponseTypeController : ControllerBase
{
	private readonly ILogger<ResponseTypeController> _logger;

	public ResponseTypeController(ILogger<ResponseTypeController> logger)
	{
		_logger = logger;
	}

	[HttpGet]
	public IActionResult GetResponseType(ResponseType responseType)
	{
		switch (responseType)
		{
			case ResponseType.Success:
				_logger.LogInformation("Success response type selected.");
				return Ok(new { Message = "Success" });
			case ResponseType.Failure:
				_logger.LogWarning("Failure response type selected.");
				return BadRequest(new { Message = "Failure" });
			case ResponseType.Warning:
				_logger.LogWarning("Warning response type selected.");
				return StatusCode(299, new { Message = "Warning" });
			case ResponseType.Error:
				_logger.LogError("Error response type selected.");
				return StatusCode(500, new { Message = "Error" });
			case ResponseType.NotFound:
				_logger.LogWarning("NotFound response type selected.");
				return NotFound(new { Message = "Not Found" });
			case ResponseType.Exception:
				_logger.LogError("Exception response type selected.");
				throw new Exception("Exception occurred");
			default:
				_logger.LogWarning("Unknown response type selected.");
				return NotFound(new { Message = "Unknown response type" });
		}
	}
}
