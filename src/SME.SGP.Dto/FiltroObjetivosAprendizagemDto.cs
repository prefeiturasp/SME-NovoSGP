using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class FiltroObjetivosAprendizagemDto
    {
        public int Ano { get; set; }
        public IList<long> ComponentesCurricularesIds { get; set; }
    }
}