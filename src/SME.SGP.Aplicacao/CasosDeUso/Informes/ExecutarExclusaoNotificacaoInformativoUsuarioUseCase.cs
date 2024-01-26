using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarExclusaoNotificacaoInformativoUsuarioUseCase : AbstractUseCase, IExecutarExclusaoNotificacaoInformativoUsuarioUseCase
    {
        public ExecutarExclusaoNotificacaoInformativoUsuarioUseCase(IMediator mediator) : base(mediator)
        { }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var param = mensagem.ObterObjetoMensagem<string>();
            if (string.IsNullOrEmpty(param)) return false;
            var notificacaoId = long.Parse(param);

            return await mediator.Send(new ExcluirNotificacaoPorIdCommand(notificacaoId));
        }
    }
}