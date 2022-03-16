using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmasPorPeriodoUseCase : AbstractUseCase, IConciliacaoFrequenciaTurmasPorPeriodoUseCase
    {
        public ConciliacaoFrequenciaTurmasPorPeriodoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var dto = mensagem.ObterObjetoMensagem<ConciliacaoFrequenciaTurmaPorPeriodoDto>();

            foreach (var turma in  dto.TurmasDaModalidade)
                await mediator.Send(new IncluirFilaConciliacaoFrequenciaTurmaCommand(turma, dto.Bimestre, dto.ComponenteCurricularId, dto.DataInicio, dto.DataFim));

            return true;
        }
    }
}
