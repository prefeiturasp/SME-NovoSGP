using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.EscolaAqui.Anos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosPorCodigoUeModalidadeHandler : IRequestHandler<ObterAnosPorCodigoUeModalidadeQuery, IEnumerable<AnosPorCodigoUeModalidadeEscolaAquiResult>>
    {
        private readonly IRepositorioAnoEscolar _repositorioAnoEscolar;

        public ObterAnosPorCodigoUeModalidadeHandler(IRepositorioAnoEscolar repositorioAnoEscolar)
        {
            this._repositorioAnoEscolar = repositorioAnoEscolar ?? throw new ArgumentNullException(nameof(repositorioAnoEscolar));
        }

        public async Task<IEnumerable<AnosPorCodigoUeModalidadeEscolaAquiResult>> Handle(ObterAnosPorCodigoUeModalidadeQuery request, CancellationToken cancellationToken)
        {
            return await _repositorioAnoEscolar.ObterAnosPorCodigoUeModalidade(request.CodigoUe, request.Modalidade);
        }
    }
}
