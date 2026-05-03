using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyaleVilla_API.Data;
using RoyaleVilla_API.Models;
using RoyaleVilla_API.Models.DTO;

namespace RoyaleVilla_API.Controllers
{
    [Route("api/Villa")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;
        public VillaController(ApplicationDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
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

        [HttpPost]
        public async Task<ActionResult<Villa>> CreateVilla(VillaCreateDTO VillaDTO)
        {
            try
            {
                if (VillaDTO == null)
                {
                    return BadRequest("Villa data is REQUIRED!");
                }
                else
                {
                    //The Following CODE Used Without Mapper :

                    //Villa Villaobj = new Villa()
                    //{
                    //    Name = VillaDTO.Name,
                    //    Details = VillaDTO.Details,
                    //    Rate = VillaDTO.Rate,
                    //    CreatedDate = DateTime.Now,
                    //    Sqft = VillaDTO.Sqft,
                    //    Occupancy = VillaDTO.Occupancy,
                    //    ImageUrl = VillaDTO.ImageUrl
                    //};

                    // With Mapper:

                    Villa Villaobj = _mapper.Map<Villa>(VillaDTO);

                    await _db.Villas.AddAsync(Villaobj);
                    await _db.SaveChangesAsync();

                    return Ok(Villaobj);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An Error Occured While Creating Villa:{ex.Message}");
            }
        }

    }
}
