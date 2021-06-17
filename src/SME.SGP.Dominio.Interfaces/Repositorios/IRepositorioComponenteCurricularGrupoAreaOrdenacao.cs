using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioComponenteCurricularGrupoAreaOrdenacao
    {
        Task<IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>> ObterOrdenacaoPorGruposAreas(long[] grupoMatrizIds, long[] areaConhecimentoIds);
    }
}
