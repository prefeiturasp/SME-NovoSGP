using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoMatriculaUeDto
    {
        public FiltroConsolidacaoMatriculaUeDto(long ueId, IEnumerable<int> anosAnterioresParaConsolidar)
        {
            UeId = ueId;
            AnosAnterioresParaConsolidar = anosAnterioresParaConsolidar;
        }

        public IEnumerable<int> AnosAnterioresParaConsolidar { get; set; }
        public long UeId { get; set; }
    }
}
