using SME.SGP.Dominio.Enumerados;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class EncaminhamentoAeeDto
    {
        public EncaminhamentoAeeDto()
        {
            Secoes = new List<EncaminhamentoAEESecaoDto>();
        }
        public long? Id { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public SituacaoAEE Situacao { get; set; }
        public List<EncaminhamentoAEESecaoDto> Secoes { get; set; }
    }
}
