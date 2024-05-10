using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDatasAulasPorTurmaEComponenteUseCase : AbstractUseCase, IObterDatasAulasPorTurmaEComponenteUseCase
    {
        public ObterDatasAulasPorTurmaEComponenteUseCase(IMediator mediator) : base(mediator) 
        {}

        public async Task<IEnumerable<DatasAulasDto>> Executar(ConsultaDatasAulasDto param)
        {
            return await mediator.Send(new ObterDatasAulasPorProfessorEComponenteQuery(param.TurmaCodigo, param.ComponenteCurricularCodigo));
        }
    }
}
