using System;

namespace SME.SGP.Infra
{
    public class AnexoSecaoItineranciaEncaminhamentoNAAPADto
    {
        public long SecaoItineranciaId { get; set; }
        public Guid CodigoArquivo { get; set; }
        public string NomeArquivo { get; set; }
    }
}
