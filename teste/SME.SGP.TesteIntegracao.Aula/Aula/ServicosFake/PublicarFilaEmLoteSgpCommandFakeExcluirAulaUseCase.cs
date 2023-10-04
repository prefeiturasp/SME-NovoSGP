using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao
{
    public class PublicarFilaEmLoteSgpCommandFakeExcluirAulaUseCase : IRequestHandler<PublicarFilaEmLoteSgpCommand, bool>
    {
        private readonly IExcluirPlanoAulaPorAulaIdUseCase excluirPlanoAulaPorAulaIdUseCase;
        private readonly IExcluirWorkflowAprovacaoPorIdUseCase excluirWorkflowAprovacaoPorIdUseCase;

        public PublicarFilaEmLoteSgpCommandFakeExcluirAulaUseCase(IExcluirPlanoAulaPorAulaIdUseCase excluirPlanoAulaPorAulaIdUseCase, IExcluirWorkflowAprovacaoPorIdUseCase excluirWorkflowAprovacaoPorIdUseCase)
        {
            this.excluirPlanoAulaPorAulaIdUseCase = excluirPlanoAulaPorAulaIdUseCase ?? throw new ArgumentNullException(nameof(excluirPlanoAulaPorAulaIdUseCase));
            this.excluirWorkflowAprovacaoPorIdUseCase = excluirWorkflowAprovacaoPorIdUseCase ?? throw new ArgumentNullException(nameof(excluirWorkflowAprovacaoPorIdUseCase));
        }

        public Task<bool> Handle(PublicarFilaEmLoteSgpCommand request, CancellationToken cancellationToken)
        {
            foreach (var command in request.Commands)
            {
                var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(command.Filtros));

                return command.Rota switch
                {
                    RotasRabbitSgpAula.PlanoAulaDaAulaExcluir => excluirPlanoAulaPorAulaIdUseCase.Executar(mensagem),
                    RotasRabbitSgp.WorkflowAprovacaoExcluir => excluirWorkflowAprovacaoPorIdUseCase.Executar(mensagem),
                    _ => throw new NotImplementedException($"Rota: {command.Rota} não implementada para o teste"),
                };
            }
            return Task.FromResult(true);
        }
    }
}
