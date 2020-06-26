using System;

namespace SME.SGP.Dominio
{
    public class RelatorioCorrelacaoJasper
    {
        public RelatorioCorrelacaoJasper(RelatorioCorrelacao relatorioCorrelacao, string jSessionId, Guid exportId, Guid requestId)
        {
            JSessionId = jSessionId;
            ExportId = exportId;
            RequestId = requestId;
            RelatorioCorrelacao = relatorioCorrelacao;
            RelatorioCorrelacaoId = relatorioCorrelacao.Id;
        }

        protected RelatorioCorrelacaoJasper()
        {

        }

        public Guid ExportId { get;  set; }
        public long Id { get; set; }
        public string JSessionId { get;  set; }
        public RelatorioCorrelacao RelatorioCorrelacao { get;  set; }
        public long RelatorioCorrelacaoId { get; set; }
        public Guid RequestId { get;  set; }
    }
}
