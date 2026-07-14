using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarFechamentoConsolidadoCommandHandler : IRequestHandler<SalvarFechamentoConsolidadoCommand, long>
    {
        private readonly IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado;

        public SalvarFechamentoConsolidadoCommandHandler(IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado)
        {
            this.repositorioFechamentoConsolidado = repositorioFechamentoConsolidado ?? throw new ArgumentNullException(nameof(repositorioFechamentoConsolidado));
        }

        public async Task<long> Handle(SalvarFechamentoConsolidadoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoConsolidado.SalvarAsync(request.FechamentoConsolidadoComponenteTurma);
        }
    }
}
