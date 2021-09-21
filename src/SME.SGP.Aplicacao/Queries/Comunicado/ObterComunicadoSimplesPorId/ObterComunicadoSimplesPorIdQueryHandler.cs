using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadoSimplesPorIdQueryHandler : IRequestHandler<ObterComunicadoSimplesPorIdQuery, Comunicado>
    {
        private readonly IRepositorioComunicado repositorioComunicado;

        public ObterComunicadoSimplesPorIdQueryHandler(IRepositorioComunicado repositorioComunicado)
        {
            this.repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
        }

        public async Task<Comunicado> Handle(ObterComunicadoSimplesPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioComunicado.ObterPorIdAsync(request.Id);
    }
}
