using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FechamentoFinalConsultaRetornoDto
    {
        public FechamentoFinalConsultaRetornoDto()
        {
            Alunos = new List<FechamentoFinalConsultaRetornoAlunoDto>();
        }

        public IList<FechamentoFinalConsultaRetornoAlunoDto> Alunos { get; set; }
        public bool EhNota { get; set; }
        public bool EhRegencia { get; set; }
    }
}