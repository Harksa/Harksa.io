using System.Threading.Tasks;
using Harksa.io.Models;
using Repository.Services;
using Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Harksa.io.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IDatabaseService _databaseService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IDatabaseService service) {
            _userManager = userManager;
            _signInManager = signInManager;
            _databaseService = service;
        }

        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterModel model) {
            if (!ModelState.IsValid) return View(model);

            var user   = new ApplicationUser {UserName = model.UserName};
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded) {
                await _databaseService.RegisterAccount(new Account {UserId = user.Id});
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginModel loginModel) {
            if (!ModelState.IsValid) return View(loginModel);

            var result = await _signInManager.PasswordSignInAsync(loginModel.AccountName, loginModel.Password, loginModel.RememberMe, false);

            if (result.Succeeded) {
                return RedirectToAction("Index", "Home");
            }

            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}