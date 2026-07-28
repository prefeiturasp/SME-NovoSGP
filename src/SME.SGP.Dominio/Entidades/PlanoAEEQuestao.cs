using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class PlanoAEEQuestao : EntidadeBase
    {
        public long PlanoAEEVersaoId { get; set; }
        [Computed]
        public PlanoAEEVersao PlanoAEEVersao { get; set; }

        public long QuestaoId { get; set; }
        [Computed]
        public Questao Questao { get; set; }

        public bool Excluido { get; set; }
    }
}
