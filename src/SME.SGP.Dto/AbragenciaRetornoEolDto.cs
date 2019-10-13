using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class AbragenciaRetornoEolDto
    {
        public AbragenciaRetornoEolDto()
        {
            Dres = new List<AbragenciaDreRetornoEolDto>();
        }

        public IList<AbragenciaDreRetornoEolDto> Dres { get; set; }
    }
}