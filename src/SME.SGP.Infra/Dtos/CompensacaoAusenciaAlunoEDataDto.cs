using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class CompensacaoAusenciaAlunoEDataDto
    {
        public long CompensacaoAusenciaAlunoId { get; set; }
        public long CompensacaoAusenciaAlunoAulaId { get; set; }
        public int QuantidadeCompensacoes { get; set; }
        public int QuantidadeRegistrosFrequenciaAluno { get; set; }
        public long CompensacaoAusenciaId { get; set; }
    }
}
