using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEventoTipoIdPorCodigoQueryHandler : IRequestHandler<ObterEventoTipoIdPorCodigoQuery, long>
    {
        private readonly IRepositorioEventoTipo repositorioEventoTipo;

        public ObterEventoTipoIdPorCodigoQueryHandler(IRepositorioEventoTipo repositorioEventoTipo)
        {
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new ArgumentNullException(nameof(repositorioEventoTipo));
        }

        public async Task<long> Handle(ObterEventoTipoIdPorCodigoQuery request, CancellationToken cancellationToken)
            => await repositorioEventoTipo.ObterIdPorCodigo((int)request.TipoEvento);
    }
}
