using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoDevolutivaUseCase : IExcluirNotificacaoDevolutivaUseCase
    {
        private readonly IMediator mediator;
        
        public ExcluirNotificacaoDevolutivaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosMensagem = mensagemRabbit.ObterObjetoMensagem<ExcluirNotificacaoDevolutivaDto>();
            
            return await mediator.Send(new ExcluirNotificacaoDevolutivaCommand(dadosMensagem.DevolutivaId));
        }
    }
}
