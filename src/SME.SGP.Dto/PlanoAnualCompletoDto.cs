using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dto
{
    public class PlanoAnualCompletoDto : PlanoAnualDto
    {
        //public long Ano { get; set; }
        //public long Bimestre { get; set; }
        //public string Descricao { get; set; }
        //public long EscolaId { get; set; }
        //public long Id { get; set; }
        public IEnumerable<long> IdsObjetivosAprendizagem => ObjetivosAprendizagem?.Split(',').Select(c => Convert.ToInt64(c));

        //public long TurmaId { get; set; }
        private string ObjetivosAprendizagem { get; set; }
    }
}