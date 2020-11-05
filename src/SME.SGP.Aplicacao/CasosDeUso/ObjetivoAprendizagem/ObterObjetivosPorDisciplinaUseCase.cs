using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObjetivosPorDisciplinaUseCase : IObterObjetivosPorDisciplinaUseCase
    {
        private readonly IMediator mediator;

        public ObterObjetivosPorDisciplinaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ObjetivosAprendizagemPorComponenteDto>> Executar(DateTime dataReferencia, long turmaId, long componenteCurricularId, long disciplinaId, bool regencia = false)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var turma = await mediator.Send(new ObterTurmaPorIdQuery(turmaId));

            var bimestre = await mediator.Send(new ObterBimestreAtualPorTurmaIdQuery(turma, dataReferencia));

            var filtrarSomenteRegencia = regencia && !usuarioLogado.EhProfessorCj();

            return await mediator.Send(new ObterObjetivosPlanoDisciplinaQuery(bimestre,
                                                                              turmaId,
                                                                              componenteCurricularId,
                                                                              disciplinaId,
                                                                              filtrarSomenteRegencia));

        }
    }
}
