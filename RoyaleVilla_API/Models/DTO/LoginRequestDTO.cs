using System.ComponentModel.DataAnnotations;

namespace RoyaleVilla_API.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        [EmailAddress]
        public required string EmailAddress { get; set; }

        [Required]
        public required string Password { get; set; }

    }
}
