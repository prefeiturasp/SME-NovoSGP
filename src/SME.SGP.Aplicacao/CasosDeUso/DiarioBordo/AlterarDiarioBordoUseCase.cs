using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarDiarioBordoUseCase : AbstractUseCase, IAlterarDiarioBordoUseCase
    {
        public AlterarDiarioBordoUseCase(IMediator mediator): base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(AlterarDiarioBordoDto param)
        {
            var auditoria = await mediator.Send(new AlterarDiarioBordoCommand(param.Id, param.AulaId, param.Planejamento, param.ReflexoesReplanejamento,param.ComponenteCurricularId));

            return auditoria;
        }
    }
}
