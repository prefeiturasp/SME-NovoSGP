using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class DiarioBordoDetalhesDto
    {
        public long Id { get; set; }
        public long AulaId { get; set; }
        public AulaDto Aula { get; set; }
        public long? DevolutivaId { get; set; }
        public string Devolutivas { get; set; }

        public string Planejamento { get; set; }

        public bool Excluido { get; set; }
        public bool Migrado { get; set; }

        public bool TemPeriodoAberto { get; set; }
        public bool InseridoCJ { get; set; }

        public AuditoriaDto Auditoria { get; set; }

        public IEnumerable<ObservacaoNotificacoesDiarioBordoDto> Observacoes { get; set; }
    }
}
