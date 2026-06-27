using Microsoft.AspNetCore.Authorization;
using Net9.Security.Controllers.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Net9.Security.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
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

        #region [- Index() -]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        #endregion

        #region [- RegisterRole() -]
        [HttpGet]
        public IActionResult RegisterRole()
        {
            return View("RegisterRole");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterRole(RegisterRoleDto registerRoleDto)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole { Name = registerRoleDto.RoleName };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    TempData["Massage"] = "Role Created Successfully";
                    return RedirectToAction("Index", "Role");
                }
                else
                {
                    foreach(var e in result.Errors)
                    {
                        ModelState.AddModelError("", e.Description);
                    }
                }
            }
            return View("RegisterRole", registerRoleDto);
        }
        #endregion

        #region [- Delete() -]
        [HttpPost]
        public async Task<IActionResult> Delete(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role == null)
            {
                return NotFound()
;            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["Message"] = "Role is Deleted";
            }
            else
            {
                TempData["Message"] = "Fail to delete role";
            }
            return RedirectToAction("Index", "Role");
        }
        #endregion
    }
}
