using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra
{
    public class PlanoCicloCompletoDto
    {
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public long CicloId { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string Descricao { get; set; }
        public long Id { get; set; }
        public IEnumerable<long> IdsMatrizesSaber => MatrizesSaber?.Split(',').Select(c => Convert.ToInt64(c));
        public IEnumerable<long> IdsObjetivosDesenvolvimentoSustentavel => ObjetivosDesenvolvimento?.Split(',').Select(c => Convert.ToInt64(c));
        public bool Migrado { get; set; }
        private string MatrizesSaber { get; set; }
        private string ObjetivosDesenvolvimento { get; set; }
    }
}