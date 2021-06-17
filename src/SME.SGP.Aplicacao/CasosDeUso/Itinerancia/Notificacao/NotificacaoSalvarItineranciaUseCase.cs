using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
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

            var ue = await mediator.Send(new ObterUePorIdQuery(mensagemRabbit.UeId));
            if (ue == null)
                throw new NegocioException("Não foi possível localizar um Unidade Escolar!");

                await mediator.Send(new NotificacaoSalvarItineranciaAlunosCommand(ue.CodigoUe, mensagemRabbit.CriadoRF, mensagemRabbit.CriadoPor, mensagemRabbit.DataVisita, mensagemRabbit.Estudantes, mensagemRabbit.ItineranciaId));

            return true;
        }
    }
}
