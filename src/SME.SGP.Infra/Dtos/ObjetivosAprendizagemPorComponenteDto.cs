using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ObjetivosAprendizagemPorComponenteDto
    {
        public long ComponenteCurricularId { get; set; }
        public List<ObjetivoAprendizagemDto> ObjetivosAprendizagem { get; set; }

        public ObjetivosAprendizagemPorComponenteDto()
        {
            ObjetivosAprendizagem = new List<ObjetivoAprendizagemDto>();
        }
    }
}
