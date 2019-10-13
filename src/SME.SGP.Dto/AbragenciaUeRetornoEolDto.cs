using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class AbragenciaUeRetornoEolDto
    {
        public AbragenciaUeRetornoEolDto()
        {
            Turmas = new List<AbragenciaTurmaRetornoEolDto>();
        }

        public string Codigo { get; set; }
        public string Nome { get; set; }
        public IList<AbragenciaTurmaRetornoEolDto> Turmas { get; set; }
    }
}