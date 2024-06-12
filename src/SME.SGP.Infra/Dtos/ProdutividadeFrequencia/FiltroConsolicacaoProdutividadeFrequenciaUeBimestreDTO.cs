using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroConsolicacaoProdutividadeFrequenciaUeBimestreDTO
    {
        [Required]
        public int AnoLetivo { get; set; }

        [Required]
        public string CodigoUe { get; set; }
        [Required]
        public int Bimestre { get; set; }
    }
}
