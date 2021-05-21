using MediatR;
using Sentry;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.FilaRabbit.IncluirFilaInserirAulaRecorrente
{
    public class IncluirFilaInserirAulaRecorrenteCommandHandler : IRequestHandler<IncluirFilaInserirAulaRecorrenteCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaInserirAulaRecorrenteCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaInserirAulaRecorrenteCommand request, CancellationToken cancellationToken)
        {
            var command = new InserirAulaRecorrenteCommand(request.Usuario,
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

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaInserirAulaRecorrencia, command, Guid.NewGuid(), request.Usuario, true));
            SentrySdk.AddBreadcrumb($"Incluir fila inserção de aula recorrente", "RabbitMQ");

            return true;
        }
    }
}
