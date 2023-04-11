using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class CompensacaoAusenciaAlunoDto
    {
        public string Id { get; set; }
        public int QtdFaltasCompensadas { get; set; }
        public IEnumerable<CompensacaoAusenciaAlunoAulaDto> CompensacaoAusenciaAlunoAula { get; set; }
    }
}
