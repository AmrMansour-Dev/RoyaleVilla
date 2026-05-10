using System.ComponentModel.DataAnnotations;

namespace RoyalVilla.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Email { get; set; }

        public string? Name { get; set; }

        public string? Role { get; set; }

    }
}
