using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ExecutarSincronizacaoAcompanhamentoAprendizagemAlunoSyncUseCase : AbstractUseCase, IExecutarSincronizacaoAcompanhamentoAprendizagemAlunoSyncUseCase
    {
        public ExecutarSincronizacaoAcompanhamentoAprendizagemAlunoSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutarSincronizacaoAcompanhamentoAprendizagemAlunoSyncUseCase", "Rabbit - ExecutarSincronizacaoAcompanhamentoAprendizagemAlunoSyncUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarAcompanhamentoAprendizagemAluno, string.Empty, Guid.NewGuid(), null));
        }
    }
}
