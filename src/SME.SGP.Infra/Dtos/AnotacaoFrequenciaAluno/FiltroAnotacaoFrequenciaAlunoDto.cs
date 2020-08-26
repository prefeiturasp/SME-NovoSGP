using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroAnotacaoFrequenciaAlunoDto
    {
        public FiltroAnotacaoFrequenciaAlunoDto(string codigoAluno, long aulaId)
        {
            CodigoAluno = codigoAluno;
            AulaId = aulaId;
        }

        public string CodigoAluno { get; set; }
        public long AulaId { get; set; }
    }
}
