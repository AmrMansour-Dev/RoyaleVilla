using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RoyaleVilla_API.Data;
using RoyaleVilla_API.Models.DTO;
using RoyaleVilla_API.Services;

namespace RoyaleVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _AuthenticationService;
        public AuthenticationController(IAuthService AuthenticationService)
        {
            _AuthenticationService = AuthenticationService;
        }


        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse<UserDTO>>> Registeration(RegisterationRequestDTO registerationRequestDTO)
        {
            //auth services:

            try
            {
                if (registerationRequestDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Registeration Data is Required"));
                }

                if (await _AuthenticationService.IsEmailExistsAsync(registerationRequestDTO.Email))
                {
                    return Conflict(ApiResponse<object>.Conflict($"User With Email '{registerationRequestDTO.Email}' is already used"));
                }

                var user = await _AuthenticationService.RegisterAsync(registerationRequestDTO);

                if (user == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Registeration Failed"));
                }

                return CreatedAtAction(nameof(Registeration), ApiResponse<UserDTO>.CreatedAt("User Registered Successfully", user));
            }

            catch (Exception ex)
            {
                var response = ApiResponse<object>.Error(500, "An Error Occured While Registering The User", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

    }
}
