using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterJuremaIdsPorComponentesCurricularIdQueryHandler : IRequestHandler<ObterJuremaIdsPorComponentesCurricularIdQuery, long[]>
    {
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        public ObterJuremaIdsPorComponentesCurricularIdQueryHandler(IRepositorioComponenteCurricular repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new System.ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<long[]> Handle(ObterJuremaIdsPorComponentesCurricularIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioComponenteCurricular.ListarCodigosJuremaPorComponenteCurricularId(request.Id);
        }
    }
}
