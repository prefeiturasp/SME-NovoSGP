using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroDashboardTotalRIsPorDreDTO
    {
        [Required]
        public int AnoLetivo { get; set; }
        public string Ano { get; set; }
    }
}
