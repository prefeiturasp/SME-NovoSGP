using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoConclusaoEncaminhamentoAEEUseCase : AbstractUseCase, IExecutaNotificacaoConclusaoEncaminhamentoAEEUseCase
    {
        public ExecutaNotificacaoConclusaoEncaminhamentoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar(long encaminhamentoAEEId, string usuarioRF, string usuarioNome)
        {

            SentrySdk.AddBreadcrumb($"Mensagem NotificacaoConclusaoEncaminhamentoAEEUseCase", "Rabbit - NotificacaoConclusaoEncaminhamentoAEEUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.RotaNotificacaoRegistroConclusaoEncaminhamentoAEE,
                new NotificacaoConclusaoEncaminhamentoAEEDto
                {
                    EncaminhamentoAEEId = encaminhamentoAEEId,
                    UsuarioRF = usuarioRF,
                    UsuarioNome = usuarioNome
                }, Guid.NewGuid(), null));
        }
    }
}
