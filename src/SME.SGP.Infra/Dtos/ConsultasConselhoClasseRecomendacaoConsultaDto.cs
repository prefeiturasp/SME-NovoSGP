using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ConsultasConselhoClasseRecomendacaoConsultaDto
    {
        public IEnumerable<FechamentoAlunoAnotacaoConselhoDto> AnotacoesAluno { get; set; }
        public string AnotacoesPedagocias { get; set; }
        public int Bimestre { get; set; }
        public string RecomendacaoAluno { get; set; }
        public string RecomendacaoFamilia { get; set; }
    }
}