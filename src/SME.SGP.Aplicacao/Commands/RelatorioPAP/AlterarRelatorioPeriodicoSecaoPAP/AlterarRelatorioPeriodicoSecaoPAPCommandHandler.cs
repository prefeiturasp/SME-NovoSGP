using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarRelatorioPeriodicoSecaoPAPCommandHandler : IRequestHandler<AlterarRelatorioPeriodicoSecaoPAPCommand, long>
    {
        private readonly IRepositorioRelatorioPeriodicoPAPSecao repositorio;

        public AlterarRelatorioPeriodicoSecaoPAPCommandHandler(IRepositorioRelatorioPeriodicoPAPSecao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<long> Handle(AlterarRelatorioPeriodicoSecaoPAPCommand request, CancellationToken cancellationToken)
        {
            return repositorio.SalvarAsync(request.RelatorioPeriodicoSecao);
        }
    }
}
