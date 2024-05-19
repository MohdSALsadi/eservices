using Microsoft.AspNetCore.Mvc;
using Pattern_of_life.Models;
using Pattern_of_life.Models.Entity;
using Pattern_of_life.Repository;
using Pattern_of_life.Repository.Interface;
using Pattern_of_life.Services;
using System.Diagnostics;
using eservices;

namespace Pattern_of_life.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<ShipActivity> _repository;
        private readonly ShipActivityDensityCalculator _densityCalculator;
        private readonly SettingsRepository _settingsRepository;


        public HomeController(IRepository<ShipActivity> repository, ShipActivityDensityCalculator densityCalculator, SettingsRepository settingsRepository)
        {
            _repository = repository;
            _densityCalculator = densityCalculator;
            _settingsRepository = settingsRepository;

        }
        public async Task<IActionResult> Index()
        {
            var shipActivities = await _repository.GetAllIncludingAsync();
            var densityMap = _densityCalculator.CalculateDensity(shipActivities, _settingsRepository.GetDistanceThreshold());

            var viewModel = new DensityViewModel
            {
                ShipActivities = shipActivities,
                DensityMap = densityMap
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
   

    }
}