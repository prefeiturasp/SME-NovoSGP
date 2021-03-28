using Microsoft.AspNetCore.Http;

namespace SME.SGP.Infra
{
    public class AcompanhamentoTurmaDto
    {
        public long TurmaId { get; set; }
        public int Semestre { get; set; }
        public string ApanhadoGeral { get; set; }
        public long AcompanhamentoTurmaId { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}

