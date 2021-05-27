using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarAulaFrequenciaTratarUseCase : AbstractUseCase, IAlterarAulaFrequenciaTratarUseCase
    {
        public AlterarAulaFrequenciaTratarUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<AulaAlterarFrequenciaRequestDto>();

            var aulaParaTratar = await mediator.Send(new ObterAulaPorIdQuery(filtro.AulaId));
            if (aulaParaTratar != null)
            {
                await mediator.Send(new AlterarAulaFrequenciaTratarCommand(aulaParaTratar, filtro.QuantidadeAnterior));
                await mediator.Send(new ValidaAusenciaParaConciliacaoFrequenciaTurmaCommand(aulaParaTratar.TurmaId, aulaParaTratar.DataAula, aulaParaTratar.DisciplinaId));
            }

            return true;
        }
    }
}
