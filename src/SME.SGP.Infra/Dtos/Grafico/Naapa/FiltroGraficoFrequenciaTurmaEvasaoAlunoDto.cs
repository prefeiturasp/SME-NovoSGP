using SME.SGP.Dominio;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroGraficoFrequenciaTurmaEvasaoAlunoDto
    {
        [Required]
        public int AnoLetivo { get; set; }

        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public string TurmaCodigo { get; set; }

        [Required]
        public Modalidade Modalidade { get; set; }

        public int Semestre { get; set; }
        public int Mes { get; set; }
    }
}
