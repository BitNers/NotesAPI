using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesAPI.Models
{
    public class UserModel
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [MinLength(3), MaxLength(200)]

        public string? Username { get; set; }

        [Required]
        [MaxLength(255)]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        
        [Required]
        public string? PasswordSalt { get; set; }

        public ICollection<NotesModel>? Notes { get; set; }

        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime created_at { get; set; } 
        
        [DataType(DataType.DateTime)]
        public DateTime updated_at { get; set; }

        [Timestamp]
        public Byte[]? chk_timestamp { get; set; }
    }
}
