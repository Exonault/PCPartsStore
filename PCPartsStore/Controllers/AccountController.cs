using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PCPartsStore.Entities;
using PCPartsStore.Models.Account;
using PCPartsStore.Repository.Interfaces;

namespace PCPartsStore.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IAddressRepository _addressRepository;

    public AccountController(ILogger<AccountController> logger, UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager, IAddressRepository addressRepository)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _addressRepository = addressRepository;
    }

    [AllowAnonymous]
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

    [AllowAnonymous]
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

    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("SessionData");
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [Authorize(Roles = "Admin")]
    [Route("Account/Admin")]
    public IActionResult AdminPage()
    {
        return View();
    }

    [Authorize(Roles = "User")]
    [Route("Account/User")]
    public IActionResult UserPage()
    {
        return View();
    }

    [Authorize(Roles = "User")]
    [Route("Account/User/Addresses")]
    public async Task<IActionResult> Addresses()
    {
        var userId = _userManager.GetUserId(User);
        var addresses = await _addressRepository.GetAddressesByUserId(userId);
        if (addresses.Any())
            return RedirectToAction("AddAddress");

        return View(addresses);
    }

    [Authorize(Roles = "User")]
    [Route("Account/User/Address/New")]
    public IActionResult AddAddress()
    {
        return View();
    }
}