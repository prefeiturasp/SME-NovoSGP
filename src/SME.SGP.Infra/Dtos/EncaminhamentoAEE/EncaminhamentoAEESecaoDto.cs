using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class EncaminhamentoAEESecaoDto
    {
        public EncaminhamentoAEESecaoDto()
        {
            Questoes = new List<EncaminhamentoAEESecaoQuestaoDto>();
        }
        public long SecaoId { get; set; }
        public bool Concluido { get; set; }
        public List<EncaminhamentoAEESecaoQuestaoDto> Questoes { get; set; }
    }
}
