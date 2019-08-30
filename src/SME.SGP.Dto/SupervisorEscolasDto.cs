using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class SupervisorEscolasDto
    {
        public SupervisorEscolasDto()
        {
            Escolas = new List<UnidadeEscolarDto>();
        }

        public List<UnidadeEscolarDto> Escolas { get; set; }
        public long SupervisorId { get; set; }
        public string SupervisorNome { get; set; }
    }
}