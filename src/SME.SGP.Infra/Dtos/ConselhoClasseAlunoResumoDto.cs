using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ConselhoClasseAlunoResumoDto
    {
        public long? ConselhoClasseTurmaId { get; set; }
        public long FechamentoTurmaId { get; set; }
        public int Bimestre { get; set; }
        public DateTime? PeriodoFechamentoInicio { get; set; }
        public DateTime? PeriodoFechamentoFim { get; set; }
    }
}
