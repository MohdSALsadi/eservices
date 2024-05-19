using Microsoft.AspNetCore.Mvc;
using Pattern_of_life.Models;
using Pattern_of_life.Models.Entity;
using Pattern_of_life.Repository.Interface;

namespace Pattern_of_life.Controllers
{
    public class ShipMovementController : Controller
    {
        private readonly IRepository<ShipMovement> _repository;
        private readonly IRepository<VesselType> _vesselTypeRepository;
        private readonly IRepository<FlagState> _flagStateRepository;

        public ShipMovementController(IRepository<ShipMovement> repository,
              IRepository<VesselType> vesselTypeRepository,
           IRepository<FlagState> flagStateRepository)
        {
            _repository = repository;
            _vesselTypeRepository = vesselTypeRepository;
            _flagStateRepository = flagStateRepository;
          
        }

        public async Task<IActionResult> Index()
        {
          
            var shipMovements = await _repository.GetAllIncludingAsync(
             sa => sa.VesselType,
             sa => sa.FlagState
         );
            return View(shipMovements);
        }

        public async Task<IActionResult> Details(int id)
        {
            var shipMovement = await _repository.GetById(id);
            if (shipMovement == null)
            {
                return NotFound();
            }
            return View(shipMovement);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new ShipMovementViewModel
            {
                EAT = DateTime.Now,
                ETD = DateTime.Now,
                VesselTypes = await _vesselTypeRepository.GetAll(),
                FlagStates = await _flagStateRepository.GetAll(),
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShipMovementViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var shipMovement = new ShipMovement
                {
                    VesselName=viewModel.VesselName,
                    ETD=viewModel.ETD,
                    EAT= viewModel.ETD,
                    ExtraDetails = viewModel.ExtraDetails,
                    FlagStateID= viewModel.FlagStateID,
                    Berth= viewModel.Berth,
                    VesselTypeID= viewModel.VesselTypeID,
                    IMO= viewModel.IMO,
                    ProposeOfArrival= viewModel.ProposeOfArrival,
                    
                };
                await _repository.Add(shipMovement);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var shipMovement = await _repository.GetById(id);
            if (shipMovement == null)
            {
                return NotFound();
            }
            var viewModel = new ShipMovementViewModel
            {
            ETD= shipMovement.ETD,
            EAT= shipMovement.EAT,
            ExtraDetails = shipMovement.ExtraDetails,
            Berth = shipMovement.Berth,
            FlagStateID = shipMovement.FlagStateID,
            IMO = shipMovement.IMO,
            ProposeOfArrival = shipMovement.ProposeOfArrival,
            VesselTypeID = shipMovement.VesselTypeID,
            Id= shipMovement.Id,
            VesselName= shipMovement.VesselName,
            
            };
            viewModel.VesselTypes = await _vesselTypeRepository.GetAll();
            viewModel.FlagStates = await _flagStateRepository.GetAll();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ShipMovementViewModel viewModel)
        {
           

            if (ModelState.IsValid)
            {
                var shipMovement = await _repository.GetById(viewModel.Id);
                shipMovement.ETD=viewModel.ETD;
                shipMovement.EAT= viewModel.EAT;
                shipMovement.Berth= viewModel.Berth;
                shipMovement.ProposeOfArrival= viewModel.ProposeOfArrival;
                shipMovement.ExtraDetails= viewModel.ExtraDetails;
                shipMovement.FlagStateID= viewModel.FlagStateID;
                shipMovement.IMO= viewModel.IMO;
                shipMovement.VesselTypeID= viewModel.VesselTypeID;

                await _repository.Update(shipMovement);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }


        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}