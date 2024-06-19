using System.ComponentModel.DataAnnotations;

namespace Acef.Reasons.ApplicationCore.Entities
{
    public class Reason : BaseEntity
    {
        [Required(ErrorMessage = "The Name field is required")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; } = null;
    }
}
