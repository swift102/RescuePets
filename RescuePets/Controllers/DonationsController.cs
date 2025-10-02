using RescuePets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RescuePets.Controllers
{
    public class DonationsController : Controller
    {
        private RescueDBEntities db = new RescueDBEntities();
        private readonly decimal DONATION_GOAL = 10000m; // Set your fundraising goal here

        // GET: Donations
        public ActionResult Index()
        {
            var totalDonations = db.Donations.Sum(d => (decimal?)d.Amount) ?? 0;
            var progressPercentage = Math.Min((totalDonations / DONATION_GOAL) * 100, 100);

            var model = new DonationViewModel
            {
                Users = new SelectList(db.Users.Select(u => new {
                    UserID = u.UserID,
                    FullName = u.FirstName + " " + u.LastName
                }), "UserID", "FullName"),
                TotalDonations = totalDonations,
                DonationGoal = DONATION_GOAL,
                ProgressPercentage = progressPercentage,
                GoalReached = totalDonations >= DONATION_GOAL
            };

            return View(model);
        }

        // POST: Donations/Donate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Donate(int donorId, decimal amount)
        {
            if (amount > 0)
            {
                try
                {
                    var donation = new Donation
                    {
                        DonorUserID = donorId,
                        Amount = amount,
                        DonationDate = DateTime.Now
                    };

                    db.Donations.Add(donation);
                    db.SaveChanges();

                    TempData["Success"] = "Thank you for your generous donation!";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "An error occurred while processing your donation. Please try again.";
                }
            }
            else
            {
                TempData["Error"] = "Please enter a valid donation amount greater than R0.";
            }

            return RedirectToAction("Index");
        }

        // AJAX method to get phone number by user (for consistency with PetsController)
        public JsonResult GetPhoneByUser(int userId)
        {
            var user = db.Users.Find(userId);
            return Json(new { phoneNumber = user?.PhoneNumber ?? "" }, JsonRequestBehavior.AllowGet);
        }

 
    }
    // ViewModel class for Donations
    public class DonationViewModel
    {
        public SelectList Users { get; set; }
        public decimal TotalDonations { get; set; }
        public decimal DonationGoal { get; set; }
        public decimal ProgressPercentage { get; set; }
        public bool GoalReached { get; set; }
    }
}