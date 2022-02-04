using System;
using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos
{
    public class DiarioBordoDto
    {

        public DateTime Data { get; set; }
        public long AulaId { get; set; }
        public AulaDto Aula { get; set; }
        public long? DevolutivaId { get; set; }
        public string Devolutivas { get; set; }

        public bool Excluido { get; set; }
        public bool Migrado { get; set; }

        public bool TemPeriodoAberto { get; set; }

        public string Planejamento { get; set; }
        public string ReflexoesReplanejamento { get; set; }
        public bool InseridoCJ { get; set; }

        public IEnumerable<ObservacaoNotificacoesDiarioBordoDto> Observacoes { get; set; }

        public AuditoriaDto Auditoria { get; set; }
    }
}
