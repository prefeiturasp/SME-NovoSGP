using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.Aula
{
    public class PublicarFilaSgpCommandFakeExcluirAulaUseCase : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IExcluirPlanoAulaPorAulaIdUseCase excluirPlanoAulaPorAulaIdUseCase;
        private readonly IExcluirWorkflowAprovacaoPorIdUseCase excluirWorkflowAprovacaoPorIdUseCase;

        public PublicarFilaSgpCommandFakeExcluirAulaUseCase(IExcluirPlanoAulaPorAulaIdUseCase excluirPlanoAulaPorAulaIdUseCase, IExcluirWorkflowAprovacaoPorIdUseCase excluirWorkflowAprovacaoPorIdUseCase)
        {
            this.excluirPlanoAulaPorAulaIdUseCase = excluirPlanoAulaPorAulaIdUseCase ?? throw new ArgumentNullException(nameof(excluirPlanoAulaPorAulaIdUseCase));
            this.excluirWorkflowAprovacaoPorIdUseCase = excluirWorkflowAprovacaoPorIdUseCase ?? throw new ArgumentNullException(nameof(excluirWorkflowAprovacaoPorIdUseCase));
        }

        public Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(request.Filtros));

            return request.Rota switch
            {
                RotasRabbitSgpAula.PlanoAulaDaAulaExcluir => excluirPlanoAulaPorAulaIdUseCase.Executar(mensagem),
                RotasRabbitSgp.WorkflowAprovacaoExcluir => excluirWorkflowAprovacaoPorIdUseCase.Executar(mensagem),
                _ => throw new NotImplementedException($"Rota: {request.Rota} não implementada para o teste"),
            };
        }
    }
}