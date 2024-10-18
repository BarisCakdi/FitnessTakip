using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FitnessTakip.Model
{
    public class Lesson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public int CreatedUserId { get; set; }
        [JsonIgnore]
        public List<UserProgram> UserPrograms { get; set; }
    }
}
