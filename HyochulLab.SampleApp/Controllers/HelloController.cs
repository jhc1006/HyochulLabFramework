using Microsoft.AspNetCore.Mvc;
using HyochulLab.Core.Results;
using HyochulLab.SampleApp.Models;
using HyochulLab.Data.Interfaces;
using HyochulLab.Data.Entities;

namespace HyochulLab.SampleApp.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class HelloController : ControllerBase
{
    private readonly ILogger<HelloController> _logger;
    private readonly IUnitOfWork _uow;

    public HelloController(ILogger<HelloController> logger, IUnitOfWork uow)
    {
        _logger = logger;
        _uow = uow;
    }

    //[HttpPost]
    //public IActionResult SayHello([FromBody] HelloRequest request)
    //{
    //    var message = $"안녕하세요, {request.Name}님!";
    //    return Ok(DataResult<string>.Success(message));
    //}

    [HttpGet]
    public IActionResult Get() => Ok("✅ v1 API 호출됨");

    [HttpGet("log-test")]
    public IActionResult LogTest()
    {
        _logger.LogInformation("🔥 로깅 테스트 성공!");
        return Ok("로깅 테스트 성공!");
    }

    [HttpGet("error-test")]
    public IActionResult ErrorTest()
    {
        throw new Exception("🔥 강제 에러 발생 테스트!");
    }

}
