using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Note
    {
        [Key]
        public int ID { get; set; }
        public Guid ExposedID { get; set; }
        public string Relation { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set;}

        // TODO: change this to User instead of ApplicationUser in the future
        public ApplicationUser CreatedBy { get; set; }
    }
}
