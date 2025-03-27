using HyochulLab.Caching.Interfaces;
using HyochulLab.Core.Results;
using HyochulLab.Data.Entities;
using HyochulLab.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HyochulLab.SampleApp.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<HelloController> _logger;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;

        public UserController(ILogger<HelloController> logger, IUnitOfWork uow, ICacheService cacheService) 
        {
            _logger = logger;
            _uow = uow;
            _cacheService = cacheService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            await _uow.Users.AddAsync(user);
            await _uow.SaveChangesAsync();
            return Ok(Result.Success("유저 생성 완료"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            string cacheKey = $"user-{id}";

            // 캐시에서 조회
            var cachedUser = await _cacheService.GetAsync<User>(cacheKey);
            if (cachedUser != null)
                return Ok(DataResult<User>.Success(cachedUser));

            // DB에서 조회 후 캐시에 저장
            var user = await _uow.Users.GetAsync(u => u.Id == id);
            if (user == null)
                return NotFound(Result.Fail("유저 없음"));

            await _cacheService.SetAsync(cacheKey, user, TimeSpan.FromMinutes(5));
            return Ok(DataResult<User>.Success(user));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updateUser)
        {
            var user = await _uow.Users.GetAsync(u => u.Id == id);
            if (user == null) return NotFound();

            user.Name = updateUser.Name;
            await _uow.SaveChangesAsync();

            // 👇 캐시 데이터 삭제 (갱신 효과)
            string cacheKey = $"user-{id}";
            await _cacheService.RemoveAsync(cacheKey);

            return Ok(Result.Success("유저 정보 갱신 완료"));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("secure-data")]
        public IActionResult GetSecureData()
        {
            return Ok("이건 Admin만 볼 수 있는 데이터입니다!");
        }
    }
}
