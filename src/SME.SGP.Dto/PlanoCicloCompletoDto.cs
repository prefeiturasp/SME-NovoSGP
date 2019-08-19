using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dto
{
    public class PlanoCicloCompletoDto
    {
        public long CicloId { get; set; }
        public string Descricao { get; set; }
        public long Id { get; set; }
        public IEnumerable<long> IdsMatrizesSaber => MatrizesSaber?.Split(',').Select(c => Convert.ToInt64(c));
        public IEnumerable<long> IdsObjetivosDesenvolvimentoSustentavel => ObjetivosDesenvolvimento?.Split(',').Select(c => Convert.ToInt64(c));
        private string MatrizesSaber { get; set; }
        private string ObjetivosDesenvolvimento { get; set; }
    }
}
