using Microsoft.AspNetCore.Mvc;
using Pattern_of_life.Models.Entity;
using Pattern_of_life.Repository.Interface;

namespace Pattern_of_life.Controllers
{
    public class ActivityNameController : Controller
    {
        private readonly IRepository<ActivityName> _repository;

        public ActivityNameController(IRepository<ActivityName> repository)

        { _repository = repository; }

        public async Task<IActionResult> Index()
        {
            var activityName = await _repository.GetAll();
            return View(activityName);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityName activityName)
        {
            if (ModelState.IsValid)
            {
                await _repository.Add(activityName); // Utilizing the Add method from the repository
                return RedirectToAction(nameof(Index));
            }
            return View(activityName);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var activityName = await _repository.GetById(id);
            if (activityName == null)
            {
                return NotFound();
            }
            return View(activityName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActivityName activityName)
        {
            if (id != activityName.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _repository.Update(activityName);
                return RedirectToAction(nameof(Index));
            }
            return View(activityName);
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
