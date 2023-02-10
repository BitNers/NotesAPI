using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesAPI.Models
{
    [Table("AspNetUsers")]
    public class UserModel : IdentityUser
    {
        public ICollection<NotesModel>? Notes { get; set; }
    }
}
