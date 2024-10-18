using System.Text.Json.Serialization;

namespace FitnessTakip.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        [JsonIgnore]
        public List<UserProgram> UserPrograms { get; set; }
    }
}
