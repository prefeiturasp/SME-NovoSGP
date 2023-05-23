using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosSemNotasRecomendacoesUseCase : IObterAlunosSemNotasRecomendacoesUseCase
    {
        private IMediator mediator;

        public ObterAlunosSemNotasRecomendacoesUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<InconsistenciasAlunoFamiliaDto>> Executar(FiltroInconsistenciasAlunoFamiliaDto param)
        {
            var retorno = new List<InconsistenciasAlunoFamiliaDto>();
            var turmaRegular = await mediator.Send(new ObterTurmaPorIdQuery(param.TurmaId));
            var alunosDaTurma = await mediator.Send(new ObterTodosAlunosNaTurmaQuery(int.Parse(turmaRegular.CodigoTurma)));
            var turmaComplementares = await mediator.Send(new ObterTurmasComplementaresPorAlunoQuery(alunosDaTurma.Select(x => x.CodigoAluno).ToArray()));
            var turmaItinerario = await mediator.Send(new ObterTurmaItinerarioEnsinoMedioQuery());
            return retorno;
        }
    }
}