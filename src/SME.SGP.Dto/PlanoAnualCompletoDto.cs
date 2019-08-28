using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dto
{
    public class PlanoAnualCompletoDto : PlanoAnualDto
    {
        public DateTime AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
        public new IEnumerable<long> IdsObjetivosAprendizagem => ObjetivosAprendizagem?.Split(',').Select(c => Convert.ToInt64(c));

        private string ObjetivosAprendizagem { get; set; }
    }
}