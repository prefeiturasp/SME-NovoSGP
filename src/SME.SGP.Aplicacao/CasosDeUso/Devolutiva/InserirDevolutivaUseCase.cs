using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirDevolutivaUseCase : AbstractUseCase, IInserirDevolutivaUseCase
    {
        public InserirDevolutivaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(InserirDevolutivaDto param)
        {
            IEnumerable<DateTime> datas = await mediator.Send(new ObterDatasDiariosPorIdsQuery(param.DiariosBordoIds));

            DateTime periodoInicio = datas.Min();
            DateTime periodoFim = datas.Max();

            AuditoriaDto auditoria = await mediator.Send(new InserirDevolutivaCommand(param.CodigoComponenteCurricular, param.DiariosBordoIds, periodoInicio, periodoFim, param.Descricao));

            bool diariosAtualizados = await mediator.Send(new AtualizarDiarioBordoComDevolutivaCommand(param.DiariosBordoIds, auditoria.Id));

            return auditoria;
        }
    }
}
