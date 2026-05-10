using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyaleVilla_API.Data;
using RoyaleVilla_API.Models;
using RoyalVilla.DTO;

namespace RoyaleVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAmenitiesController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;
        public VillaAmenitiesController(ApplicationDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaAmenitiesDTO>>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse<IEnumerable<VillaAmenitiesDTO>>>> GetAllVillaAmenities()
        {
            var villasamenities = await _db.VillaAmenities.ToListAsync();

            var VillaAmenitiesDTOResult = _mapper.Map<List<VillaAmenitiesDTO>>(villasamenities);

            return Ok(ApiResponse<IEnumerable<VillaAmenitiesDTO>>.Ok("Villa Amenities Retrieved Successfully", VillaAmenitiesDTOResult));
        }


        [HttpGet("{ID:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]

        public async Task <ActionResult<ApiResponse<VillaAmenitiesDTO>>> GetVillaAmenitiesByID(int ID)
        {
            try
            {
                if (ID <= 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("Villa Amenities ID Must Be Greater Than 0 !"));
                }
                else
                {
                    var villaAmenitiesobj = _db.VillaAmenities.FirstOrDefault(V => V.Id == ID);

                    if (villaAmenitiesobj == null)
                    {
                        return NotFound(ApiResponse<object>.NotFound($"Villa Amenities With ID:{ID} Was not Found!"));
                    }
                    else
                    {
                        var VillaAmenitiesDTOResult = _mapper.Map<VillaAmenitiesDTO>(villaAmenitiesobj);

                        return Ok(ApiResponse<VillaAmenitiesDTO>.Ok($"Record Retrieved Successfully", VillaAmenitiesDTOResult));
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An Error Occured While Retrieving Villa Amenities With ID {ID}:{ex.Message}");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> CreateVillaAmenities([FromBody]VillaAmenitiesCreateDTO VillaAmenitiesCreateDTO) // We used VillaAmenitiesDTO Instead of VillaAmenitiesCreateDTO To pass the ID
        {
            try
            {
                if (VillaAmenitiesCreateDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa Amenities data is REQUIRED!"));
                }
                else
                {

                    // Populate With Mapper:

                    var VillaExists = await _db.Villas.FirstOrDefaultAsync(V => V.Id == VillaAmenitiesCreateDTO.VillaId);

                    if (VillaExists == null)
                    {
                        return Conflict(ApiResponse<object>.Conflict($"Villa With ID: {VillaAmenitiesCreateDTO.VillaId} Does not Exists!"));
                    }

                    VillaAmenities VillaAmenitiesobj = _mapper.Map<VillaAmenities>(VillaAmenitiesCreateDTO);
                    VillaAmenitiesobj.CreatedDate = DateTime.UtcNow;

                    await _db.VillaAmenities.AddAsync(VillaAmenitiesobj);
                    await _db.SaveChangesAsync();

                    var VillaAmenitiesDTOResult = _mapper.Map<VillaAmenitiesDTO>(VillaAmenitiesobj);

                    return CreatedAtAction(nameof(GetVillaAmenitiesByID), new { ID = VillaAmenitiesobj.Id },ApiResponse<VillaAmenitiesDTO>.CreatedAt("Villa Amenities Created Successfully", VillaAmenitiesDTOResult));
                }
            }
            catch (Exception ex)
            {
                var errorresponse = ApiResponse<object>.Error(500, "", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorresponse);
            }
        }

        [HttpPut("{ID}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> UpdateVillaAmenities(int ID, VillaAmenitiesUpdateDTO VillaAmenitiesUpdateDTO)
        {
            try
            {
                if (VillaAmenitiesUpdateDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa Amenities data is REQUIRED!"));
                }

                if (VillaAmenitiesUpdateDTO.Id != ID)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa Amenities ID in Url Does Not Match Villa Amenities ID in Request Body!"));
                }
                else
                {
                    var ExistingVillaAmenities = await _db.Villas.FirstOrDefaultAsync(V => V.Id == VillaAmenitiesUpdateDTO.VillaId);

                    if (ExistingVillaAmenities == null)
                    {
                        return NotFound(ApiResponse<object>.NotFound($"Villa With ID '{VillaAmenitiesUpdateDTO.VillaId}' is not exist"));
                    }

                    var VillaAmenitiesExists = await _db.VillaAmenities.FirstOrDefaultAsync(V => V.Id == ID);

                    if (VillaAmenitiesExists == null)
                    {
                        //return Conflict($"VillaAmenities With Name: {VillaAmenitiesDTO.Name} already Exists!");
                        return Conflict(ApiResponse<object>.Conflict($"Villa Amenities With ID: {VillaAmenitiesUpdateDTO.VillaId} Does not Exists!"));
                    }

                    _mapper.Map(VillaAmenitiesUpdateDTO, VillaAmenitiesExists);
                    VillaAmenitiesExists.UpdatedDate = DateTime.Now;

                    await _db.SaveChangesAsync();

                    return Ok(ApiResponse<VillaAmenitiesDTO>.Ok("Villa Amenities Updated Successfully", _mapper.Map<VillaAmenitiesDTO>(VillaAmenitiesExists)));
                }
            }
            catch (Exception ex)
            {
                var errorresponse = ApiResponse<object>.Error(500, "", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, errorresponse);
            }
        }

        [HttpDelete("{ID}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteVillaAmenities(int ID)
        {
            try
            {

                VillaAmenities ExistingVillaAmenities = await _db.VillaAmenities.FirstOrDefaultAsync(V => V.Id == ID);

                if (ExistingVillaAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa Amenities With {ID} is not found"));
                }

                _db.VillaAmenities.Remove(ExistingVillaAmenities);

                await _db.SaveChangesAsync();

                return Ok(ApiResponse<object>.NoContent("Villa Amenities Deleted Successfully!"));

            }
            catch (Exception ex)
            {
                var errorresponse = ApiResponse<object>.Error(500, "Error Occured While Deleting the Villa Ameneties", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, errorresponse);
            }
        }

    }
}
