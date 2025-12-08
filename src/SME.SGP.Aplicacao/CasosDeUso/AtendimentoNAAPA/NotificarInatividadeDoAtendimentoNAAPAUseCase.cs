using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarInatividadeDoAtendimentoNAAPAUseCase : AbstractUseCase, INotificarInatividadeDoAtendimentoNAAPAUseCase
    {
        public NotificarInatividadeDoAtendimentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var listaUes = await mediator.Send(ObterTodasUesIdsQuery.Instance);

            foreach (var ueId in listaUes)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarNotificacaoInatividadeAtendimentoPorUeNAAPA, ueId, Guid.NewGuid()));

            return true;
        }
    }
}
