using Microsoft.AspNetCore.Mvc;
using RoyalVilla.DTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;
using System.Diagnostics;

namespace RoyalVillaWeb.Controllers
{
    public class HomeController : Controller
    {
        private IVillaService _villaservice;

        public HomeController(IVillaService villaService)
        {
            _villaservice = villaService;
        }
        public async Task<IActionResult> Index()
        {
            List<VillaDTO> villaslist = new List<VillaDTO>();
            try
            {
                var response = await _villaservice.GetAllAsync<ApiResponse<List<VillaDTO>>>();
                if(response != null && response.Success && response.Data != null)
                {
                    villaslist = response.Data;
                }
            }
            catch (Exception ex)
            {
                TempData["Errors"] = $"An error occured:{ex.Message}";
            }

            return View(villaslist);
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
