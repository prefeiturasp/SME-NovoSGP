using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class EnviarSincronizacaoEstruturaOrganizacionalUesExclusaoUseCase : AbstractUseCase, IEnviarSincronizacaoEstruturaOrganizacionalUesExclusaoUseCase
    {
        public EnviarSincronizacaoEstruturaOrganizacionalUesExclusaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            SentrySdk.AddBreadcrumb($"Mensagem EnviarNotificacaoSincronizacaoUesExclusaoUseCase", "Rabbit - EnviarNotificacaoSincronizacaoUesExclusaoUseCase");

            var command = mensagemRabbit.ObterObjetoMensagem<EnviarSincronizacaoEstruturaInstitucionalUesCommand>();

            return await mediator.Send(command);
        }
    }

}
