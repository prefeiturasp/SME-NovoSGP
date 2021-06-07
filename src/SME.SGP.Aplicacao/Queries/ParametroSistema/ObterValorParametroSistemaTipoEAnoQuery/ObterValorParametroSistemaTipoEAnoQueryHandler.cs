using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterValorParametroSistemaTipoEAnoQueryHandler : IRequestHandler<ObterValorParametroSistemaTipoEAnoQuery, string>
    {
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;

        public ObterValorParametroSistemaTipoEAnoQueryHandler(IRepositorioParametrosSistema repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }

        public async Task<string> Handle(ObterValorParametroSistemaTipoEAnoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioParametrosSistema.ObterValorPorTipoEAno(request.Tipo, request.Ano);
        }
    }
}
