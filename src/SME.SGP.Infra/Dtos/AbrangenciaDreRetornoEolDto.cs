using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dto
{
    [ExcludeFromCodeCoverage]
    public class AbrangenciaDreRetornoEolDto
    {
        public AbrangenciaDreRetornoEolDto()
        {
            Ues = new List<AbrangenciaUeRetornoEolDto>();
        }

        public string Abreviacao { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public IList<AbrangenciaUeRetornoEolDto> Ues { get; set; }
    }
}