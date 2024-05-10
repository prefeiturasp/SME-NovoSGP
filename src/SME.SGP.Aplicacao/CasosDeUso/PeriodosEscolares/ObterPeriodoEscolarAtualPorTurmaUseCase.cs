using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarAtualPorTurmaUseCase : AbstractUseCase, IObterPeriodoEscolarAtualPorTurmaUseCase
    {
        public ObterPeriodoEscolarAtualPorTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PeriodoDto> Executar(long turmaId, DateTime dataReferencia)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(turmaId));
            var periodo = await mediator.Send(new ObterPeriodoEscolarAtualPorTurmaQuery(turma, dataReferencia));
            return MontarDto(periodo);
        }

        private PeriodoDto MontarDto(PeriodoEscolar periodo)
        {
            return new PeriodoDto(periodo.PeriodoInicio, periodo.PeriodoFim);
        }
    }
}
