using System;
using System.Collections.Generic;
using System.Text;

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
