using SME.SGP.Dominio;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroGraficoFrequenciaGlobalPorDREDto
    {
        [Required]
        public int AnoLetivo { get; set; }
        [Required]
        public Modalidade Modalidade { get; set; }
        public string Ano { get; set; }
        public int? Semestre { get; set; }
    }
}