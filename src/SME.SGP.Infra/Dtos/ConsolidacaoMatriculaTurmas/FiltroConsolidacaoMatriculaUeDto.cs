using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoMatriculaUeDto
    {
        public FiltroConsolidacaoMatriculaUeDto(string ueCodigo, IEnumerable<int> anosAnterioresParaConsolidar)
        {
            UeCodigo = ueCodigo;
            AnosAnterioresParaConsolidar = anosAnterioresParaConsolidar;
        }

        public IEnumerable<int> AnosAnterioresParaConsolidar { get; set; }
        public string UeCodigo { get; set; }
    }
}
