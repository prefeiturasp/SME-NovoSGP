using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.AreaDoConhecimento;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.AreaDoConhecimento.ObterOrdenacaoAreasConhecimento
{
    public class ObterOrdenacaoAreasConhecimentoQueryHandler : IRequestHandler<ObterOrdenacaoAreasConhecimentoQuery, IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>>
    {
        private readonly IObterOrdenacaoAreasConhecimentoUseCase obterOrdenacaoAreasConhecimentoUseCase;

        public ObterOrdenacaoAreasConhecimentoQueryHandler(IObterOrdenacaoAreasConhecimentoUseCase obterOrdenacaoAreasConhecimentoUseCase)
        {
            this.obterOrdenacaoAreasConhecimentoUseCase = obterOrdenacaoAreasConhecimentoUseCase ?? throw new ArgumentNullException(nameof(obterOrdenacaoAreasConhecimentoUseCase));
        }

        public async Task<IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>> Handle(ObterOrdenacaoAreasConhecimentoQuery request, CancellationToken cancellationToken)
        {
            return await obterOrdenacaoAreasConhecimentoUseCase
                .Executar((request.ComponentesCurricularesTurma, request.AreasConhecimento));
        }
    }
}
