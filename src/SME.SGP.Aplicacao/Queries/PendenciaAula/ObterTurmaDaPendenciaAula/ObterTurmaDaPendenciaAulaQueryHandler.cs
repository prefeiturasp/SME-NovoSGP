using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDaPendenciaAulaQueryHandler : IRequestHandler<ObterTurmaDaPendenciaAulaQuery, Turma>
    {
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;

        public ObterTurmaDaPendenciaAulaQueryHandler(IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<Turma> Handle(ObterTurmaDaPendenciaAulaQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaAula.ObterTurmaPorPendencia(request.PendenciaId);
    }
}
