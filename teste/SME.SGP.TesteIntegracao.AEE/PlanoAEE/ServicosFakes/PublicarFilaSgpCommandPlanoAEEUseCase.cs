using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.PendenciaGeral.ServicosFake
{
    public class PublicarFilaSgpCommandPlanoAEEUseCase : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IAtualizarInformacoesDoPlanoAEEUseCase executarAtualizarInformacoesDoPlanoAEEUseCase;
        private readonly IAtualizarTurmaDoPlanoAEEUseCase executarAtualizarTurmaDoPlanoAEEUseCase;

        public PublicarFilaSgpCommandPlanoAEEUseCase(IAtualizarInformacoesDoPlanoAEEUseCase executarAtualizarInformacoesDoPlanoAEEUseCase, IAtualizarTurmaDoPlanoAEEUseCase executarAtualizarTurmaDoPlanoAEEUseCase)
        {
            this.executarAtualizarInformacoesDoPlanoAEEUseCase = executarAtualizarInformacoesDoPlanoAEEUseCase ?? throw new ArgumentNullException(nameof(executarAtualizarInformacoesDoPlanoAEEUseCase));
            this.executarAtualizarTurmaDoPlanoAEEUseCase = executarAtualizarTurmaDoPlanoAEEUseCase ?? throw new ArgumentNullException(nameof(executarAtualizarTurmaDoPlanoAEEUseCase));
        }

        public Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(request.Filtros));

            return request.Rota switch
            {
                RotasRabbitSgp.ExecutarAtualizacaoDasInformacoesPlanoAEE => executarAtualizarInformacoesDoPlanoAEEUseCase.Executar(mensagem),
                RotasRabbitSgp.ExecutarAtualizacaoDaTurmaDoPlanoAEE => executarAtualizarTurmaDoPlanoAEEUseCase.Executar(mensagem),
                _ => throw new NotImplementedException($"Rota: {request.Rota} não implementada para o teste"),
            };
        }
    }
}
