using MediatR;
using Sentry;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaExclusaoAulaRecorrenteCommandHandler : IRequestHandler<IncluirFilaExclusaoAulaRecorrenteCommand, bool>
    {
        private readonly IServicoFila servicoFila;

        public IncluirFilaExclusaoAulaRecorrenteCommandHandler(IServicoFila servicoFila)
        {
            this.servicoFila = servicoFila ?? throw new ArgumentNullException(nameof(servicoFila));
        }

        public Task<bool> Handle(IncluirFilaExclusaoAulaRecorrenteCommand request, CancellationToken cancellationToken)
        {
            var command = new ExcluirAulaRecorrenteCommand(request.AulaId,
                                                           request.Recorrencia,
                                                           request.ComponenteCurricularNome,
                                                           request.Usuario);

            servicoFila.PublicaFilaWorkerSgp(new PublicaFilaSgpDto(RotasRabbit.RotaExcluirAulaRecorrencia, command, Guid.NewGuid(), request.Usuario, true));
            SentrySdk.AddBreadcrumb($"Incluir fila exclusão de aula recorrente", "RabbitMQ");

            return Task.FromResult(true);
        }
    }
}
