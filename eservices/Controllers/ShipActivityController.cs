using Microsoft.AspNetCore.Mvc;
using Pattern_of_life.Models;
using Pattern_of_life.Models.Entity;
using Pattern_of_life.Repository.Interface;
using System.Linq.Expressions;

namespace Pattern_of_life.Controllers
{
    public class ShipActivityController : Controller
    {
        private readonly IRepository<ShipActivity> _repository;
        private readonly IRepository<VesselType> _vesselTypeRepository;
        private readonly IRepository<FlagState> _flagStateRepository;
        private readonly IRepository<ActivityName> _activityNameRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [ActivatorUtilitiesConstructor]
        public ShipActivityController(
            IRepository<ShipActivity> repository,
            IRepository<VesselType> vesselTypeRepository,
            IRepository<FlagState> flagStateRepository,
            IRepository<ActivityName> activityNameRepository
          ,IWebHostEnvironment hostingEnvironment)
        {
            _repository = repository;
            _vesselTypeRepository = vesselTypeRepository;
            _flagStateRepository = flagStateRepository;
            _activityNameRepository = activityNameRepository;
            _hostingEnvironment = hostingEnvironment;

        }
        public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate)
        {
            var shipActivities = await _repository.GetAllIncludingAsync(
                sa => sa.VesselType,
                sa => sa.FlagState,
                sa => sa.ActivityName
            );
            // Apply date range filter if provided
            if (fromDate != null)
            {
                shipActivities = shipActivities.Where(sa => sa.DTG >= fromDate);
            }

            if (toDate != null)
            {
                shipActivities = shipActivities.Where(sa => sa.DTG <= toDate);
            }
            return View(shipActivities);
        }

        public async Task<IActionResult> Details(int id)
        {
            var shipActivity = await _repository.GetById(id);
            if (shipActivity == null)
            {
                return NotFound();
            }
            return PartialView("_shipActivitiesDetails", shipActivity);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new ShipActivityViewModel
            {
                DTG=DateTime.Now,
                VesselTypes = await _vesselTypeRepository.GetAll(),
                FlagStates = await _flagStateRepository.GetAll(),
                ActivityNames = await _activityNameRepository.GetAll()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ShipActivityViewModel viewModel, IFormFile? ImagePath)
        {
            if (ModelState.IsValid)
            {
                /// Check if a file was uploaded
                if (ImagePath != null && ImagePath.Length > 0)
                {
                    // Specify the destination folder to save the file
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

                    // Ensure the destination folder exists, create it if not
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate a unique filename for the uploaded file
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImagePath.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the uploaded file to the server
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        ImagePath.CopyTo(fileStream);
                    }

                    // Set the FlagImagePath property of the FlagState object to the file path
                    viewModel.ImagePath = filePath;
                }
                var shipActivity = new ShipActivity
                {
                    // Map properties from viewModel to shipActivity
                    DTG = viewModel.DTG,
                    Longitude = viewModel.Longitude,
                    Latitude = viewModel.Latitude,
                    LongitudeDMS = viewModel.LongitudeDMS,
                    LatitudeDMS = viewModel.LatitudeDMS,
                    ImagePath = viewModel.ImagePath,
                    Course = viewModel.Course,
                    IMO = viewModel.IMO,
                    POB = viewModel.POB,
                    Remarks = viewModel.Remarks,
                    Speed = viewModel.Speed,
                    Name = viewModel.Name,
                    VesselTypeID = viewModel.VesselTypeID,
                    FlagStateID = viewModel.FlagStateID,
                    ActivityNameID = viewModel.ActivityNameID,
                    SideNumber=viewModel.SideNumber
                };
                await _repository.Add(shipActivity);
                return RedirectToAction(nameof(Index));
            }

            viewModel.VesselTypes = await _vesselTypeRepository.GetAll();
            viewModel.FlagStates = await _flagStateRepository.GetAll();
            viewModel.ActivityNames = await _activityNameRepository.GetAll();
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var shipActivity = await _repository.GetById(id);
            if (shipActivity == null)
            {
                return NotFound();
            }

            var viewModel = new ShipActivityViewModel
            {
                ID = shipActivity.ID,
                // Map other properties from shipActivity to viewModel
                DTG = shipActivity.DTG,
                Longitude = shipActivity.Longitude,
                Latitude = shipActivity.Latitude,
                LongitudeDMS = shipActivity.LongitudeDMS,
                LatitudeDMS = shipActivity.LatitudeDMS,
                Course = shipActivity.Course,
                IMO = shipActivity.IMO,
                POB = shipActivity.POB,
                Remarks = shipActivity.Remarks,
                Speed = shipActivity.Speed,
                Name = shipActivity.Name,
                VesselTypeID = shipActivity.VesselTypeID,
                FlagStateID = shipActivity.FlagStateID,
                SideNumber = shipActivity.SideNumber,
                ActivityNameID = shipActivity.ActivityNameID,
                VesselTypes = await _vesselTypeRepository.GetAll(),
                FlagStates = await _flagStateRepository.GetAll(),
                ActivityNames = await _activityNameRepository.GetAll()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ShipActivityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var shipActivity = await _repository.GetById(viewModel.ID);
                if (shipActivity == null)
                {
                    return NotFound();
                }

                // Update shipActivity properties from viewModel
                shipActivity.DTG = viewModel.DTG;
                shipActivity.Longitude = viewModel.Longitude;
                shipActivity.Latitude = viewModel.Latitude;
                shipActivity.Course = viewModel.Course;
                shipActivity.IMO = viewModel.IMO;
                shipActivity.POB = viewModel.POB;
                shipActivity.Remarks = viewModel.Remarks;
                shipActivity.Speed = viewModel.Speed;
                shipActivity.Name = viewModel.Name;
                shipActivity.VesselTypeID = viewModel.VesselTypeID;
                shipActivity.FlagStateID = viewModel.FlagStateID;
                shipActivity.ActivityNameID = viewModel.ActivityNameID;
                await _repository.Update(shipActivity);
                return RedirectToAction(nameof(Index));
            }

            viewModel.VesselTypes = await _vesselTypeRepository.GetAll();
            viewModel.FlagStates = await _flagStateRepository.GetAll();
            viewModel.ActivityNames = await _activityNameRepository.GetAll();
            return View(viewModel);
        }

    }
}
