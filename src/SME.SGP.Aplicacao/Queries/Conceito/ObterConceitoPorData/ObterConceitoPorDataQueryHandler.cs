using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterConceitoPorDataQueryHandler :IRequestHandler<ObterConceitoPorDataQuery,IEnumerable<Conceito>>
    {
        private readonly IRepositorioConceitoConsulta repositorioConceito;

        public ObterConceitoPorDataQueryHandler(IRepositorioConceitoConsulta conceito)
        {
            repositorioConceito = conceito ?? throw new ArgumentNullException(nameof(conceito));
        }

        public async Task<IEnumerable<Conceito>> Handle(ObterConceitoPorDataQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConceito.ObterPorData(request.DataAvaliacao);
        }
    }
}