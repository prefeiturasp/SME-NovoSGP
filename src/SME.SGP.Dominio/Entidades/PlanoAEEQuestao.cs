﻿namespace SME.SGP.Dominio
{
    public class PlanoAEEQuestao : EntidadeBase
    {
        public long PlanoAEEVersaoId { get; set; }
        public PlanoAEEVersao PlanoAEEVersao { get; set; }

        public long QuestaoId { get; set; }
        public Questao Questao { get; set; }

        public bool Excluido { get; set; }
    }
}
