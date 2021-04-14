using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
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
                await mediator.Send(new NotificacaoSalvarItineranciaAlunosCommand(mensagemRabbit.Ues.FirstOrDefault().CodigoUe, mensagemRabbit.CriadoRF, mensagemRabbit.CriadoPor, mensagemRabbit.DataVisita, mensagemRabbit.Estudantes, mensagemRabbit.ItineranciaId));
            }
            else
            {
                foreach (var ue in mensagemRabbit.Ues)
                {
                   await mediator.Send(new NotificacaoSalvarItineranciaSemAlunosVinculadosCommand(ue.CodigoUe, mensagemRabbit.CriadoRF, mensagemRabbit.CriadoPor, mensagemRabbit.DataVisita));
                }
            }

            return true;
        }
    }
}
