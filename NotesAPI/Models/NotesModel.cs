using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NotesAPI.Models
{
    public class NotesModel 
    {

        [Key]
        public int NotesID{ get; set; }

        [MinLength(1), MaxLength(100)]
        public string? Title { get; set; }
        
        [MinLength(1)]
        public string? Description { get; set; }

        public UserModel? ByUser { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime created_at { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime updated_at { get; set; } 

        [Timestamp]
        public Byte[]? chk_timestamp { get; set; }
    }
}
