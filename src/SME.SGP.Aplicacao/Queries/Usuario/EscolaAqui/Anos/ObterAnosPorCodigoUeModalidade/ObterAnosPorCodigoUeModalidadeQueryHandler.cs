using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.EscolaAqui.Anos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosPorCodigoUeModalidadeQueryHandler : IRequestHandler<ObterAnosPorCodigoUeModalidadeQuery, IEnumerable<AnosPorCodigoUeModalidadeEscolaAquiResult>>
    {
        private readonly IRepositorioAnoEscolar repositorioAnoEscolar;

        public ObterAnosPorCodigoUeModalidadeQueryHandler(IRepositorioAnoEscolar repositorioAnoEscolar)
        {
            this.repositorioAnoEscolar = repositorioAnoEscolar ?? throw new ArgumentNullException(nameof(repositorioAnoEscolar));
        }

        public async Task<IEnumerable<AnosPorCodigoUeModalidadeEscolaAquiResult>> Handle(ObterAnosPorCodigoUeModalidadeQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAnoEscolar.ObterAnosPorCodigoUeModalidade(request.CodigoUe, request.Modalidades);
        }
    }
}
