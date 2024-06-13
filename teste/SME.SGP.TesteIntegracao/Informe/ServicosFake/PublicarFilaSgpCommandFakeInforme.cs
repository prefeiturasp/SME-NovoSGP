using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.Informe.ServicosFake
{
    public class PublicarFilaSgpCommandFakeInforme : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IExecutarExclusaoNotificacaoInformativoUsuariosUseCase executarExclusaoNotificacaoInformativoUsuariosUseCase;
        private readonly IExecutarExclusaoNotificacaoInformativoUsuarioUseCase executarExclusaoNotificacaoInformativoUsuarioUseCase;
        private readonly IExecutarNotificacaoInformativoUsuariosUseCase executarNotificacaoInformativoUsuariosUseCase;
        private readonly IExecutarNotificacaoInformativoUsuarioUseCase executarNotificacaoInformativoUsuarioUseCase;

        public PublicarFilaSgpCommandFakeInforme(IExecutarExclusaoNotificacaoInformativoUsuariosUseCase executarExclusaoNotificacaoInformativoUsuariosUseCase,
                                                 IExecutarExclusaoNotificacaoInformativoUsuarioUseCase executarExclusaoNotificacaoInformativoUsuarioUseCase,
                                                 IExecutarNotificacaoInformativoUsuariosUseCase executarNotificacaoInformativoUsuariosUseCase,
                                                 IExecutarNotificacaoInformativoUsuarioUseCase executarNotificacaoInformativoUsuarioUseCase)
        {
            this.executarExclusaoNotificacaoInformativoUsuariosUseCase = executarExclusaoNotificacaoInformativoUsuariosUseCase ?? throw new ArgumentNullException(nameof(executarExclusaoNotificacaoInformativoUsuariosUseCase));
            this.executarExclusaoNotificacaoInformativoUsuarioUseCase = executarExclusaoNotificacaoInformativoUsuarioUseCase ?? throw new ArgumentNullException(nameof(executarExclusaoNotificacaoInformativoUsuarioUseCase));
            this.executarNotificacaoInformativoUsuariosUseCase = executarNotificacaoInformativoUsuariosUseCase ?? throw new ArgumentNullException(nameof(executarNotificacaoInformativoUsuariosUseCase));
            this.executarNotificacaoInformativoUsuarioUseCase = executarNotificacaoInformativoUsuarioUseCase ?? throw new ArgumentNullException(nameof(executarNotificacaoInformativoUsuarioUseCase));
        }

        public Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(request.Filtros));

            return request.Rota switch
            {
                RotasRabbitSgp.RotaExcluirNotificacaoInformativo => executarExclusaoNotificacaoInformativoUsuariosUseCase.Executar(mensagem),
                RotasRabbitSgp.RotaExcluirNotificacaoInformativoUsuario => executarExclusaoNotificacaoInformativoUsuarioUseCase.Executar(mensagem),
                RotasRabbitSgp.RotaNotificacaoInformativo => executarNotificacaoInformativoUsuariosUseCase.Executar(mensagem),
                RotasRabbitSgp.RotaNotificacaoInformativoUsuario => executarNotificacaoInformativoUsuarioUseCase.Executar(mensagem),
                RotasRabbitSgpNotificacoes.Exclusao => Task.FromResult(true),
                RotasRabbitSgpNotificacoes.Criacao => Task.FromResult(true),
                RotasRabbitSgp.RemoverArquivoArmazenamento => Task.FromResult(true),
                _ => throw new NotImplementedException($"Rota: {request.Rota} não implementada para o teste"),
            };
        }
    }
}