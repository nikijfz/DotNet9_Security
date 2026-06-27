using Microsoft.AspNetCore.Authorization;
using Net9.Security.Controllers.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Net9.Security.API.Controllers
{
<<<<<<< HEAD
    [Authorize(Policy = "AdminOnly")]
=======
    [Authorize(Roles = "Administrator")]
>>>>>>> 3c5ed30d555e0e9e5f1bedd4b957af89a62eb16a
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        #region [- Private Fields -]
        private readonly RoleManager<IdentityRole> _roleManager;
        #endregion

        #region [- Ctor() -]
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        } 
        #endregion

        #region [- RegisterRole() -]
        [HttpPost("register-role")]
        public async Task<IActionResult> RegisterRole([FromBody] RegisterRoleDto registerRoleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var role = new IdentityRole { Name = registerRoleDto.RoleName };
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return Ok(new { message = "Role Created Successfully" });
            }

            foreach (var e in result.Errors)
            {
                ModelState.AddModelError("", e.Description);
            }

            return BadRequest(ModelState);
        }
        #endregion

        #region [- Delete() -]
        [HttpDelete("delete-role/{roleId}")]
        public async Task<IActionResult> Delete(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound(new { message = "Role not found" });
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Ok(new { message = "Role is Deleted" });
            }

            return BadRequest(new { message = "Fail to delete role" });
        }
        #endregion

        #region [- GetAllRoles() -]
        [HttpGet("all-roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }
        #endregion
    }
}