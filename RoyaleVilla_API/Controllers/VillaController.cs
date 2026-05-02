using Microsoft.AspNetCore.Mvc;

namespace RoyaleVilla_API.Controllers
{
    [Route("api/Villa")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        [HttpGet]
        public string GetVillas()
        {
            return "Here are all Villas !";
        }

        [HttpGet("{ID:int}")]
        public string GetVillaByID(int ID)
        {
            return "Get Villa : " +ID;
        }

    }
}
