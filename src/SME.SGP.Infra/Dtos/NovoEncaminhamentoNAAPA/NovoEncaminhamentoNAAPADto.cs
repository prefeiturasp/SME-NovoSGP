using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA
{
    public class NovoEncaminhamentoNAAPADto
    {
        public NovoEncaminhamentoNAAPADto()
        {
            Secoes = new List<NovoEncaminhamentoNAAPASecaoDto>();
        }
        public long? Id { get; set; }
        public long? TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public long? DreId { get; set; }
        public long? UeId { get; set; }
        public int Tipo { get; set; }
        public SituacaoNAAPA Situacao { get; set; }
        public SituacaoMatriculaAluno? SituacaoMatriculaAluno { get; set; }
        public List<NovoEncaminhamentoNAAPASecaoDto> Secoes { get; set; }
    }
}