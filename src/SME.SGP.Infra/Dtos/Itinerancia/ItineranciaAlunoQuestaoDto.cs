using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ItineranciaAlunoQuestaoDto
    {
        public long QuestaoId { get; set; }
        public string Descricao { get; set; }
        public string Resposta { get; set; }
        public long ItineranciaAlunoId { get; set; }
        public bool Obrigatorio { get; set; }
    }
}
