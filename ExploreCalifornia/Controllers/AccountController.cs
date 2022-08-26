using ExploreCalifornia.DAL;
using ExploreCalifornia.Models;
using ExploreCalifornia.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ExploreCalifornia.Controllers
{
    [Route("Account")]
    [RequireHttps]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IdentityDbContext<User> _identityDbContext;

        public AccountController(SignInManager<User> signInManager
            , UserManager<User> userManager, BlogIdentityDbContext identityDbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _identityDbContext = identityDbContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] UserLoginViewModel userLoginViewModel, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(userLoginViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(userLoginViewModel.UserName, userLoginViewModel.Password, true, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Login Error!");
                return View(userLoginViewModel);
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {

            }

            return RedirectToAction("Index", "Blog");
        }

        [HttpGet]
        [Route("RegisterUser")]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [Route("LogoutUser")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel userViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }

            var userEntity = userViewModel.ToUserEntity();

            var result = await _userManager.CreateAsync(userEntity, userEntity.Password);

            if (!result.Succeeded)
            {
                CollectAllErrorMessages(ModelState, result);
            }

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        private void CollectAllErrorMessages(ModelStateDictionary modelState, IdentityResult result)
        {
            foreach (var error in result.Errors.Select(e => e.Description))
            {
                modelState.AddModelError("", error);
            }
        }
    }

    public static class UserExtensions
    {


        public static User ToUserEntity(this RegisterViewModel registerViewModel)
        {
            return new User
            {
                Email = registerViewModel.UserName,
                UserName = registerViewModel.UserName,
                Password = registerViewModel.Password,
                PasswordConfirm = registerViewModel.PasswordConfirm,
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName
            };
        }
    }
}
