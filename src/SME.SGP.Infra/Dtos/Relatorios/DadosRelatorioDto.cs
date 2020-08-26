using System;

namespace SME.SGP.Infra
{
    public class DadosRelatorioDto
    {
        public Guid RequisicaoId { get; set; }
        public Guid ExportacaoId { get; set; }
        public Guid CorrelacaoId { get; set; }
        public string JSessionId { get; set; }
    }
}
