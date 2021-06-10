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
                //TODO: TENTAR OTIMIZAR PARA BUSCAR MAIS FÁCIL O BIMESTRE
                var bimestreAtual = await mediator.Send(new ObterBimestrePorTurmaCodigoQuery(aulaParaTratar.TurmaId, aulaParaTratar.DataAula));

                await mediator.Send(new AlterarAulaFrequenciaTratarCommand(aulaParaTratar, filtro.QuantidadeAnterior));
                await mediator.Send(new CalcularFrequenciaPorTurmaCommand(null, aulaParaTratar.DataAula, aulaParaTratar.TurmaId, aulaParaTratar.DisciplinaId, bimestreAtual));
            }

            return true;
        }
    }
}
