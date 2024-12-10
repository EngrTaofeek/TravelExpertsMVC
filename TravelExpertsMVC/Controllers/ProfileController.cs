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
        [Route("Profile")]
        public async Task<IActionResult> Profile(int CustomerId)
        {
            //Get Customer data from database
            var Profile = await _context.Customers.ToListAsync();
            if (Profile == null)
            {
                return NotFound();
            }

            //Display customer data in profile
            return View("ProfileView");
        }

        //GET: Profile edit view
        [Route("ProfileEdit")]
        public async Task<IActionResult> ProfileEdit(int CustomerId)
        {
            var ProfileEditor = await _context.Customers.ToListAsync();
            if (ProfileEditor == null)
            {
                return NotFound();
            }
            return View("ProfileEditor");
        }

        //POST: Profile Edit View
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileEdit(int CustomerId, [Bind("Customer Id, Customer First Name, Customer Last Name, Customer Address, Customer City, Customer Province, Customer Country, Customer Home Phone, Customer Business Phone, Customer Email")] Customer Customer)
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

        [HttpPost]
        public async Task<IActionResult> ProfilePictureUpload(IFormFile profilePicture)
    {
        if (!profilePicture.ContentType.StartsWith("image/"))
        {
            ViewBag.Message = "Please upload an image file";
            return View();
        }
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        var filePath = Path.Combine(uploadPath, profilePicture.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await profilePicture.CopyToAsync(stream);
        }

        ViewBag.profilePicture = profilePicture.FileName;

        return View();
        }
    }
}