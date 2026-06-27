using Net9.Security.Controllers.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Net9.Security.Controllers
{
    public class AccountController : Controller
    {
        #region [- Private Fields -]
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        #endregion

        #region [- Ctor() -]
        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        #endregion

        #region [- RegisterUser() -]
        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserDto registerUserDto)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = registerUserDto.UserName,
                    Email = registerUserDto.Email
                };
                var result = await _userManager.CreateAsync(user, registerUserDto.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["Message"] = "Registration successful! Welcom to your account.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var e in result.Errors)
                    {
                        ModelState.AddModelError("", e.Description);
                    }
                }
            }
            return View(registerUserDto);
        }
        #endregion

        #region [- Login() -]
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginDto.UserName, loginDto.Password, loginDto.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid username or password");
            }
            return View("Login", loginDto);
        }
        #endregion

        #region [- Logout() -]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region [- AccessDenied() -]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        } 
        #endregion
    }
}
