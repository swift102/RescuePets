using RescuePets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RescuePets.Controllers
{
    public class HomeController : Controller
    {
        private RescueDBEntities db = new RescueDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            var model = new HomeViewModel
            {
                TotalAdoptions = db.Adoptions.Count(),
                RecentAdoptions = db.Adoptions
                    .OrderByDescending(a => a.AdoptionDate)
                    .Take(5)
                    .Select(a => new AdoptionInfo
                    {
                        AdopterName = a.User.FirstName + " " + a.User.LastName,
                        PetName = a.Pet.PetName,
                        AdoptionDate = a.AdoptionDate
                    })
                    .ToList()
            };

            return View(model);
        }

    }

    // ViewModel classes
    public class HomeViewModel
    {
        public int TotalAdoptions { get; set; }
        public List<AdoptionInfo> RecentAdoptions { get; set; }
    }

    public class AdoptionInfo
    {
        public string AdopterName { get; set; }
        public string PetName { get; set; }
        public DateTime AdoptionDate { get; set; }
    }
}
