using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RespostaQuestaoObrigatoriaDto
    {
        public long QuestaoId { get; set; }
        public string Resposta { get; set; }
        public bool Persistida { get; set; }
    }
}
