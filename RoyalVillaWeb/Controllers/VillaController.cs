using Microsoft.AspNetCore.Mvc;
using RoyalVilla.DTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;
using System.Diagnostics;

namespace RoyalVillaWeb.Controllers
{
    public class VillaController : Controller
    {
        private IVillaService _villaservice;

        public VillaController(IVillaService villaService)
        {
            _villaservice = villaService;
        }
        public async Task<IActionResult> Index()
        {
            List<VillaDTO> villaslist = new List<VillaDTO>();
            try
            {
                var response = await _villaservice.GetAllAsync<ApiResponse<List<VillaDTO>>>("");
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

        public  IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VillaCreateDTO villaCreateDTO)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                var response = await _villaservice.CreateAsync<ApiResponse<VillaDTO>>(villaCreateDTO, "");
                if (response != null && response.Success && response.Data != null)
                {
                    TempData["Success"] = $"Villa Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["Errors"] = $"An error occured:{ex.Message}";
            }

            return View(villaCreateDTO);
        }

        public async Task<IActionResult> Delete(int ID)
        {
            if (ID <= 0)
            {
                TempData["Errors"] = "Invalid Villa ID";
                return RedirectToAction("Index");
            }
            try
            {
                var response = await _villaservice.GetAsync<ApiResponse<VillaDTO>>(ID, "");

                if (response != null && response.Success && response.Data != null)
                {
                    return View(response.Data);
                }
            }
            catch(Exception ex)
            {

                TempData["Errors"] = $"An error occured:{ex.Message}";
            }


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(VillaDTO villaDTO)
        {
            try
            {
                var response = await _villaservice.DeleteAsync<ApiResponse<object>>(villaDTO.Id, "");
                if (response != null && response.Success)
                {
                    TempData["Success"] = $"Villa Deleted Successfully";
                }
            }
            catch (Exception ex)
            {
                TempData["Errors"] = $"An error occured:{ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
