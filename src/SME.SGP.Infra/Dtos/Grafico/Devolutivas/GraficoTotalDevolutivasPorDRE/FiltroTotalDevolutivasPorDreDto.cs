using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroTotalDevolutivasPorDreDto
    {
        [Required]
        public int AnoLetivo { get; set; }

        public string Ano { get; set; }
    }
}
