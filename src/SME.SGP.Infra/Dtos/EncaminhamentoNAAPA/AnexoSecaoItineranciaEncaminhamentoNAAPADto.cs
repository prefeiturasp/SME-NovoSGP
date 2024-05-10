using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AnexoSecaoItineranciaEncaminhamentoNAAPADto
    {
        public long SecaoItineranciaId { get; set; }
        public Guid CodigoArquivo { get; set; }
        public string NomeArquivo { get; set; }
    }
}
