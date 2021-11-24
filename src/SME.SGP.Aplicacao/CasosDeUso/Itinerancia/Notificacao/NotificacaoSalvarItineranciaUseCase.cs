using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoSalvarItineranciaUseCase : AbstractUseCase, INotificacaoSalvarItineranciaUseCase
    {
        private readonly IRepositorioTesteLog repositorioTesteLog;

        public NotificacaoSalvarItineranciaUseCase(IMediator mediator, IRepositorioTesteLog repositorioTesteLog) : base(mediator)
        {
            this.repositorioTesteLog = repositorioTesteLog ?? throw new ArgumentNullException(nameof(repositorioTesteLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var notificacao = JsonConvert.DeserializeObject<NotificacaoSalvarItineranciaDto>(mensagemRabbit.Mensagem.ToString());

                if (notificacao == null)
                    throw new NegocioException("Não foi possível obter os dados do registro de itinerância, para criar a notificação.");

                await mediator.Send(new NotificacaoSalvarItineranciaAlunosCommand(notificacao.UeId, notificacao.CriadoRF, notificacao.CriadoPor, notificacao.DataVisita, notificacao.Estudantes, notificacao.ItineranciaId));

                return true;
            }
            catch (Exception ex)
            {
                await repositorioTesteLog.Gravar(ex.ToString());
                throw;
            }
        }
    }
}
