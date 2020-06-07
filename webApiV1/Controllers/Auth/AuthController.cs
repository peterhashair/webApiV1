using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyAwesomeWebApi.Models.Requests;
using webApiV1.Helpers;
using webApiV1.Models.Identity;
using webApiV1.Models.Requests;
using webApiV1.Models.Responses;

namespace webApiV1.Controllers.Auth
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginEntity _user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(_user.Email, _user.Password, false, false);
                if (result.Succeeded)
                {
                    var appUser = _userManager.Users.SingleOrDefault(r => r.Email == _user.Email);

                    var roles = await _userManager.GetRolesAsync(appUser);
                    var token = AuthenticationHelper.GenerateJwtToken(_user.Email, appUser, roles, _configuration);

                    var data = new LoginResponse(token, appUser.Id.ToString(), appUser.Email);
                    return Ok(data);
                }
                return StatusCode((int)HttpStatusCode.Unauthorized, "Bad Credentials");
            }
            var errorMessage = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return BadRequest(errorMessage ?? "Bad Request");

        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterEntity _user)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { FirstName = _user.FirstName, LastName = _user.LastName, UserName = _user.Email, Email = _user.Email };

                var result = await _userManager.CreateAsync(user, _user.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return Ok(user);
                }
                return Ok(string.Join(",", result.Errors?.Select(error => error.Description)));
            }
            var errorMessage = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return BadRequest(errorMessage ?? "Bad Request");

        }

    }
}