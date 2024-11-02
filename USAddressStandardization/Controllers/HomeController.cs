using Microsoft.AspNetCore.Mvc;
using USAddressStandardization.Models;
using USAddressStandardization.Services;
using System.Diagnostics;

namespace USAddressStandardization.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAddressStandardizerService _standardizer;

        public HomeController(IAddressStandardizerService standardizer)
        {
            _standardizer = standardizer;
        }

        public IActionResult Index()
        {
            return View(new AddressViewModel());
        }

        [HttpPost]
        public IActionResult Index(AddressViewModel model)
        {
            //if (!ModelState.IsValid)
            //    return View(model);

            var standardizedAddress = _standardizer.StandardizeAddress(model.RawAddress);

            if (standardizedAddress == null)
            {
                model.ErrorMessage = "Unable to parse the provided address. Please check the format and try again.";
                return View(model);
            }

            model.StandardizedAddress = standardizedAddress;
            return View(model);
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
