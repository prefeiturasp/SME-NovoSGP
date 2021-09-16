using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ConsultasConselhoClasseRecomendacaoConsultaDto
    {
        public IEnumerable<FechamentoAlunoAnotacaoConselhoDto> AnotacoesAluno { get; set; }

        public string AnotacoesPedagogicas { get; set; }
        public string RecomendacaoAluno { get; set; }
        public string RecomendacaoFamilia { get; set; }
        public bool SomenteLeitura { get; set; }
        public AuditoriaDto Auditoria { get; set; }
        public string SituacaoConselho { get; set; }
    }
}