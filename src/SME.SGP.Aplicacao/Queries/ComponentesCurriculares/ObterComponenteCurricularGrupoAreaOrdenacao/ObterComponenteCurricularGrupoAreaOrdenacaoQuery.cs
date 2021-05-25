using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponenteCurricularGrupoAreaOrdenacaoQuery : IRequest<IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>>
    {
        public ObterComponenteCurricularGrupoAreaOrdenacaoQuery(long[] grupoMatrizIds, long[] areaDoConhecimentoIds)
        {
            GrupoMatrizIds = grupoMatrizIds;
            AreaDoConhecimentoIds = areaDoConhecimentoIds;
        }

        public long[] GrupoMatrizIds { get; set; }

        public long[] AreaDoConhecimentoIds { get; set; }
    }
}
