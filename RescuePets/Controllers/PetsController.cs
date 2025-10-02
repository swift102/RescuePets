using RescuePets.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RescuePets.Controllers
{
    public class PetsController : Controller
    {
        private RescueDBEntities db = new RescueDBEntities();

        // GET: Pets
        public ActionResult Index(int? typeId, int? breedId, int? locationId)
        {
            var pets = db.Pets
                .Include("PetType")
                .Include("PetBreed")
                .Include("Location")
                .Include("Gender")
                .Include("User")
                .AsQueryable();

            // Apply filters
            if (typeId.HasValue && typeId > 0)
            {
                pets = pets.Where(p => p.TypeID == typeId);
            }

            if (breedId.HasValue && breedId > 0)
            {
                pets = pets.Where(p => p.BreedID == breedId);
            }

            if (locationId.HasValue && locationId > 0)
            {
                pets = pets.Where(p => p.LocationID == locationId);
            }

            var model = new PetsViewModel
            {
                Pets = pets.OrderByDescending(p => p.DatePosted).ToList(),
                PetTypes = new SelectList(db.PetTypes, "TypeID", "TypeName"),
                PetBreeds = new SelectList(db.PetBreeds, "BreedID", "BreedName"),
                Locations = new SelectList(db.Locations, "LocationID", "LocationName"),
                SelectedTypeId = typeId,
                SelectedBreedId = breedId,
                SelectedLocationId = locationId
            };

            return View(model);
        }

        // GET: Pets/Adopt/5
        public ActionResult Adopt(int id)
        {
            var pet = db.Pets
                 .Include("PetType")
                 .Include("PetBreed")
                 .Include("Location")
                 .Include("Gender")
                 .Include("User")
                .FirstOrDefault(p => p.PetID == id);

            if (pet == null || pet.Status != "Available")
            {
                return HttpNotFound();
            }

            var model = new AdoptViewModel
            {
                Pet = pet,
                Users = new SelectList(db.Users.Select(u => new
                {
                    UserID = u.UserID,
                    FullName = u.FirstName + " " + u.LastName
                }), "UserID", "FullName")
            };

            return View(model);
        }

        // POST: Pets/Adopt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Adopt(int petId, int adopterId)
        {
            try
            {
                var pet = db.Pets.Find(petId);
                if (pet != null && pet.Status == "Available")
                {
                    // Create adoption record
                    var adoption = new Adoption
                    {
                        PetID = petId,
                        AdopterUserID = adopterId,
                        AdoptionDate = DateTime.Now
                    };

                    db.Adoptions.Add(adoption);

                    // Update pet status
                    pet.Status = "Adopted";

                    db.SaveChanges();

                    TempData["Success"] = "Pet adopted successfully!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while processing the adoption.";
            }

            return RedirectToAction("Index");
        }

        // GET: Pets/Post
        public ActionResult PostaPet()
        {
            var model = new PostPetViewModel
            {
                Users = new SelectList(db.Users.Select(u => new
                {
                    UserID = u.UserID,
                    FullName = u.FirstName + " " + u.LastName
                }), "UserID", "FullName"),
                PetTypes = new SelectList(db.PetTypes, "TypeID", "TypeName"),
                PetBreeds = new SelectList(db.PetBreeds, "BreedID", "BreedName"),
                Locations = new SelectList(db.Locations, "LocationID", "LocationName"),
                Genders = new SelectList(db.Genders, "GenderID", "GenderName")
            };

            return View(model);
        }

        // POST: Pets/Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostaPet(PostPetViewModel model, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string imageData = "";
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        byte[] imageBytes = new byte[imageFile.ContentLength];
                        imageFile.InputStream.Read(imageBytes, 0, imageFile.ContentLength);
                        imageData = Convert.ToBase64String(imageBytes);
                    }

                    var pet = new Pet
                    {
                        PetName = model.PetName,
                        TypeID = model.TypeID,
                        BreedID = model.BreedID,
                        LocationID = model.LocationID,
                        Age = model.Age,
                        Weight = Convert.ToDecimal(model.Weight),
                        GenderID = model.GenderID,
                        PetStory = model.PetStory,
                        ImageData = imageData,
                        Status = "Available",
                        PostedByUserID = model.PostedByUserID,
                        DatePosted = DateTime.Now
                    };

                    db.Pets.Add(pet);
                    db.SaveChanges();

                    TempData["Success"] = "Pet posted successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "An error occurred while posting the pet.";
                }
            }

            // Reload dropdowns if validation fails
            model.Users = new SelectList(db.Users.Select(u => new
            {
                UserID = u.UserID,
                FullName = u.FirstName + " " + u.LastName
            }), "UserID", "FullName");
            model.PetTypes = new SelectList(db.PetTypes, "TypeID", "TypeName");
            model.PetBreeds = new SelectList(db.PetBreeds, "BreedID", "BreedName");
            model.Locations = new SelectList(db.Locations, "LocationID", "LocationName");
            model.Genders = new SelectList(db.Genders, "GenderID", "GenderName");

            return View(model);
        }

        // AJAX method to get breeds by type
        public JsonResult GetBreedsByType(int typeId)
        {
            var breeds = db.PetBreeds
                .Where(b => b.TypeID == typeId)
                .Select(b => new { b.BreedID, b.BreedName })
                .ToList();

            return Json(breeds, JsonRequestBehavior.AllowGet);
        }

        // AJAX method to get phone number by user
        public JsonResult GetPhoneByUser(int userId)
        {
            var user = db.Users.Find(userId);
            return Json(new { phoneNumber = user?.PhoneNumber ?? "" }, JsonRequestBehavior.AllowGet);
        }




        // ViewModel classes
        public class PetsViewModel
        {
            public List<Pet> Pets { get; set; }
            public SelectList PetTypes { get; set; }
            public SelectList PetBreeds { get; set; }
            public SelectList Locations { get; set; }
            public int? SelectedTypeId { get; set; }
            public int? SelectedBreedId { get; set; }
            public int? SelectedLocationId { get; set; }
        }

        public class AdoptViewModel
        {
            public Pet Pet { get; set; }
            public SelectList Users { get; set; }
            public int SelectedUserId { get; set; }
        }

        public class PostPetViewModel
        {
            public string PetName { get; set; }
            public int TypeID { get; set; }
            public int BreedID { get; set; }
            public int LocationID { get; set; }
            public int Age { get; set; }
            [Range(0.1, 200.0, ErrorMessage = "Weight must be between 0.1 and 200 kg")]
            [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
            public double Weight { get; set; }
            public int GenderID { get; set; }
            public string PetStory { get; set; }
            public int PostedByUserID { get; set; }

            public SelectList Users { get; set; }
            public SelectList PetTypes { get; set; }
            public SelectList PetBreeds { get; set; }
            public SelectList Locations { get; set; }
            public SelectList Genders { get; set; }
        }
    }
}
