using MediatR;
using SME.SGP.Infra;
using System.Collections;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.AreaDoConhecimento.ObterComponentesAreasConhecimento
{
    public class ObterComponentesAreasConhecimentoQuery : IRequest<IEnumerable<DisciplinaDto>>
    {
        public ObterComponentesAreasConhecimentoQuery(IEnumerable<DisciplinaDto> componentesCurricularesTurma,
                                                      IEnumerable<AreaDoConhecimentoDto> areasConhecimento)
        {
            ComponentesCurricularesTurma = componentesCurricularesTurma;
            AreasConhecimento = areasConhecimento;
        }

        public IEnumerable<DisciplinaDto> ComponentesCurricularesTurma { get; set; }
        public IEnumerable<AreaDoConhecimentoDto> AreasConhecimento { get; set; }
    }
}
