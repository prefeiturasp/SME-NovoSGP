using SME.SGP.Dominio;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroGraficoDevolutivasEstimadasEConfirmadasDto
    {
        [Required]
        public int AnoLetivo { get; set; }

        [Required]
        public Modalidade Modalidade { get; set; }

        public long? DreId { get; set; }
        public long? UeId { get; set; }
    }
}