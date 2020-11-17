using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDaPendenciaQueryHandler : IRequestHandler<ObterTurmaDaPendenciaQuery, Turma>
    {
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;

        public ObterTurmaDaPendenciaQueryHandler(IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<Turma> Handle(ObterTurmaDaPendenciaQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaAula.ObterTurmaPorPendencia(request.PendenciaId);
    }
}
