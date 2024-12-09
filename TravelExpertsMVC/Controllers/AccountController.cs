using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelExpertsData.Models;
using TravelExpertsMVC.Models;

namespace TravelExpertsMVC.Controllers
{
    public class AccountController : Controller
    {
        private TravelExpertsContext context { get; set; }
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(SignInManager<User> signInManager,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        TravelExpertsContext context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel loginModel)
        {
            if (ModelState.IsValid) 
            {
                var result = await signInManager.PasswordSignInAsync(loginModel.Email,
                    loginModel.Password, loginModel.RememberMe, false);
                if (result.Succeeded) 
                {
                    return RedirectToAction("Index", "Home");
                }
                else 
                {
                    ModelState.AddModelError("", "Invalid Login");
                    return View();
                }
            }
            return View();
        }

        public async Task<IActionResult> LogoutAsync()
        {
            //sign out user
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new()
                {
                    CustFirstName = registerModel.CustFirstName!,
                    CustLastName = registerModel.CustLastName!,
                    CustAddress = registerModel.CustAddress!,
                    CustCity = registerModel.CustCity!,
                    CustProv = registerModel.CustProv!,
                    CustPostal = registerModel.CustPostal!,
                    CustCountry = registerModel.CustCountry!,
                    CustHomePhone = registerModel.CustHomePhone!,
                    CustBusPhone = registerModel.CustBusPhone!,
                    CustEmail = registerModel.CustEmail!
                };
                context.Customers.Add(customer);
                await context.SaveChangesAsync();
                User user = new()
                {
                    CustomerId = 144,
                    UserName = registerModel.CustEmail,
                    Email = registerModel.CustEmail,
                    FullName = registerModel.CustFirstName+" "+registerModel.CustLastName
                };
                //store user object in db
                var result = await userManager.CreateAsync(user, registerModel.Password!);

                if (result.Succeeded)//if successful, sign in the user
                {
                    //assign tole to user
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                //otherwise display errors
                foreach (var item in result.Errors)
                    ModelState.AddModelError("", item.Description);
            }

            return View();
        }


    }
}
