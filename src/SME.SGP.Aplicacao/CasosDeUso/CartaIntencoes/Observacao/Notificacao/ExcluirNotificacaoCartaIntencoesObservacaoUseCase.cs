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
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IRepositorioNotificacaoCartaIntencoesObservacao repositorioNotificacaoCartaIntencoesObservacao;

        public ExcluirNotificacaoCartaIntencoesObservacaoUseCase(IMediator mediator, IRepositorioNotificacao repositorioNotificacao,
            IRepositorioNotificacaoCartaIntencoesObservacao repositorioNotificacaoCartaIntencoesObservacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
            this.repositorioNotificacaoCartaIntencoesObservacao = repositorioNotificacaoCartaIntencoesObservacao ?? throw new ArgumentNullException(nameof(repositorioNotificacaoCartaIntencoesObservacao));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosMensagem = mensagemRabbit.ObterObjetoMensagem<ExcluirNotificacaoCartaIntencoesObservacaoDto>();
            
            return await mediator.Send(new ExcluirNotificacaoCartaIntencoesObservacaoCommand(dadosMensagem.CartaIntencoesObservacaoId));
        }
    }
}
