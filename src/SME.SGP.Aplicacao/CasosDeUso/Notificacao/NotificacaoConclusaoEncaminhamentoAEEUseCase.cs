using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoConclusaoEncaminhamentoAEEUseCase : AbstractUseCase, INotificacaoConclusaoEncaminhamentoAEEUseCase
    {
        public NotificacaoConclusaoEncaminhamentoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var mensagemRabbit = JsonConvert.DeserializeObject<NotificacaoEncaminhamentoAEEDto>(param.Mensagem.ToString());

            if (mensagemRabbit.EncaminhamentoAEEId > 0)
            {
                await mediator.Send(new NotificacaoConclusaoEncaminhamentoAEECommand(mensagemRabbit.EncaminhamentoAEEId, mensagemRabbit.UsuarioRF, mensagemRabbit.UsuarioNome));
            }

            return true;
        }
    }
}
