using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class PlanoCicloDto
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public IEnumerable<long> IdsMatrizesSaber { get; set; }
        public IEnumerable<long> IdsObjetivosDesenvolvimento { get; set; }
    }
}
