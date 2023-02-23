using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaFechamentoCommandHandler : IRequestHandler<SalvarPendenciaFechamentoCommand, long>
    {
        private readonly IRepositorioPendenciaFechamento repositorioPendenciaFechamento;

        public SalvarPendenciaFechamentoCommandHandler(IRepositorioPendenciaFechamento repositorioPendenciaFechamento)
        {
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
        }

        public async Task<long> Handle(SalvarPendenciaFechamentoCommand request, CancellationToken cancellationToken)
        {
            var pendenciaFechamentoId = await repositorioPendenciaFechamento.SalvarAsync(new PendenciaFechamento(request.FechamentoTurmaDisciplinaId,
                                                                                                                 request.PendenciaId));
            return pendenciaFechamentoId;
        }
    }
}
