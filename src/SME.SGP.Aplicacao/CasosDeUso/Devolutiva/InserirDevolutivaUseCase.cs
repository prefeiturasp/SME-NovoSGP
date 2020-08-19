using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirDevolutivaUseCase : AbstractUseCase, IInserirDevolutivaUseCase
    {
        public InserirDevolutivaUseCase(IMediator mediator): base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(InserirDevolutivaDto param)
        {
            var auditoria = await mediator.Send(new InserirDevolutivaCommand(param.CodigoComponenteCurricular, param.PeriodoInicio, param.PeriodoFim, param.Descricao));

            return auditoria;
        }
    }
}
