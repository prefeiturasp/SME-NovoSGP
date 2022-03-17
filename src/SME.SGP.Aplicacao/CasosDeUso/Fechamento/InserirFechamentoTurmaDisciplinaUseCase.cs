using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static SME.SGP.Aplicacao.GerarNotificacaoAlteracaoLimiteDiasUseCase;

namespace SME.SGP.Aplicacao
{
    public class InserirFechamentoTurmaDisciplinaUseCase : AbstractUseCase, IInserirFechamentoTurmaDisciplinaUseCase
    {
        public InserirFechamentoTurmaDisciplinaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaPersistenciaDto> Executar(FechamentoFinalTurmaDisciplinaDto fechamentoTurma)
        {

            var auditoria = await mediator.Send(new SalvarFechamentoCommand(fechamentoTurma));

            if (!auditoria.EmAprovacao)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitFechamento.ConsolidarTurmaFechamentoSync,
                                                               new ConsolidacaoTurmaDto(Int64.Parse(fechamentoTurma.TurmaId), 0),
                                                               Guid.NewGuid(),
                                                               null));
            return auditoria;
        }
    }
}
