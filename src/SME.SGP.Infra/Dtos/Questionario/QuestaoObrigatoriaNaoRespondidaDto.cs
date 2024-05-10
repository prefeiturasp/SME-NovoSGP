using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class QuestaoObrigatoriaNaoRespondidaDto
    {
        public QuestaoObrigatoriaNaoRespondidaDto(long secaoId, string secaoNome, string questaoOrdem)
        {
            SecaoId = secaoId;
            QuestaoOrdem = questaoOrdem;
            SecaoNome = secaoNome;
        }

        public long SecaoId { get; set; }
        public string QuestaoOrdem { get; set; }
        public string SecaoNome { get; set; }
        
    }
}
