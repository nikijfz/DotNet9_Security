using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Net9.Security.Controllers.Dtos
{
    public class EditUserRolesDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public List<string> UserRoles { get; set; }

        [Required]
        public List<IdentityRole> Roles { get; set; }
    }
}
