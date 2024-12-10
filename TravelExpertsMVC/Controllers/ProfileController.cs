using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Web;
using TravelExpertsData.Models;
using TravelExpertsMVC.Models;

namespace TravelExpertsMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly TravelExpertssContext _context;
        private string imageFolder = "~/images/";


        public ProfileController(TravelExpertssContext context)
        {
            _context = context;
        }

        //GET: Profile front end display
        public async Task<IActionResult> View(int CustomerId)
        {
            //Get Customer data from database
            var Profile = await _context.Customers.ToListAsync();
            if (Profile == null)
            {
                return NotFound();
            }

            //Display customer data in profile
            return View(Profile);
        }

        //GET: Profile edit view
        public async Task<IActionResult> Edit(int CustomerId)
        {
            var ProfileEditor = await _context.Customers.ToListAsync();
            if (ProfileEditor == null)
            {
                return NotFound();
            }
            return View(ProfileEditor);
        }

        //POST: Profile Edit View
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int CustomerId, [Bind("Customer Id, Customer First Name, Customer Last Name, Customer Address, Customer City, Customer Province, Customer Country, Customer Home Phone, Customer Business Phone, Customer Email")] Customer Customer)
        {
            if (CustomerId != Customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Customer);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (!_context.Customers.Any(c => c.CustomerId == CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Profile", new { id = Customer.CustomerId });
            }
            return View();
        }

        //getting User data for profile
        public ActionResult Index()
        {
            var model = new ProfileViewModel
            {
                ProfilePicturePath = "~/Uploads/images/"
            };

            return View(model);
        }

        //Handling User Image Upload
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase ProfileImage)
        {
            if (ProfileImage != null)
            {
                var allowExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtensions = Path.GetExtension(ProfileImage.FileName).ToLower();

                if (allowExtensions.Contains(fileExtensions)) 
                {
                    var user = TravelExpertsData.Models.Customer.Find();
                }
            }
        }
    }
}