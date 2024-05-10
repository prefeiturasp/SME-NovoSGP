using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterGradeAulasPorTurmaEProfessorUseCase
    {
        public async Task<GradeComponenteTurmaAulasDto> Executar(IMediator mediator, string turmaCodigo, long componenteCurricular, DateTime dataAula, string codigoRf = null, bool ehRegencia = false)
        {
            var ehExperienciPedagogica = await mediator.Send(new AulaDeExperienciaPedagogicaQuery(componenteCurricular));
            return await mediator.Send(new ObterGradeAulasPorTurmaEProfessorQuery(turmaCodigo, new long[] { componenteCurricular }, dataAula, codigoRf, ehRegencia));
        }
    }
}
