﻿namespace SME.SGP.Dominio
{
    public class ItineranciaQuestao : EntidadeBase
    {        
        public long QuestaoId { get; set; }
        public string Resposta { get; set; }
        public bool Excluido { get; set; }
        public long ItineranciaId { get; set; }
    }
}