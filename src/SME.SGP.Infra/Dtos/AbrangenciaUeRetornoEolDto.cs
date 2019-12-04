using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class AbrangenciaUeRetornoEolDto
    {
        public AbrangenciaUeRetornoEolDto()
        {
            Turmas = new List<AbrangenciaTurmaRetornoEolDto>();
        }

        public string Codigo { get; set; }
        public TipoEscola CodTipoEscola { get; set; }
        public string Nome { get; set; }
        public IList<AbrangenciaTurmaRetornoEolDto> Turmas { get; set; }
    }
}