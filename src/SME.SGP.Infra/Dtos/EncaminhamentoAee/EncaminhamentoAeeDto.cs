using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

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
