using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
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

            var codigoDre = mensagemRabbit.Mensagem.ToString();

            await mediator.Send(new EnviarSincronizacaoEstruturaInstitucionalUesCommand(codigoDre));

            return true;
        }
    }
}
