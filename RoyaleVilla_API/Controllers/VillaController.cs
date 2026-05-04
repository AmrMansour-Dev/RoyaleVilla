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
                if (ID <= 0)
                {
                    return BadRequest("Villa ID Must Be Greater Than 0");
                }
                else
                {
                    var villaobj = _db.Villas.FirstOrDefault(V => V.Id == ID);

                    if (villaobj == null)
                    {
                        return NotFound($"No User Is Found with The ID: {ID}");
                    }
                    else
                    {
                        return Ok(villaobj);
                    }
                }
            }
            catch (Exception ex)
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

                    var DuplicateVilla = await _db.Villas.FirstOrDefaultAsync(V => V.Name.ToLower() == VillaDTO.Name.ToLower());

                    if (DuplicateVilla != null)
                    {
                        return Conflict($"Villa With Name: {VillaDTO.Name} already Exists!");
                    }

                    Villa Villaobj = _mapper.Map<Villa>(VillaDTO);

                    await _db.Villas.AddAsync(Villaobj);
                    await _db.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetVillaByID), new { ID = Villaobj.Id }, Villaobj);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An Error Occured While Creating Villa:{ex.Message}");
            }
        }

        [HttpPut("{ID}")]
        public async Task<ActionResult<Villa>> UpdateVilla(int ID, VillaUpdateDTO VillaDTO)
        {
            try
            {
                if (VillaDTO == null)
                {
                    return BadRequest("Villa data is REQUIRED!");
                }

                if (VillaDTO.Id != ID)
                {
                    return BadRequest("Villa ID in Url Does Not Match Villa ID in Request Body!");
                }
                else
                {
                    Villa ExistingVilla = await _db.Villas.FirstOrDefaultAsync(V => V.Id == ID);

                    if (ExistingVilla == null)
                    {
                        return NotFound($"Villa With {ID} is not found");
                    }

                    Villa DuplicateVilla = await _db.Villas.FirstOrDefaultAsync(V=>V.Name.ToLower() == VillaDTO.Name.ToLower() &&
                    V.Id != ID);

                    if(DuplicateVilla != null)
                    {
                        return Conflict($"Villa With Name: {VillaDTO.Name} already Exists!");
                    }

                    _mapper.Map(VillaDTO, ExistingVilla);
                    ExistingVilla.UpdatedDate = DateTime.Now;

                    await _db.SaveChangesAsync();

                    return Ok(VillaDTO);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An Error Occured While Updating Villa:{ex.Message}");
            }
        }

        [HttpDelete("{ID}")]
        public async Task<ActionResult<Villa>> DeleteVilla(int ID)
        {
            try
            {

                Villa ExistingVilla = await _db.Villas.FirstOrDefaultAsync(V => V.Id == ID);

                if (ExistingVilla == null)
                {
                    return NotFound($"Villa With {ID} is not found");
                }

                _db.Villas.Remove(ExistingVilla);

                await _db.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An Error Occured While Deleting Villa:{ex.Message}");
            }
        }

    }
}
