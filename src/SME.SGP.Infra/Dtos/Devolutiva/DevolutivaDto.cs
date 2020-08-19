using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class DevolutivaDto
    {
        public DevolutivaDto()
        {
        }

        public string Descricao { get; set; }

        public IEnumerable<long> DiariosIds { get; set; }

        public long CodigoComponenteCurricular { get; set; }

        public DateTime PeriodoInicio { get; set; }

        public DateTime PeriodoFim { get; set; }

        public AuditoriaDto Auditoria { get; set; }
    }
}
