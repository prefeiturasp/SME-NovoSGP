using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFake
{
    public class PublicarFilaSgpCommandFakeItinerancia : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IExecutarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase executarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase;

        public PublicarFilaSgpCommandFakeItinerancia(IExecutarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase executarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase)
        {
            this.executarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase = executarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase ?? throw new ArgumentNullException(nameof(executarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase));
        }

        public Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(request.Filtros));

            return request.Rota switch
            {
                RotasRabbitSgpNAAPA.RotaExcluirNotificacaoInatividadeAtendimento => executarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase.Executar(mensagem),
                RotasRabbitSgpNotificacoes.Exclusao => Task.FromResult(true),
                RotasRabbitSgpNotificacoes.Criacao => Task.FromResult(true),
                RotasRabbitSgp.RemoverArquivoArmazenamento => Task.FromResult(true),
                _ => throw new NotImplementedException($"Rota: {request.Rota} não implementada para o teste"),
            };
        }
    }
}