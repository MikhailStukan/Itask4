using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Itask_4.Areas.Identity.Data;

public class AppUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(50)")]
    public string Name { get; set; }

    public DateTime? LastLoginTime { get; set; }
    public DateTime? RegistrationTime { get; set; }

    public bool isActive { get; set; }
}
