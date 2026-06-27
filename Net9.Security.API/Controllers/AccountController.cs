using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Net9.Security.Controllers.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace Net9.Security.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        #region [- Private Fields -]
        private readonly UserManager<IdentityUser> _userManager;
        #endregion

        #region [- Ctor() -]
        public AccountController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        #endregion

        #region [- RegisterUser() -]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUserDto.UserName,
                Email = registerUserDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            if (result.Succeeded)
            {
                return Ok(new { message = "Registration successful!" });
            }


            foreach (var e in result.Errors)
            {
                ModelState.AddModelError("", e.Description);
            }
            return BadRequest(ModelState);
        }
        #endregion

        #region [- Login() -]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecretKeyThatIsAtLeast32CharactersLong!"));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = "MySecureBackend",
                    Audience = "MySecureClients",
                    Expires = DateTime.Now.AddHours(3),
                    SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                    Subject = new ClaimsIdentity(authClaims)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token),
                    expiration = tokenDescriptor.Expires
                });
            }

            return Unauthorized(new { message = "Invalid username or password" });
        }
        #endregion
    }
}