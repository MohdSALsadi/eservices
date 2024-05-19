using Microsoft.AspNetCore.Mvc;
using Pattern_of_life.Repository.Interface;
using Microsoft.Extensions.Logging;
using Pattern_of_life.Models.Entity;


namespace Pattern_of_life.Controllers
{
    public class FlagStateController : Controller
    {
        private readonly IRepository<FlagState> _repository;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public FlagStateController(IRepository<FlagState> repository, IWebHostEnvironment hostingEnvironment)

        {
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var flagState = await _repository.GetAll();
            return View(flagState);
        }

        // GET: FlagState/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FlagState/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlagState flagState, IFormFile? flagImage)
        {
            if (ModelState.IsValid)
            {
                /// Check if a file was uploaded
                if (flagImage != null && flagImage.Length > 0)
                {
                    // Specify the destination folder to save the file
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

                    // Ensure the destination folder exists, create it if not
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate a unique filename for the uploaded file
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + flagImage.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the uploaded file to the server
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        flagImage.CopyTo(fileStream);
                    }

                    // Set the FlagImagePath property of the FlagState object to the file path
                    flagState.FlagImagePath = filePath;
                }
                await _repository.Add(flagState);
                return RedirectToAction(nameof(Index));
            }
            return View(flagState);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var flagState = await _repository.GetById(id);
            if (flagState == null)
            {
                return NotFound();
            }
            // Calculate relative path based on the absolute path in flagState.FlagImagePath
            string relativePath = flagState.FlagImagePath.Replace(_hostingEnvironment.WebRootPath, "").Replace("\\", "/");

            flagState.FlagImagePath = relativePath;

            return View(flagState);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FlagState flagState, IFormFile? flagImage)
        {
            if (ModelState.IsValid)
            {
                // Check if a new file was uploaded
                if (flagImage != null && flagImage.Length > 0)
                {
                    // Specify the destination folder to save the file
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

                    // Ensure the destination folder exists, create it if not
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate a unique filename for the uploaded file
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + flagImage.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the uploaded file to the server
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        flagImage.CopyTo(fileStream);
                    }

                    // Set the FlagImagePath property of the FlagState object to the file path
                    flagState.FlagImagePath = filePath;
                }
                // Always generate a new FlagImagePath before updating
        // Specify the destination folder to save the file
        var newUploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

                // Generate a unique filename for the uploaded file
                var newUniqueFileName = Guid.NewGuid().ToString() + "_FlagImage.jpg";
                var newFilePath = Path.Combine(newUploadsFolder, newUniqueFileName);

                // Set the FlagImagePath property of the FlagState object to the new file path
                flagState.FlagImagePath = newFilePath;

                // Here, you would update the FlagState entity in your database with the new data
                await _repository.Update(flagState);

                // Redirect to the Index action after successful update
                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, return the Edit view with the model
            return View(flagState);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult GetFlagImage(int Id)
        {
            // Get the flag state entity by ID
            var flagState = _repository.GetById(Id);

            if (flagState != null)
            {

                // Assuming you have a property in the FlagState entity that stores the URL of the flag image
                var flagImageUrl = flagState.Result.FlagImagePath;

                // Return the flag image URL as a JSON response
                return Json(new { imageUrl = flagImageUrl });
            }
            else
            {
                // Return a not found response if the flag state ID is invalid
                return NotFound();
            }
        }
    }
}
