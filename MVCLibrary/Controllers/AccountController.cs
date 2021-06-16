using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MVCLibrary.Models;
using System.Threading.Tasks;
using MVCLibrary.ViewModels;

namespace MVCLibrary.Controllers
{
    public class AccountController : Controller
    {
        private readonly MVCLibraryContext _db;
        private readonly UserManager<LibrarianUser> _userManager;
        private readonly SignInManager<LibrarianUser> _signInManager;
        public AccountController (UserManager<LibrarianUser> userManager, SignInManager<LibrarianUser> signInManager, MVCLibraryContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register (RegisterViewModel model)
        {
            var user = new LibrarianUser { UserName = model.Email };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }   

        public ActionResult Login()
        {
            return View();
        }  

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // badlogin.Visible = true;
                return View();
            }
        }
        [HttpPost]
        public async Task<ActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}