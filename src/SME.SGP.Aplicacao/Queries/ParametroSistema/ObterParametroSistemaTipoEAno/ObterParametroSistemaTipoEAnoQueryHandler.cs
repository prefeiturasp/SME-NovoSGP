using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterParametroSistemaTipoEAnoQueryHandler : IRequestHandler<ObterParametroSistemaTipoEAnoQuery, string>
    {
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;

        public ObterParametroSistemaTipoEAnoQueryHandler(IRepositorioParametrosSistema repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }

        public async Task<string> Handle(ObterParametroSistemaTipoEAnoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioParametrosSistema.ObterValorPorTipoEAno(request.Tipo);

        }
    }
}
