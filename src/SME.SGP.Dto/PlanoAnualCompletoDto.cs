using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dto
{
    public class PlanoAnualCompletoDto
    {
        public string Descricao { get; set; }
        public long Id { get; set; }
        public IEnumerable<long> IdsObjetivosAprendizagem => ObjetivosAprendizagem?.Split(',').Select(c => Convert.ToInt64(c));
        private string MatrizesSaber { get; set; }
        private string ObjetivosAprendizagem { get; set; }
    }
}