using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterParametroSistemaPorTipoEAnoQueryHandler : IRequestHandler<ObterParametroSistemaPorTipoEAnoQuery, ParametrosSistema>
    {
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;

        public ObterParametroSistemaPorTipoEAnoQueryHandler(IRepositorioParametrosSistema repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }

        public async Task<ParametrosSistema> Handle(ObterParametroSistemaPorTipoEAnoQuery request, CancellationToken cancellationToken)
            => await repositorioParametrosSistema.ObterParametroPorTipoEAno(request.TipoParametroSistema, request.Ano);
    }
}
