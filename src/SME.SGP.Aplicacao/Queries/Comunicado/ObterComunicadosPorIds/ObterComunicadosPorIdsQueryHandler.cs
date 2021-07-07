using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosPorIdsQueryHandler : IRequestHandler<ObterComunicadosPorIdsQuery, IEnumerable<Comunicado>>
    {
        private readonly IRepositorioComunicado repositorioComunicado;

        public ObterComunicadosPorIdsQueryHandler(IRepositorioComunicado repositorioComunicado)
        {
            this.repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
        }

        public async Task<IEnumerable<Comunicado>> Handle(ObterComunicadosPorIdsQuery request, CancellationToken cancellationToken)
            => await repositorioComunicado.ObterComunicadosPorIds(request.Ids);
    }
}
