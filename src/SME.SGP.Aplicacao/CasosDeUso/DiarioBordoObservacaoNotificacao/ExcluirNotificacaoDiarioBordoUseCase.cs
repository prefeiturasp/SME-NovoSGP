using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoDiarioBordoUseCase : IExcluirNotificacaoDiarioBordoUseCase
    {
        private readonly IMediator mediator;
        public ExcluirNotificacaoDiarioBordoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosMensagem = mensagemRabbit.ObterObjetoMensagem<ExcluirNotificacaoDiarioBordoDto>();

            return await mediator.Send(new ExcluirNotificacaoDiarioBordoCommand(dadosMensagem.ObservacaoId));
        }
    }
}
