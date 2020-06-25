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
        private readonly IServicoFila servicoFila;

        public IncluirFilaInserirAulaRecorrenteCommandHandler(IServicoFila servicoFila)
        {
            this.servicoFila = servicoFila ?? throw new ArgumentNullException(nameof(servicoFila));
        }

        public Task<bool> Handle(IncluirFilaInserirAulaRecorrenteCommand request, CancellationToken cancellationToken)
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

            servicoFila.PublicaFilaWorkerSgp(new PublicaFilaSgpDto(RotasRabbit.RotaInserirAulaRecorrencia, command, Guid.NewGuid(), request.Usuario, true));
            SentrySdk.AddBreadcrumb($"Incluir fila inserção de aula recorrente", "RabbitMQ");

            return Task.FromResult(true);
        }
    }
}
