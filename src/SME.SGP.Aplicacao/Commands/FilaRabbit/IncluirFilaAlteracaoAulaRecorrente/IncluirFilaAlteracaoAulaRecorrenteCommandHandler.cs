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
        private readonly IServicoFila servicoFila;

        public IncluirFilaAlteracaoAulaRecorrenteCommandHandler(IServicoFila servicoFila)
        {
            this.servicoFila = servicoFila ?? throw new ArgumentNullException(nameof(servicoFila));
        }

        public Task<bool> Handle(IncluirFilaAlteracaoAulaRecorrenteCommand request, CancellationToken cancellationToken)
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

            servicoFila.PublicaFilaWorkerSgp(new PublicaFilaSgpDto(RotasRabbit.RotaAlterarAulaRecorrencia, command, Guid.NewGuid(), request.Usuario));
            SentrySdk.AddBreadcrumb($"Incluir fila alteração de aula recorrente", "RabbitMQ");

            return Task.FromResult(true);
        }
    }
}
