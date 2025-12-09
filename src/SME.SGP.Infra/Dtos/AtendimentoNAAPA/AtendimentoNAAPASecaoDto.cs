using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class AtendimentoNAAPASecaoDto
    {
        public AtendimentoNAAPASecaoDto()
        {
            Questoes = new List<AtendimentoNAAPASecaoQuestaoDto>();
        }
        public long SecaoId { get; set; }
        public bool Concluido { get; set; }
        public long? EncaminhamentoEscolarId { get; set; } = null;
        public List<AtendimentoNAAPASecaoQuestaoDto> Questoes { get; set; }
    }
}
