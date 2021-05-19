using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoMatriculaDreDto
    {
        public FiltroConsolidacaoMatriculaDreDto(long id, IEnumerable<int> anosAnterioresParaConsolidar)
        {
            Id = id;
            AnosAnterioresParaConsolidar = anosAnterioresParaConsolidar;
        }

        public long Id { get; set; }
        public IEnumerable<int> AnosAnterioresParaConsolidar { get; set; }
    }
}
