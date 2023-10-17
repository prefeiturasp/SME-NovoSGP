using MediatR;
using Minio.DataModel;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandler : IRequestHandler<ObterInfoComponentesCurricularesESPorTurmasCodigoQuery, IEnumerable<InfoComponenteCurricular>>
    {
        private readonly IMediator mediator;
        public ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<InfoComponenteCurricular>> Handle(ObterInfoComponentesCurricularesESPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            var turmas = await mediator.Send(new ObterTurmasDreUePorCodigosQuery(request.CodigosDeTurmas));
            var componentesDaTurma = new List<RetornoConsultaListagemTurmaComponenteDto>();
            foreach (var turma in turmas)
            {
                componentesDaTurma.AddRange((await mediator.Send(new ObterTurmasComComponentesQuery(turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre, turma.CodigoTurma, turma.AnoLetivo, qtdeRegistros:0, qtdeRegistrosIgnorados:0, bimestre: null, turma.ModalidadeCodigo, turma.Semestre, false, string.Empty, false, DateTime.Now, string.Empty))).Items);
            }

            var codigosComponentes = componentesDaTurma.Select(x => x.ComponenteCurricularCodigo).Distinct().ToArray();
            
            var componentesCurricularesSgp = await mediator.Send(new ObterInfoPedagogicasComponentesCurricularesPorIdsQuery(codigosComponentes));
            return componentesCurricularesSgp;
        }
    }
}
