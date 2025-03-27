using Microsoft.AspNetCore.Mvc;
using HyochulLab.Core.Results;
using HyochulLab.SampleApp.Models;

namespace HyochulLab.SampleApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelloController : ControllerBase
{
    [HttpPost]
    public IActionResult SayHello([FromBody] HelloRequest request)
    {
        var message = $"안녕하세요, {request.Name}님!";
        return Ok(DataResult<string>.Success(message));
    }
}
