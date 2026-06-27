using Microsoft.AspNetCore.Authorization;
using Net9.Security.Controllers.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;


namespace Net9.Security.API.Controllers
{
    [Authorize(Roles = "Administrator")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        #region [- Private Fields -]
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        #endregion

        #region [- Ctor() -]
        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        } 
        #endregion

        #region [- EditUserRoles() -]
        [HttpGet("get-user-roles/{userId}")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found" });

            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = await _roleManager.Roles.ToListAsync();

            var editUserRolesDto = new EditUserRolesDto
            {
                UserName = user.UserName,
                UserId = user.Id,
                Roles = roles,
                UserRoles = [.. userRoles]
            };

            return Ok(editUserRolesDto);
        }


        [HttpPost("update-user-roles/{userId}")]
        public async Task<IActionResult> UpdateUserRoles(string userId, [FromBody] List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found" });

            var currentRoles = await _userManager.GetRolesAsync(user);

            foreach (var item in currentRoles)
            {
                if (!roles.Any(c => c == item))
                {
                    await _userManager.RemoveFromRoleAsync(user, item);
                }
            }
            foreach (var item in roles)
            {
                var isInRole = await _userManager.IsInRoleAsync(user, item);
                if (!isInRole)
                {
                    await _userManager.AddToRoleAsync(user, item);
                }
            }

            return Ok(new { message = "User roles updated successfully" });
        }
        #endregion

        #region [- DeleteUser() -]
        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { message = "User deleted successfully" });
            }

            return BadRequest(new { message = "Error occurred while deleting the user" });
        }
        #endregion

        #region [- GetAllUsers() -]
        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }
        #endregion
    }
}