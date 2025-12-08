using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarInatividadeDoAtendimentoNAAPAPorUeUseCase : AbstractUseCase, INotificarInatividadeDoAtendimentoNAAPAPorUeUseCase
    {
        public NotificarInatividadeDoAtendimentoNAAPAPorUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            long.TryParse(param.Mensagem.ToString(), out long ueId);

            if (ueId > 0)
            {
                var informacoesInatividadeAtendimento = await mediator.Send(new ObterAtendimentosNAAPAComInatividadeDeAtendimentoQuery(ueId));

                foreach (var informacao in informacoesInatividadeAtendimento)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarNotificacaoInatividadeAtendimentoInformacaoNAAPA, informacao, Guid.NewGuid()));
            }

            return true;
        }
    }
}
