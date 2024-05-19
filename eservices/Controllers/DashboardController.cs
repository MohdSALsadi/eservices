using Microsoft.AspNetCore.Mvc;
using Pattern_of_life.Models.Entity;
using Pattern_of_life.Models;
using Pattern_of_life.Repository.Interface;

namespace Pattern_of_life.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IRepository<ShipActivity> _shipActivityRepository;

        public DashboardController(IRepository<ShipActivity> shipActivityRepository)
        {
            _shipActivityRepository = shipActivityRepository;
        }

        public async Task<IActionResult> Index()
        {
            var topShips = await GetTopRegisteredShips();
            return View(topShips);
        }

        private async Task<List<TopShipViewModel>> GetTopRegisteredShips()
        {
            var shipActivities = await _shipActivityRepository.GetAll();
            var groupedShips = shipActivities.GroupBy(sa => sa.IMO)
                .Select(g => new TopShipViewModel
                {
                    ShipId = g.Key,
                    ShipName = g.First().Name, // Assuming the ship name is the same for all activities with the same IMO
                    ActivitiesCount = g.Count(),
                })
                .OrderByDescending(s => s.ActivitiesCount)
                .Take(5)
                .ToList();

            return groupedShips;
        }

    }
}