using System.ComponentModel.DataAnnotations;

namespace RoyalVilla.DTO
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
