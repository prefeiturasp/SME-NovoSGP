using System.Collections.Generic;

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
