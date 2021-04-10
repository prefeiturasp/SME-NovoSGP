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
    public class NotificacaoSalvarItineranciaUseCase : AbstractUseCase, INotificacaoSalvarItineranciaUseCase
    {
        public NotificacaoSalvarItineranciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var mensagemRabbit = JsonConvert.DeserializeObject<NotificacaoSalvarItineranciaDto>(param.Mensagem.ToString());

            if (mensagemRabbit.Estudantes.Any())
            {
                await mediator.Send(new NotificacaoSalvarItineranciaAlunosCommand(mensagemRabbit.Ues.FirstOrDefault().CodigoUe, mensagemRabbit.CriadoRF, mensagemRabbit.CriadoPor,  mensagemRabbit.DataVisita, mensagemRabbit.Estudantes));
            }

            return true;
        }
    }
}
