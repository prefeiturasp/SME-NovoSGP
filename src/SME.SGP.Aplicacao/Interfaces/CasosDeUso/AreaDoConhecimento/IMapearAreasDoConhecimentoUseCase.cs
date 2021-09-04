using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.AreaDoConhecimento
{
    public interface IMapearAreasDoConhecimentoUseCase
    {
        IEnumerable<IGrouping<(string Nome, int? Ordem, long Id), AreaDoConhecimentoDto>> MapearAreasDoConhecimento(IEnumerable<DisciplinaDto> componentesCurricularesDaTurma,
                                                                                                                    IEnumerable<AreaDoConhecimentoDto> areasDoConhecimentos,
                                                                                                                    IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto> grupoAreaOrdenacao,
                                                                                                                    long grupoMatrizId);
    }
}
