using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAEEQuestaoCommand : IRequest<long>
    {
        public long PlanoId { get; set; }
        public long PlanoVersaoId { get; set; }
        public long QuestaoId { get; set; }

        public SalvarPlanoAEEQuestaoCommand(long planoId, long questaoId, long planoVersaoId)
        {
            PlanoId = planoId;
            QuestaoId = questaoId;
            PlanoVersaoId = planoVersaoId;
        }
    }
}
