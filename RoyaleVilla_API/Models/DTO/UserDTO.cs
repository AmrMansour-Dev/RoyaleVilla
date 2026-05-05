using System.ComponentModel.DataAnnotations;

namespace RoyaleVilla_API.Models.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Email { get; set; }

        public string? Name { get; set; }

        public string? Role { get; set; }

    }
}
