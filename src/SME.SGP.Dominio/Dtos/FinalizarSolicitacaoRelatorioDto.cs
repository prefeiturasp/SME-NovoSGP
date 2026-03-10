using System;

namespace SME.SGP.Dominio.Dtos
{
    public class FinalizarSolicitacaoRelatorioDto
    {
        public int SolicitacaoRelatorioId { get; set; }
        public string UrlRelatorio { get; set; }
        public Guid CodigoCorrelacao { get; set; }
    }
}
