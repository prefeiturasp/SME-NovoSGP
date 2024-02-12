using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EncerrarEncaminhamentoAEEAutomaticoSyncUseCase : AbstractUseCase, IEncerrarEncaminhamentoAEEAutomaticoSyncUseCase
    {
        public EncerrarEncaminhamentoAEEAutomaticoSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var encaminhamentosVigentes = await mediator.Send(new ObterEncaminhamentoAEEVigenteQuery());

            if (!encaminhamentosVigentes.Any())
                return false;

            foreach (var encaminhamento in encaminhamentosVigentes)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAEE.RotaValidarEncerrarEncaminhamentoAEEAutomatico,
                    new FiltroValidarEncerrarEncaminhamentoAEEAutomaticoDto(encaminhamento.EncaminhamentoId, encaminhamento.UeCodigo,
                    encaminhamento.AlunoCodigo, encaminhamento.AnoLetivo)));
            }

            return true;
        }
    }
}
