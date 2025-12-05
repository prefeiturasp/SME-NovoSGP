using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class AtendimentoNAAPADto
    {
        public AtendimentoNAAPADto()
        {
            Secoes = new List<AtendimentoNAAPASecaoDto>();
        }
        public long? Id { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public SituacaoNAAPA Situacao { get; set; }
        public SituacaoMatriculaAluno? SituacaoMatriculaAluno { get; set; }
    public List<AtendimentoNAAPASecaoDto> Secoes { get; set; }
    }
}
