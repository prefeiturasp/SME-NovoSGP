using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class ConselhoClasseAlunoResumoDto
    {
        public long? ConselhoClasseId { get; set; }
        public long? FechamentoTurmaId { get; set; }
        public long? ConselhoClasseAlunoId { get; set; }
        public int Bimestre { get; set; }
        public DateTime? BimestrePeriodoInicio { get; set; }
        public DateTime? BimestrePeriodoFim { get; set; }
        public DateTime? PeriodoFechamentoInicio { get; set; }
        public DateTime? PeriodoFechamentoFim { get; set; }
        public TipoNota TipoNota { get; set; }
        public double Media { get; set; }
        public int AnoLetivo { get; set; }
    }
}
