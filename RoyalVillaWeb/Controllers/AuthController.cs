using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla.DTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace RoyalVillaWeb.Controllers
{
    public class AuthController : Controller
    {
        private IAuthService _authaservice;

        public AuthController(IAuthService authService)
        {
            _authaservice = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {

            try
            {
                var response = await _authaservice.LoginAsync<ApiResponse<LoginResponseDTO>>(loginRequestDTO);
                if (response != null && response.Success && response.Data != null)
                {
                    LoginResponseDTO loginResponseDTO = response.Data;

                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(loginResponseDTO.Token);

                    var Identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    Identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.First(U => U.Type == "email").Value));
                    Identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.First(U => U.Type == "role").Value));

                    var principal = new ClaimsPrincipal(Identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);

                    HttpContext.Session.SetString("JWTToken", loginResponseDTO.Token);
                    return RedirectToAction("Index","Home");


                }
                else
                {
                    TempData["Errors"] ="Username/Password is incorrect, Try Again!";
                    return View(loginRequestDTO);

                }
            }
            catch (Exception ex)
            {
                TempData["Errors"] = $"An error occured:{ex.Message}";
            }

            return View();
        }

        public IActionResult Register()
        {
            return View(new RegisterationRequestDTO()
            {
                Email = string.Empty,
                Name = string.Empty,
                Password = string.Empty,
                Role = "Customer"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterationRequestDTO registerationRequestDTO)
        {

            try
            {
                var response = await _authaservice.RegisterAsync<ApiResponse<UserDTO>>(registerationRequestDTO);
                if (response != null && response.Success && response.Data != null)
                {
                    TempData["Success"] = "Registeration Successfull! Please login with your credentials";
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    TempData["Errors"] = response?.Message?? "Registeration failed, Try Again!";
                    return View(registerationRequestDTO);

                }
            }
            catch (Exception ex)
            {
                TempData["Errors"] = $"An error occured:{ex.Message}";
            }

            return View(registerationRequestDTO);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }

    }

}
