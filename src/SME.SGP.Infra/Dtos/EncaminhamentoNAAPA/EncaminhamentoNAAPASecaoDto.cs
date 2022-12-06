using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class EncaminhamentoNAAPASecaoDto
    {
        public EncaminhamentoNAAPASecaoDto()
        {
            Questoes = new List<EncaminhamentoNAAPASecaoQuestaoDto>();
        }
        public long SecaoId { get; set; }
        public bool Concluido { get; set; }
        public List<EncaminhamentoNAAPASecaoQuestaoDto> Questoes { get; set; }
    }
}
