using Microsoft.AspNetCore.Mvc;
using Pattern_of_life.Models.Entity;
using Pattern_of_life.Repository.Interface;

namespace Pattern_of_life.Controllers
{
    public class VesselTypeController : Controller
    {
        private readonly IRepository<VesselType> _repository;

        public VesselTypeController(IRepository<VesselType> repository) 
        
        {_repository = repository; }

        public async Task<IActionResult> Index()
        {
            var vesselTypes = await _repository.GetAll();
            return View(vesselTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VesselType vesselType)
        {
            if (ModelState.IsValid)
            {
                await _repository.Add(vesselType); // Utilizing the Add method from the repository
                return RedirectToAction(nameof(Index));
            }
            return View(vesselType);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vesselType = await _repository.GetById(id);
            if (vesselType == null)
            {
                return NotFound();
            }
            return View(vesselType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VesselType vesselType)
        {
            if (id != vesselType.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _repository.Update(vesselType);
                return RedirectToAction(nameof(Index));
            }
            return View(vesselType);
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
