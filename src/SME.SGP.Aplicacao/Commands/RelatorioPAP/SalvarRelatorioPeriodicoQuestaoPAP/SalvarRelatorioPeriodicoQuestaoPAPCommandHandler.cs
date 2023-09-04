using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarRelatorioPeriodicoQuestaoPAPCommandHandler : IRequestHandler<SalvarRelatorioPeriodicoQuestaoPAPCommand, RelatorioPeriodicoPAPQuestao>
    {
        private readonly IRepositorioRelatorioPeriodicoPAPQuestao repositorio;

        public SalvarRelatorioPeriodicoQuestaoPAPCommandHandler(IRepositorioRelatorioPeriodicoPAPQuestao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<RelatorioPeriodicoPAPQuestao> Handle(SalvarRelatorioPeriodicoQuestaoPAPCommand request, CancellationToken cancellationToken)
        {
            var relatorioQuestao = new RelatorioPeriodicoPAPQuestao()
            {
                QuestaoId = request.QuestaoId,
                RelatorioPeriodiocoSecaoId = request.RelatorioPeriodiocoSecaoId,
            };

            await repositorio.SalvarAsync(relatorioQuestao);

            return relatorioQuestao;
        }
    }
}
