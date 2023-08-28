using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarRelatorioPeriodicoQuestaoPAPCommandHandler : IRequestHandler<AlterarRelatorioPeriodicoQuestaoPAPCommand, long>
    {
        private readonly IRepositorioRelatorioPeriodicoPAPQuestao repositorio;

        public AlterarRelatorioPeriodicoQuestaoPAPCommandHandler(IRepositorioRelatorioPeriodicoPAPQuestao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<long> Handle(AlterarRelatorioPeriodicoQuestaoPAPCommand request, CancellationToken cancellationToken)
        {
            return repositorio.SalvarAsync(request.RelatorioPeriodicoQuestao);
        }
    }
}
