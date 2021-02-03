using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class RegistroItineranciaAlunoQuestaoDto
    {
        public long Id { get; set; }
        public long QuestaoId { get; set; }
        public string Descricao { get; set; }
        public string Resposta { get; set; }
        public long RegistroItineranciaAlunoId { get; set; }
        public bool Obrigatorio { get; set; }
    }
}
