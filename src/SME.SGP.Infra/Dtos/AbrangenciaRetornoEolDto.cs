using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class AbrangenciaRetornoEolDto
    {
        public AbrangenciaRetornoEolDto()
        {
            Dres = new List<AbrangenciaDreRetornoEolDto>();
        }

        public IList<AbrangenciaDreRetornoEolDto> Dres { get; set; }
    }
}