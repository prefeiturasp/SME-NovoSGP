using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ConselhoClasseAlunoDto
    {
        public long ConselhoClasseId { get; set; }
        public long FechamentoTurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public string AnotacoesPedagogicas { get; set; }
        public string RecomendacaoAluno { get; set; }
        public string RecomendacaoFamilia { get; set; }
    }
}
