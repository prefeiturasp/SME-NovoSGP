using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class MapeamentoEstudanteSecaoDto
    {
        public MapeamentoEstudanteSecaoDto()
        {
            Questoes = new List<MapeamentoEstudanteSecaoQuestaoDto>();
        }
        public long SecaoId { get; set; }
        public bool Concluido { get; set; }
        public List<MapeamentoEstudanteSecaoQuestaoDto> Questoes { get; set; }
    }
}
