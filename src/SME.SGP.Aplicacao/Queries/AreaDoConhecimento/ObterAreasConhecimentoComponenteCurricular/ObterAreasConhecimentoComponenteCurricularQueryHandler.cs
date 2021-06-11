using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAreasConhecimentoComponenteCurricularQueryHandler : IRequestHandler<ObterAreasConhecimentoComponenteCurricularQuery, IEnumerable<AreaDoConhecimentoDto>>
    {
        private readonly IRepositorioAreaDoConhecimento repositorioAreaDoConhecimento;

        public ObterAreasConhecimentoComponenteCurricularQueryHandler(IRepositorioAreaDoConhecimento repositorioAreaDoConhecimento)
        {
            this.repositorioAreaDoConhecimento = repositorioAreaDoConhecimento ?? throw new ArgumentNullException(nameof(repositorioAreaDoConhecimento));
        }

        public async Task<IEnumerable<AreaDoConhecimentoDto>> Handle(ObterAreasConhecimentoComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            var areasDoConhecimento = await repositorioAreaDoConhecimento.ObterAreasDoConhecimentoPorComponentesCurriculares(request.CodigosComponenteCurricular);

            if (areasDoConhecimento == null || !areasDoConhecimento.Any())
                throw new NegocioException("Não foi possível obter as áreas de conhecimento dos componentes informados");

            return areasDoConhecimento;
        }
    }
}
