using ChatApplication.Models;
using ChatApplication.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Controllers
{
    [Route("{controller}/{action}")]
    public class AccountController : Controller
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(AppDbContext appDbContext, UserManager<User> userManager , SignInManager<User> signInManager)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
       
        [HttpPost]

        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = registerViewModel.UserName,
                    UserName = registerViewModel.UserName

                };
                var result = await userManager.CreateAsync(user, password: registerViewModel.password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return Json(true);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }


            return Json(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));

            


            
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        
        public async Task<IActionResult> Login(LoginViewModel myLoginModel)
        {
            if (ModelState.IsValid)
            {


                

                
                   

                    var signInResult = await signInManager.PasswordSignInAsync(myLoginModel.UserName, myLoginModel.password, false, false);
                    if (signInResult.Succeeded)
                    {
                        return Json(true);
                    }
                    else
                    {
                        ModelState.AddModelError("", errorMessage: "Either username or password is wrong");

                    }

                
               


            }
            return Json(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));

        }
    }
}
