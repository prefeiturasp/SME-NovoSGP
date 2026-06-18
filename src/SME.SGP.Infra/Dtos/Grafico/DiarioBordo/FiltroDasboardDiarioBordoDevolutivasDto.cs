using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroDasboardDiarioBordoDevolutivasDto
    {
        [Required]
        public int AnoLetivo { get; set; }
        public int Mes { get; set; }
        public long DreId { get; set; }
    }
}
