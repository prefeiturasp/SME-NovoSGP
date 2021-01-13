using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class EncaminhamentoAEEDto
    {
        public EncaminhamentoAEEDto()
        {
            Secoes = new List<EncaminhamentoAEESecaoDto>();
        }
        public long? Id { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public List<EncaminhamentoAEESecaoDto> Secoes { get; set; }
    }
}
