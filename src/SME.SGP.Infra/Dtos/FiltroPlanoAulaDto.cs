using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroPlanoAulaDto
    {
        public DateTime Data { get; set; }
        public string EscolaId { get; set; }
        public int TurmaId { get; set; }
        public string DisciplinaId { get; set; }
    }
}
