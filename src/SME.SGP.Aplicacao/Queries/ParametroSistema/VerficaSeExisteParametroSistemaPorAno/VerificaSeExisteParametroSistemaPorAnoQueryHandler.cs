using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaSeExisteParametroSistemaPorAnoQueryHandler : IRequestHandler<VerificaSeExisteParametroSistemaPorAnoQuery, bool>
    {
        private readonly IRepositorioParametrosSistemaConsulta repositorioParametrosSistema;

        public VerificaSeExisteParametroSistemaPorAnoQueryHandler(IRepositorioParametrosSistemaConsulta repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }

        public Task<bool> Handle(VerificaSeExisteParametroSistemaPorAnoQuery request, CancellationToken cancellationToken)
                    => repositorioParametrosSistema.VerificaSeExisteParametroSistemaPorAno(request.Ano);
    }
}
