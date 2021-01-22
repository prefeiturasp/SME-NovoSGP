using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirDiarioBordoUseCase : AbstractUseCase, IInserirDiarioBordoUseCase
    {
        public InserirDiarioBordoUseCase(IMediator mediator): base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(InserirDiarioBordoDto param)
        {
            var auditoria = await mediator.Send(new InserirDiarioBordoCommand(param.AulaId, param.Planejamento, param.ReflexoesReplanejamento));
            await mediator.Send(new ExcluirPendenciaAulaCommand(param.AulaId, Dominio.TipoPendencia.DiarioBordo));
            return auditoria;
        }
    }
}
