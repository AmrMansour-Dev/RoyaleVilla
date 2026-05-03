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
        public ActionResult<Villa> GetVillaByID(int ID)
        {
            try
            {
                if(ID <= 0)
                {
                    return BadRequest("Villa ID Must Be Greater Than 0");
                }
                else
                {
                    var villaobj = _db.Villas.FirstOrDefault(V=>V.Id == ID);

                    if(villaobj == null)
                    {
                        return NotFound($"No User Is Found with The ID: {ID}");
                    }
                    else
                    {
                        return Ok(villaobj);
                    }
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An Error Occured While Retrieving Villa With ID {ID}:{ex.Message}");
            }
        }

    }
}
