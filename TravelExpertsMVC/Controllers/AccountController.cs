using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                string? imagePath = null;

                if (registerModel.ProfileImage != null)
                {
                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(registerModel.ProfileImage.FileName).ToLower();
                    var mimeType = registerModel.ProfileImage.ContentType.ToLower();

                    if (!allowedExtensions.Contains(fileExtension) || !mimeType.StartsWith("image/"))
                    {
                        ModelState.AddModelError("ProfileImage", "Only image files (JPG, PNG, GIF) are allowed.");
                        return View(registerModel);
                    }

                    // Define the path to save the image
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsDir))
                        Directory.CreateDirectory(uploadsDir);

                    var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    imagePath = Path.Combine("uploads", uniqueFileName);
                    var filePath = Path.Combine(uploadsDir, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await registerModel.ProfileImage.CopyToAsync(fileStream);
                    }
                }

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
                    CustEmail = registerModel.CustEmail!,
                    ProfileImagePath = imagePath // Save image path to database
                };

                context.Customers.Add(customer);
                await context.SaveChangesAsync();

                User user = new()
                {
                    CustomerId = customer.CustomerId,
                    UserName = registerModel.CustEmail,
                    Email = registerModel.CustEmail,
                    FullName = registerModel.CustFirstName + " " + registerModel.CustLastName
                };

                var result = await userManager.CreateAsync(user, registerModel.Password!);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var item in result.Errors)
                    ModelState.AddModelError("", item.Description);
            }

            return View(registerModel);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Retrieve current user
            var currentUser = getCurrentUser(); // Your existing method to get the logged-in customer
            ViewData["FirstName"] = currentUser.CustFirstName;
            ViewData["ProfilePicture"] = currentUser.ProfileImagePath;
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            // Populate the view model with existing data
            var model = new ProfileViewModel
            {
                CustFirstName = currentUser.CustFirstName,
                CustLastName = currentUser.CustLastName,
                CustAddress = currentUser.CustAddress,
                CustCity = currentUser.CustCity,
                CustProv = currentUser.CustProv,
                CustPostal = currentUser.CustPostal,
                CustCountry = currentUser.CustCountry,
                CustHomePhone = currentUser.CustHomePhone,
                CustBusPhone = currentUser.CustBusPhone,
                CustEmail = currentUser.CustEmail,
                ProfileImagePath = currentUser.ProfileImagePath
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProfileAsync(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Retrieve the current user
            var currentUser = getCurrentUser();
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            // Update the user's profile details
            currentUser.CustFirstName = model.CustFirstName!;
            currentUser.CustLastName = model.CustLastName!;
            currentUser.CustAddress = model.CustAddress!;
            currentUser.CustCity = model.CustCity!;
            currentUser.CustProv = model.CustProv!;
            currentUser.CustPostal = model.CustPostal!;
            currentUser.CustCountry = model.CustCountry!;
            currentUser.CustHomePhone = model.CustHomePhone!;
            currentUser.CustBusPhone = model.CustBusPhone!;

            // Handle profile image upload
            if (model.ProfileImage != null)
            {
                if (!IsValidImage(model.ProfileImage))
                {
                    ModelState.AddModelError("", "Please upload a valid image file (JPG, JPEG, PNG, GIF).");
                    return View(model);
                }

                // Generate unique file name and save the file
                var fileName = $"{Guid.NewGuid()}_{model.ProfileImage.FileName}";
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                // Update the ProfileImagePath in the database
                currentUser.ProfileImagePath = $"/uploads/{fileName}";
            }

            // Save changes to the database
            context.Customers.Update(currentUser);
            await context.SaveChangesAsync();

            ViewBag.SuccessMessage = "Profile updated successfully.";
            return View(model);
        }

        private bool IsValidImage(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(extension);
        }

        public Customer getCurrentUser()
        {
            string userName = User.Identity.Name;

            var customer = context.Customers.FirstOrDefault(cus => cus.CustEmail == userName);
            return customer;
        }

    }
}
