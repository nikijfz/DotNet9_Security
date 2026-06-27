using System.ComponentModel.DataAnnotations;

namespace Net9.Security.Controllers.Dtos
{
    public class RegisterRoleDto
    {
        [Required]
        public string RoleName { get; set; }
    }
}
