using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroConsolicacaoGeralDashBoardFrequenciaDto
    {
        [Required]
        public int AnoLetivo { get; set; }

        [Required]
        public int Mes { get; set; }
    }
}
