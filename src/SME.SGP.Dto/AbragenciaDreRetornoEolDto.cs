using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class AbragenciaDreRetornoEolDto
    {
        public AbragenciaDreRetornoEolDto()
        {
            Ues = new List<AbragenciaUeRetornoEolDto>();
        }

        public string Abreviacao { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public IList<AbragenciaUeRetornoEolDto> Ues { get; set; }
    }
}