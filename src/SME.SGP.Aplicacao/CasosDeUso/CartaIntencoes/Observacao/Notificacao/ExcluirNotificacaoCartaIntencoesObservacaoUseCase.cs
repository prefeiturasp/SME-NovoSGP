using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoCartaIntencoesObservacaoUseCase : IExcluirNotificacaoCartaIntencoesObservacaoUseCase
    {
        private readonly IMediator mediator;
        
        public ExcluirNotificacaoCartaIntencoesObservacaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosMensagem = mensagemRabbit.ObterObjetoMensagem<ExcluirNotificacaoCartaIntencoesObservacaoDto>();
            
            return await mediator.Send(new ExcluirNotificacaoCartaIntencoesObservacaoCommand(dadosMensagem.CartaIntencoesObservacaoId));
        }
    }
}
