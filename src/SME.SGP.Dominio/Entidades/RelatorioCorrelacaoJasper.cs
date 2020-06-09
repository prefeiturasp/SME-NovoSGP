using System;

namespace SME.SGP.Dominio.Entidades
{
    public class RelatorioCorrelacaoJasper : EntidadeBase
    {
        public Guid JSessionId { get; private set; }

        public Guid ExportId { get; private set; }

        public Guid RequestId { get; private set; }

        public RelatorioCorrelacao RelatorioCorrelacao { get; private set; }
        public long RelatorioCorrelacaoId { get; set; }

        public RelatorioCorrelacaoJasper()
        {
            JSessionId = Guid.NewGuid();
            ExportId = Guid.NewGuid();
            RequestId = Guid.NewGuid();
        }
    }
}
