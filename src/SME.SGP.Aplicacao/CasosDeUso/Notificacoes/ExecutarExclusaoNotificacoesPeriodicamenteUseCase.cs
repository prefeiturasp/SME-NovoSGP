using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarExclusaoNotificacoesPeriodicamenteUseCase : AbstractUseCase, IExecutarExclusaoNotificacoesPeriodicamenteUseCase
    {
        public ExecutarExclusaoNotificacoesPeriodicamenteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var idsNotificacoes = await mediator.Send(new ObterIdsDasNotificacoesParaExclusaoPorDiasQuery());

            foreach (var id in idsNotificacoes)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutarExclusaoDaNotificacao, id, Guid.NewGuid()));

            return true;
        }
    }
}
