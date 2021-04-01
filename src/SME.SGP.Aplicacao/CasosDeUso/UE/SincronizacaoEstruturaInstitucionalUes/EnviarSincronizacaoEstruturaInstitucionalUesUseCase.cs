using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class EnviarSincronizacaoEstruturaInstitucionalUesUseCase : AbstractUseCase, IEnviarSincronizacaoEstruturaInstitucionalUesUseCase
    {
        public EnviarSincronizacaoEstruturaInstitucionalUesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            SentrySdk.AddBreadcrumb($"Mensagem EnviarSincronizacaoEstruturaInstitucionalUesInclusaoUseCase", "Rabbit - EnviarSincronizacaoEstruturaInstitucionalUesInclusaoUseCase");

            var codigosDre = await mediator.Send(new ObterCodigosDresQuery());

            foreach (var codigoDre in codigosDre)
                await mediator.Send(new EnviarSincronizacaoEstruturaInstitucionalUesCommand(codigoDre));

            return true;
        }
    }
}
