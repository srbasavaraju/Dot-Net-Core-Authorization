using System.Collections.Generic;
using AuthenticationWebApi.Entities;
using AuthenticationWebApi.Interfaces;
using AuthenticationWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace AuthenticationWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IDistributedCache _distributedCache;

        public UsersController(IUserService userService, IDistributedCache distributedCache)
        {
            _userService = userService;
            _distributedCache = distributedCache;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel authenticateModel)
        {
            User user = _userService.Authenticate(authenticateModel);

            if (user == null)
            {
                return BadRequest(new { message = "Username or Passowrd is incorrect" });
            }
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usersCacheKey = "UsersCache";
            var cahedUsers = await _distributedCache.GetStringAsync(usersCacheKey);
            if (!string.IsNullOrEmpty(cahedUsers))
            {
                return Ok(JsonConvert.DeserializeObject(cahedUsers) as IEnumerable<User>);
            }
            else
            {
                var users = _userService.getAll();
                await _distributedCache.SetStringAsync(usersCacheKey, JsonConvert.SerializeObject(users));
                return Ok(users);
            }
        }

        [HttpDelete]
        public IActionResult LogOut(string userName)
        {
            if (userName == null)
            {
                return BadRequest("userName is empty");
            }

            return Ok(_userService.delete(userName));
        }
    }
}
