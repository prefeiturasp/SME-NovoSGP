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
    public class IncluirFilaExclusaoPendenciasAulaCommandHandler : IRequestHandler<IncluirFilaExclusaoPendenciasAulaCommand, bool>
    {
        private readonly IServicoFila servicoFila;

        public IncluirFilaExclusaoPendenciasAulaCommandHandler(IServicoFila servicoFila)
        {
            this.servicoFila = servicoFila ?? throw new ArgumentNullException(nameof(servicoFila));
        }

        public async Task<bool> Handle(IncluirFilaExclusaoPendenciasAulaCommand request, CancellationToken cancellationToken)
        {
            var command = new ExcluirTodasPendenciasAulaCommand(request.AulaId);

            servicoFila.PublicaFilaWorkerSgp(new PublicaFilaSgpDto(RotasRabbit.RotaExecutaExclusaoPendenciasAula, command, Guid.NewGuid(), request.Usuario));
            SentrySdk.AddBreadcrumb($"Incluir fila exclusão pendências aula", "RabbitMQ");

            return true;
        }
    }
}
