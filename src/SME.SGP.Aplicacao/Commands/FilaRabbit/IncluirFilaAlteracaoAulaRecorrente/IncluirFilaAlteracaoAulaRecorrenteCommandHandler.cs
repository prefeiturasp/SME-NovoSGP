using MediatR;
using Sentry;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaAlteracaoAulaRecorrenteCommandHandler : IRequestHandler<IncluirFilaAlteracaoAulaRecorrenteCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaAlteracaoAulaRecorrenteCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaAlteracaoAulaRecorrenteCommand request, CancellationToken cancellationToken)
        {
            var command = new AlterarAulaRecorrenteCommand(request.Usuario,
                                                           request.AulaId,
                                                           request.DataAula,
                                                           request.Quantidade,
                                                           request.CodigoTurma,
                                                           request.ComponenteCurricularId,
                                                           request.NomeComponenteCurricular,
                                                           request.TipoCalendarioId,
                                                           request.TipoAula,
                                                           request.CodigoUe,
                                                           request.EhRegencia,
                                                           request.RecorrenciaAula);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaAlterarAulaRecorrencia, command, Guid.NewGuid(), request.Usuario));
            SentrySdk.AddBreadcrumb($"Incluir fila alteração de aula recorrente", "RabbitMQ");

            return true;
        }
    }
}
