using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class SupervisorEscolasDto : AuditoriaDto
    {
        public SupervisorEscolasDto()
        {
            Escolas = new List<UnidadeEscolarDto>();
        }

        public List<UnidadeEscolarDto> Escolas { get; set; }
        public string SupervisorId { get; set; }
        public string SupervisorNome { get; set; }
    }
}