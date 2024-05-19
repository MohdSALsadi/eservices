using Microsoft.AspNetCore.Mvc;
using Pattern_of_life.Models.Entity;
using Pattern_of_life.Repository.Interface;

namespace eservices.Controllers
{
    public class LocationController : Controller
    {
        private readonly IRepository<Location> _repository;

        public LocationController(IRepository<Location> repository)

        { _repository = repository; }

        public async Task<IActionResult> Index()
        {
            var location = await _repository.GetAll();
            return View(location);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Location location)
        {
            if (ModelState.IsValid)
            {
                await _repository.Add(location); // Utilizing the Add method from the repository
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var location = await _repository.GetById(id);
            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _repository.Update(location);
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
