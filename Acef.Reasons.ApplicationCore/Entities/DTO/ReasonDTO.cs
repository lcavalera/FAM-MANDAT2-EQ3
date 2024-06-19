using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Acef.Reasons.ApplicationCore.Entities.DTO
{
    public class ReasonDTO
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [Required(ErrorMessage = "The consultation reason name is mandatory")]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
