using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ConsultasConselhoClasseRecomendacaoConsultaDto
    {
        public long FechamentoTurmaId { get; set; }
        public long ConselhoClasseId { get; set; }
        public IEnumerable<FechamentoAlunoAnotacaoConselhoDto> AnotacoesAluno { get; set; }

        public string AnotacoesPedagogicas { get; set; }
        public int Bimestre { get; set; }
        public string RecomendacaoAluno { get; set; }
        public string RecomendacaoFamilia { get; set; }
        public DateTime? PeriodoFechamentoInicio { get; set; }
        public DateTime? PeriodoFechamentoFim { get; set; }
        public bool SomenteLeitura { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}