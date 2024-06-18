using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Acef.MVC.Models.DTO
{
    public class RaisonDTO
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Veuillez renseigner le nom de la raison de la consultation")]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
