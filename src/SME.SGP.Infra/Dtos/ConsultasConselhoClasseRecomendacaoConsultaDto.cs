using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ConsultasConselhoClasseRecomendacaoConsultaDto
    {
        public IEnumerable<FechamentoAlunoAnotacaoConselhoDto> AnotacoesPedagogicas { get; set; }
        public string RecomendacaoAluno { get; set; }
        public string RecomendacaoFamilia { get; set; }
    }
}