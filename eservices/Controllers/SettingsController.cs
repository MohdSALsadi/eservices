using Microsoft.AspNetCore.Mvc;
using Pattern_of_life.Models;
using Pattern_of_life.Repository;
using Pattern_of_life.Repository.Interface;

namespace Pattern_of_life.Controllers
{
    public class SettingsController : Controller
    {
        private readonly SettingsRepository _repository;
        public SettingsController(SettingsRepository repository)
        {
            _repository = repository;

        }
        public void Index(int id)
        {
            _repository.UpdateDistanceThreshold(id);
        }
    }
}
