using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class CompensacaoAusenciaListagemDto
    {
        public long Id { get; set; }
        public int Bimestre { get; set; }
        public string AtividadeNome { get; set; }
        public List<CompensacaoAusenciaListagemAlunosDto> Alunos { get; set; }
    }
}
