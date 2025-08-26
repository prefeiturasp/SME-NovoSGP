using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class FiltroConsolidarInformacoesFrequenciaPainelEducacional
    {
        [Required]
        public int AnoLetivo { get; set; }

        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
    }
}
