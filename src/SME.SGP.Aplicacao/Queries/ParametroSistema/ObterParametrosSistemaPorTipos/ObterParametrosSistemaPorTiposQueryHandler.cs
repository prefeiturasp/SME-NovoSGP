using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterParametrosSistemaPorTiposQueryHandler : IRequestHandler<ObterParametrosSistemaPorTiposQuery, IEnumerable<ParametrosSistema>>
    {
        private readonly IRepositorioParametrosSistemaConsulta repositorioParametrosSistema;

        public ObterParametrosSistemaPorTiposQueryHandler(IRepositorioParametrosSistemaConsulta repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }
        public async Task<IEnumerable<ParametrosSistema>> Handle(ObterParametrosSistemaPorTiposQuery request, CancellationToken cancellationToken)
        {
            return await repositorioParametrosSistema.ObterPorTiposAsync(request.Tipos);
        }
    }
}
