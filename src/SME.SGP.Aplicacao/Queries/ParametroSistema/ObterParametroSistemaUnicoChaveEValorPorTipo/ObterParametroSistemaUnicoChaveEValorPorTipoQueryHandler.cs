using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterParametroSistemaUnicoChaveEValorPorTipoQueryHandler : IRequestHandler<ObterParametroSistemaUnicoChaveEValorPorTipoQuery, KeyValuePair<string, string>?>
    {
        private readonly IRepositorioParametrosSistemaConsulta repositorioParametrosSistema;

        public ObterParametroSistemaUnicoChaveEValorPorTipoQueryHandler(IRepositorioParametrosSistemaConsulta repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }
        public async Task<KeyValuePair<string, string>?> Handle(ObterParametroSistemaUnicoChaveEValorPorTipoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioParametrosSistema.ObterUnicoChaveEValorPorTipo(request.Tipo);
        }
    }
}