using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDaPendenciaFechamentoQueryHandler : IRequestHandler<ObterTurmaDaPendenciaFechamentoQuery, Turma>
    {
        private readonly IRepositorioPendenciaFechamento repositorioPendenciaFechamento;

        public ObterTurmaDaPendenciaFechamentoQueryHandler(IRepositorioPendenciaFechamento repositorioPendenciaFechamento)
        {
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
        }

        public async Task<Turma> Handle(ObterTurmaDaPendenciaFechamentoQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaFechamento.ObterTurmaPorPendenciaId(request.PendenciaId);
    }
}
