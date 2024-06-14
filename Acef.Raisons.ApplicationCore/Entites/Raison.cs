using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acef.Raisons.ApplicationCore.Entites
{
    public class Raison: BaseEntity
    {
        [Required(ErrorMessage = "Veuillez renseigner le nom de la raison de la consultation")]
        public string NomRaison { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
