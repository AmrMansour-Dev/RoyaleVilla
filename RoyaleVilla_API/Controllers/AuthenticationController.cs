using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyaleVilla_API.Data;
using RoyaleVilla_API.Models.DTO;

namespace RoyaleVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public AuthenticationController()
        {

        }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse<UserDTO>>> Registeration(RegisterationRequestDTO registerationRequestDTO)
        {
            //auth services:


            return Ok(ApiResponse<UserDTO>.Ok("User Registered Successfully", null!));
        }

    }
}
