using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalVisaoGeralRetornoDto
    {
        public string Indicador { get; set; }
        public IEnumerable<PainelEducacionalSerieDto> Series { get; set; }
    }

    public class PainelEducacionalSerieDto
    {
        public string Serie { get; set; }
        public decimal Valor { get; set; }
    }
}
