using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirFechamentoTurmaDisciplinaUseCase : AbstractUseCase, IInserirFechamentoTurmaDisciplinaUseCase
    {
        public InserirFechamentoTurmaDisciplinaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaPersistenciaFechamentoNotaConceitoTurmaDto> Executar(FechamentoFinalTurmaDisciplinaDto fechamentoTurma)
        {
            var auditoria = await mediator.Send(new SalvarFechamentoCommand(fechamentoTurma));

            if (!auditoria.EmAprovacao && fechamentoTurma.EhFinal)
            {
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(fechamentoTurma.TurmaId));

                if (turma.NaoEhNulo())
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaFechamentoSync,
                        new ConsolidacaoTurmaDto(turma.Id, 0),
                        Guid.NewGuid()));
            }

            return auditoria;
        }
    }
}
