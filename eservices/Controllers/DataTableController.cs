using Pattern_of_life.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Pattern_of_life.Controllers
{
    public class DataTablesParameters
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public Search? Search { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Subject { get; set; }
        public string? Status { get; set; }
        public string? Originator { get; set; }
        public int? DepartmentId { get; set; }
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
    }

    public class Search
    {
        public string? Value { get; set; }
        public bool Regex { get; set; }
    }
    //[CustomAuthorize("aw")]
    public class DataTableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetData(DataTablesParameters parameters)
        {
            // Simulate data retrieval
            var allData = GenerateSampleData(); // Call a method to generate sample data
            var filteredData = ApplyFilters(allData, parameters.Search.Value); // Apply search filters

            // Apply paging
            var pagedData = filteredData.Skip(parameters.Start).Take(parameters.Length).ToList();

            // Prepare data in the format expected by DataTables
            var response = new
            {
                draw = parameters.Draw,
                recordsTotal = allData.Count,
                recordsFiltered = filteredData.Count(),
                data = pagedData
            };

            return Json(response);
        }

        private List<object> GenerateSampleData()
        {
            // Generate sample data
            List<object> data = new List<object>();
            for (int i = 1; i <= 22; i++)
            {
                data.Add(new { ID = i, Name = "Item " + i });
            }
            return data;
        }

        private IEnumerable<object> ApplyFilters(IEnumerable<dynamic> data, string searchValue)
        {
            // Apply search filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(d => d.Name.Contains(searchValue));
            }
            return data;
        }
    }
}
