using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RoyaleVilla_API.Data;
using RoyaleVilla_API.Services;
using RoyalVilla.DTO;

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


        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
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

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<LoginResponseDTO>>> Login(LoginRequestDTO loginRequestDTO)
        {
            //auth services:

            try
            {
                if (loginRequestDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Login Data is Required"));
                }

                var LoginResponseObj = await _AuthenticationService.LoginAsync(loginRequestDTO);

                if (LoginResponseObj == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Login Failed"));
                }

                var response = ApiResponse<LoginResponseDTO>.Ok("Login Success", LoginResponseObj);

                return Ok(response);
            }

            catch (Exception ex)
            {
                var response = ApiResponse<object>.Error(500, "An Error Occured While Login The User", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

    }
}
