using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterGradeAulasPorTurmaEProfessorUseCase
    {
        public static async Task<GradeComponenteTurmaAulasDto> Executar(IMediator mediator, string turmaCodigo, long componenteCurricular, DateTime dataAula, string codigoRf = null, bool ehRegencia = false)
        {
            var ehExperienciPedagogica = await mediator.Send(new AulaDeExperienciaPedagogicaQuery(componenteCurricular));
            return await mediator.Send(new ObterGradeAulasPorTurmaEProfessorQuery(turmaCodigo, componenteCurricular, dataAula, codigoRf, ehRegencia));
        }
    }
}
