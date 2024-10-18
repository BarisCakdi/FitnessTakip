using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FitnessTakip.Model
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }

        [JsonIgnore]
        public List<Education> Educations { get; set; }
    }
}
