using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.AreaDoConhecimento;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.AreaDoConhecimento.ObterComponentesAreasConhecimento
{
    public class ObterComponentesAreasConhecimentoQueryHandler : IRequestHandler<ObterComponentesAreasConhecimentoQuery, IEnumerable<DisciplinaDto>>
    {
        private readonly IObterComponentesDasAreasDeConhecimentoUseCase obterComponentesDasAreasDeConhecimentoUseCase;

        public ObterComponentesAreasConhecimentoQueryHandler(IObterComponentesDasAreasDeConhecimentoUseCase obterComponentesDasAreasDeConhecimentoUseCase)
        {
            this.obterComponentesDasAreasDeConhecimentoUseCase = obterComponentesDasAreasDeConhecimentoUseCase ?? throw new ArgumentNullException(nameof(obterComponentesDasAreasDeConhecimentoUseCase));
        }

        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesAreasConhecimentoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(obterComponentesDasAreasDeConhecimentoUseCase
                .ObterComponentesDasAreasDeConhecimento(request.ComponentesCurricularesTurma, request.AreasConhecimento));
        }
    }
}
