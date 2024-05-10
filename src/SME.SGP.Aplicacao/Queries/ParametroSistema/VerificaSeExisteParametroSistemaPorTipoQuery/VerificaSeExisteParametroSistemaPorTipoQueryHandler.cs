using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class VerificaSeExisteParametroSistemaPorTipoQueryHandler : IRequestHandler<VerificaSeExisteParametroSistemaPorTipoQuery,bool>
    {
        private readonly IRepositorioParametrosSistemaConsulta repositorioParametrosSistema;

        public VerificaSeExisteParametroSistemaPorTipoQueryHandler(IRepositorioParametrosSistemaConsulta repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }

        public async Task<bool> Handle(VerificaSeExisteParametroSistemaPorTipoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioParametrosSistema.VerificaSeExisteParametroSistemaPorTipo(request.Tipo);
        }
    }
}