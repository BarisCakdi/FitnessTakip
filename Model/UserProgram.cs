using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FitnessTakip.Model
{
    public class UserProgram
    {
        public int Id { get; set; }

        [JsonIgnore]
        public DateTime KayitTarih { get; set; } = DateTime.Now;
        public int Price { get; set; }
        public int Credi { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int LessonId { get; set; } 
        [ForeignKey("LessonId")]
        public Lesson Lesson { get; set; }

        public int EducationId { get; set; }
        [ForeignKey("EducationId")]
        public Education Education { get; set; }

        public bool HasEntryPermission()
        {
            return this.Credi > 0;
        }
    }
}
