using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesAPI.Models
{
    public class NotesModel 
    {

        [Key]
        public int NotesID{ get; set; }

        [Required]
        [MinLength(1), MaxLength(100)]
        public string? Title { get; set; }
        
        [Required]
        [MinLength(1)]
        public string? Description { get; set; }


        public virtual UserModel? ByUser { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime created_at { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime updated_at { get; set; } 

        [Timestamp]
        public Byte[]? chk_timestamp { get; set; }
    }
}
