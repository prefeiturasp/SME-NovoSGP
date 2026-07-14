using System.Collections.Generic;

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
