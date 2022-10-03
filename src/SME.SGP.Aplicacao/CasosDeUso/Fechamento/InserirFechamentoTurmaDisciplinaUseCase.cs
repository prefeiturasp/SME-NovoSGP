using MediatR;
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

            if (!auditoria.EmAprovacao)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaFechamentoSync,
                    new ConsolidacaoTurmaDto(long.Parse(fechamentoTurma.TurmaId), 0),
                    Guid.NewGuid()));
            }

            return auditoria;
        }
    }
}
