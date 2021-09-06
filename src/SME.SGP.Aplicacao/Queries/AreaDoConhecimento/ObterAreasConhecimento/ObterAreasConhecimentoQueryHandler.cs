using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.AreaDoConhecimento;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.AreaDoConhecimento.ObterAreasConhecimento
{
    public class ObterAreasConhecimentoQueryHandler : IRequestHandler<ObterAreasConhecimentoQuery, IEnumerable<AreaDoConhecimentoDto>>
    {
        private readonly IObterAreasConhecimentoUseCase obterAreasConhecimentoUseCase;

        public ObterAreasConhecimentoQueryHandler(IObterAreasConhecimentoUseCase obterAreasConhecimentoUseCase)
        {
            this.obterAreasConhecimentoUseCase = obterAreasConhecimentoUseCase ?? throw new ArgumentNullException(nameof(obterAreasConhecimentoUseCase));
        }

        public async Task<IEnumerable<AreaDoConhecimentoDto>> Handle(ObterAreasConhecimentoQuery request, CancellationToken cancellationToken)
        {
            return await obterAreasConhecimentoUseCase.Executar(request.ComponentesCurriculares);
        }
    }
}
