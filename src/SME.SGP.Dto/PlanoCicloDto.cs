using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class PlanoCicloDto
    {
        public PlanoCicloDto()
        {
            IdsObjetivosDesenvolvimento = new List<long>();
            IdsMatrizesSaber = new List<long>();
        }

        public string Descricao { get; set; }
        public long Id { get; set; }
        public List<long> IdsMatrizesSaber { get; set; }
        public List<long> IdsObjetivosDesenvolvimento { get; set; }
    }
}