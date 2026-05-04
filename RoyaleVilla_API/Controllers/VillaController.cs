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
        public async Task<ActionResult<ApiResponse<IEnumerable<VillaDTO>>>> GetVillas()
        {
            var villas = await _db.Villas.ToListAsync();

            var VillaDTOResult = _mapper.Map<List<VillaDTO>>(villas);

            return Ok(ApiResponse<IEnumerable<VillaDTO>>.Ok("Villas Retrieved Successfully",VillaDTOResult));
        }


        [HttpGet("{ID:int}")]
        public async Task <ActionResult<ApiResponse<VillaDTO>>> GetVillaByID(int ID)
        {
            try
            {
                if (ID <= 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("Villa ID Must Be Greater Than 0 !"));
                }
                else
                {
                    var villaobj = _db.Villas.FirstOrDefault(V => V.Id == ID);

                    if (villaobj == null)
                    {
                        return NotFound(ApiResponse<object>.NotFound($"Villa With ID:{ID} Was not Found!"));
                    }
                    else
                    {
                        var VillaDTOResult = _mapper.Map<VillaDTO>(villaobj);

                        return Ok(ApiResponse<VillaDTO>.Ok($"Record Retrieved Successfully", VillaDTOResult));
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An Error Occured While Retrieving Villa With ID {ID}:{ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<VillaDTO>>> CreateVilla(VillaCreateDTO VillaDTO) // We used VillaDTO Instead of VillaCreateDTO To pass the ID
        {
            try
            {
                if (VillaDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa data is REQUIRED!"));
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
                        //return Conflict($"Villa With Name: {VillaDTO.Name} already Exists!");
                        return Conflict(ApiResponse<object>.Conflict($"Villa With Name: {VillaDTO.Name} already Exists!"));
                    }

                    Villa Villaobj = _mapper.Map<Villa>(VillaDTO);

                    await _db.Villas.AddAsync(Villaobj);
                    await _db.SaveChangesAsync();

                    var VillaDTOResult = _mapper.Map<VillaDTO>(Villaobj);

                    return CreatedAtAction(nameof(GetVillaByID), new { ID = Villaobj.Id },ApiResponse<VillaDTO>.CreatedAt("Villa Created Successfully", VillaDTOResult));
                }
            }
            catch (Exception ex)
            {
                var errorresponse = ApiResponse<object>.Error(500, "", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorresponse);
            }
        }

        [HttpPut("{ID}")]
        public async Task<ActionResult<ApiResponse<VillaDTO>>> UpdateVilla(int ID, VillaUpdateDTO VillaDTO)
        {
            try
            {
                if (VillaDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa data is REQUIRED!"));
                }

                if (VillaDTO.Id != ID)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa ID in Url Does Not Match Villa ID in Request Body!"));
                }
                else
                {
                    Villa ExistingVilla = await _db.Villas.FirstOrDefaultAsync(V => V.Id == ID);

                    if (ExistingVilla == null)
                    {
                        return NotFound(ApiResponse<object>.NotFound($"Villa With {ID} is not found"));
                    }

                    Villa DuplicateVilla = await _db.Villas.FirstOrDefaultAsync(V=>V.Name.ToLower() == VillaDTO.Name.ToLower() &&
                    V.Id != ID);

                    if(DuplicateVilla != null)
                    {
                        return Conflict(ApiResponse<VillaUpdateDTO>.Conflict($"Villa With Name: '{VillaDTO.Name}' already Exists!"));
                    }

                    _mapper.Map(VillaDTO, ExistingVilla);
                    ExistingVilla.UpdatedDate = DateTime.Now;

                    await _db.SaveChangesAsync();

                    return Ok(ApiResponse<VillaDTO>.Ok("Villa Updated Successfully",_mapper.Map<VillaDTO>(VillaDTO)));
                }
            }
            catch (Exception ex)
            {
                var errorresponse = ApiResponse<object>.Error(500, "", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, errorresponse);
            }
        }

        [HttpDelete("{ID}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteVilla(int ID)
        {
            try
            {

                Villa ExistingVilla = await _db.Villas.FirstOrDefaultAsync(V => V.Id == ID);

                if (ExistingVilla == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa With {ID} is not found"));
                }

                _db.Villas.Remove(ExistingVilla);

                await _db.SaveChangesAsync();

                return Ok(ApiResponse<object>.NoContent("Villa Deleted Successfully!"));

            }
            catch (Exception ex)
            {
                var errorresponse = ApiResponse<object>.Error(500, "", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, errorresponse);
            }
        }

    }
}
