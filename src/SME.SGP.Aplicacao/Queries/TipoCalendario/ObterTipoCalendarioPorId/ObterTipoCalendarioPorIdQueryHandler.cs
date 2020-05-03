using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioPorIdQueryHandler : IRequestHandler<ObterTipoCalendarioPorIdQuery, TipoCalendario>
    {
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ObterTipoCalendarioPorIdQueryHandler(IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }
        public async Task<TipoCalendario> Handle(ObterTipoCalendarioPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTipoCalendario.ObterPorIdAsync(request.Id);
        }
    }
}
