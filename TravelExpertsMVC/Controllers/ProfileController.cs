using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelExpertsData.Models;

namespace TravelExpertsMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly TravelExpertssContext _context;

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
                return RedirectToAction("Profile", new { id = Customer.CustomerId});
            }
            return View();
        }
    }
}