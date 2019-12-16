using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class EstruturaInstitucionalRetornoEolDTO
    {
        public EstruturaInstitucionalRetornoEolDTO()
        {
            Dres = new List<AbrangenciaDreRetornoEolDto>();
        }

        public IList<AbrangenciaDreRetornoEolDto> Dres { get; set; }
    }
}