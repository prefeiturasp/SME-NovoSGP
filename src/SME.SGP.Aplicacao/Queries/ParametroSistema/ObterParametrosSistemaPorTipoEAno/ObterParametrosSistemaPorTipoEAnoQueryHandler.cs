using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterParametrosSistemaPorTipoEAnoQueryHandler : IRequestHandler<ObterParametrosSistemaPorTipoEAnoQuery, IEnumerable<ParametrosSistema>>
    {
        private readonly IRepositorioParametrosSistemaConsulta repositorioParametrosSistema;

        public ObterParametrosSistemaPorTipoEAnoQueryHandler(IRepositorioParametrosSistemaConsulta repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }

        public async Task<IEnumerable<ParametrosSistema>> Handle(ObterParametrosSistemaPorTipoEAnoQuery request, CancellationToken cancellationToken)
                => await repositorioParametrosSistema.ObterParametrosPorTipoEAno(request.TipoParametroSistema, request.Ano);
    }
}
