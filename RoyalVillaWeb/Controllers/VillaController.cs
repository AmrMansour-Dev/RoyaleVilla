using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMapper _Mapper;


        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaservice = villaService;
            _Mapper = mapper;
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


        [Authorize(Roles = "Admin")]
        public  IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
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


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int ID)
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
                    return View(_Mapper.Map<VillaUpdateDTO>(response.Data));
                }
            }
            catch(Exception ex)
            {

                TempData["Errors"] = $"An error occured:{ex.Message}";
            }


            return View();
          }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VillaUpdateDTO villaupdateDTO)
        {
            try
            {
                var response = await _villaservice.UpdateAsync<ApiResponse<object>>(villaupdateDTO, "");
                if (response != null && response.Success && response.Data !=null)
                {
                    TempData["Success"] = $"Villa Updated Successfully";
                }
            }
            catch (Exception ex)
            {
                TempData["Errors"] = $"An error occured:{ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
