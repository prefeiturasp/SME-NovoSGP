using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Infra
{
    public class PlanoAnualObjetivosDisciplinaDto
    {
        public int AnoLetivo { get; set; }
        public int Bimestre { get; set; }
        public List<ObjetivoAprendizagemDto> ObjetivosAprendizagem { get; set; }
        public IEnumerable<long> IdsObjetivosAprendizagem => ObjetivosAprendizagemPlano?.Split(',').Select(c => Convert.ToInt64(c));
        public string ObjetivosAprendizagemPlano { get; set; }
    }
}
