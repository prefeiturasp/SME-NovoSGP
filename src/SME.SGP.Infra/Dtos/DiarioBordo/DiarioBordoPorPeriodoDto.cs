using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class DiarioBordoPorPeriodoDto
    {
        public long AulaId { get; set; }
        public long? DiarioBordoId { get; set; }
        public DateTime DataAula { get; set; }
        public bool Pendente { get; set; }
        public string Titulo { get; set; }
        public string Planejamento { get; set; }
        public string ReflexoesReplanejamento { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}
