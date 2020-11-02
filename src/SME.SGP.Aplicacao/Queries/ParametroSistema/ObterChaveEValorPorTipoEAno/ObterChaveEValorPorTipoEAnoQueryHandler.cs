using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterChaveEValorPorTipoEAnoQueryHandler : IRequestHandler<ObterChaveEValorPorTipoEAnoQuery, IEnumerable<KeyValuePair<string, string>>>
    {
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;

        public ObterChaveEValorPorTipoEAnoQueryHandler(IRepositorioParametrosSistema repositorioParametrosSistema)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }

        public Task<IEnumerable<KeyValuePair<string, string>>> Handle(ObterChaveEValorPorTipoEAnoQuery request, CancellationToken cancellationToken)
                        => repositorioParametrosSistema.ObterChaveEValorPorTipoEAno(request.Tipo, request.Ano);
    }
}
