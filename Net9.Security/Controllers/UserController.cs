using Microsoft.AspNetCore.Authorization;
using Net9.Security.Controllers.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Net9.Security.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
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

        #region [- Index() -]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
        #endregion

        #region [- EditUserRoles() -]
        [HttpGet]
        public async Task<IActionResult> EditUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.ToList();
            var editUserRolesDto = new EditUserRolesDto
            {
                UserName = user.UserName,
                UserId = user.Id,
                Roles = roles,
                UserRoles = [.. userRoles]
            };

            return View(editUserRolesDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserRoles(string userId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var currentRoles = await _userManager.GetRolesAsync(user);


            foreach(var item in currentRoles)
            {
                if(!roles.Any(c => c == item))
                {
                    var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, item);
                }
            }
            foreach(var item in roles)
            {
                var isInRole = await _userManager.IsInRoleAsync(user, item);
                if (!isInRole)
                {
                    var addToRoleResult = await _userManager.AddToRoleAsync(user, item);
                }
            }
            return RedirectToAction("Index", "User");
        }
        #endregion

        #region [- Delete() -]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
               return NotFound();
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Message"] = "User deleted successfully";
            }
            else
            {
                TempData["Message"] = "Error occurred while deleting the user";
            }
                return RedirectToAction("Index", "User");
        }
        #endregion
    }
}
