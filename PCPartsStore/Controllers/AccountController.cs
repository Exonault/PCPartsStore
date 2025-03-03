using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PCPartsStore.Models.Account;

namespace PCPartsStore.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(ILogger<AccountController> logger, UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            if (!_userManager.Users.Any())
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, ["Admin", "User"]);
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            else
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var item in result.Errors)
                    ModelState.AddModelError("", item.Description);
            }
        }

        return View(model);
    }

    public IActionResult Login()
    {
        if (_userManager.Users.Any() == false) return RedirectToAction("Register");

        if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            var userFound = await _userManager.FindByEmailAsync(model.Email);
            if (userFound == null)
                ModelState.AddModelError("", "User not found!");
            else
                ModelState.AddModelError("", "Invalid Login attempt");
        }

        return View();
    }

    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("SessionData");
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}