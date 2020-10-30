using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterParametroSistemaPorTipoQueryHandler : IRequestHandler<ObterParametroSistemaPorTipoQuery, string>
    {
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;

        public ObterParametroSistemaPorTipoQueryHandler(IRepositorioParametrosSistema repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }
        public async Task<string> Handle(ObterParametroSistemaPorTipoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioParametrosSistema.ObterValorUnicoPorTipo(request.Tipo);
        }
    }
}
