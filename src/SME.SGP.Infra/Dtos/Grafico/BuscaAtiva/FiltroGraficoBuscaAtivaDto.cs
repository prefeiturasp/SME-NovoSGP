using SME.SGP.Dominio;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroGraficoBuscaAtivaDto
    {
        [Required]
        public int AnoLetivo { get; set; }
        public long? UeId { get; set; }
        public long? DreId { get; set; }
        [Required]
        public Modalidade Modalidade { get; set; }
        public int? Semestre { get; set; }
    }
}