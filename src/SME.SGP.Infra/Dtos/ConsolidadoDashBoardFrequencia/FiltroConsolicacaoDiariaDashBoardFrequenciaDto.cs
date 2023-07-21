using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroConsolicacaoDiariaDashBoardFrequenciaDto
    {
        [Required]
        public int AnoLetivo { get; set; }

        [Required]
        public int Mes { get; set; }
    }
}
