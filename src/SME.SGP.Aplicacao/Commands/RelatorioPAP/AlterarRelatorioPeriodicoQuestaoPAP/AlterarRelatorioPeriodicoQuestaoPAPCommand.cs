using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AlterarRelatorioPeriodicoQuestaoPAPCommand : IRequest<long>
    {
        public AlterarRelatorioPeriodicoQuestaoPAPCommand(RelatorioPeriodicoPAPQuestao relatorioPeriodicoQuestao)
        {
            RelatorioPeriodicoQuestao = relatorioPeriodicoQuestao;
        }

        public RelatorioPeriodicoPAPQuestao RelatorioPeriodicoQuestao { get; set; }
    }
}
