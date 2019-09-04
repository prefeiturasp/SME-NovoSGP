using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class FiltroObjetivosAprendizagemDto
    {
        public FiltroObjetivosAprendizagemDto()
        {
            ComponentesCurricularesIds = new List<long>();
        }

        public int Ano { get; set; }
        public IList<long> ComponentesCurricularesIds { get; set; }
    }
}