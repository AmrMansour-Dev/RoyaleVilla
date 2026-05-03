using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyaleVilla_API.Data;
using RoyaleVilla_API.Models;

namespace RoyaleVilla_API.Controllers
{
    [Route("api/Villa")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        public VillaController(ApplicationDBContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Villa>>> GetVillas()
        {
            return Ok(await _db.Villas.ToListAsync());
        }

        [HttpGet("{ID:int}")]
        public string GetVillaByID(int ID)
        {
            return "Get Villa : " +ID;
        }

    }
}
