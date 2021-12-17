using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterParametroSistemaPorTipoQueryHandler : IRequestHandler<ObterParametroSistemaPorTipoQuery, string>
    {
        private readonly IRepositorioParametrosSistemaConsulta repositorioParametrosSistema;

        public ObterParametroSistemaPorTipoQueryHandler(IRepositorioParametrosSistemaConsulta repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }
        public async Task<string> Handle(ObterParametroSistemaPorTipoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioParametrosSistema.ObterValorUnicoPorTipo(request.Tipo);
        }
    }
}
