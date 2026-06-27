using System.ComponentModel.DataAnnotations;

namespace Net9.Security.Controllers.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
