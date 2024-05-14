using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra.Dtos
{
    public class InformativoAnexoDto
    {
        public long Id { get; set; }
        public long InformativoId { get; set; }
        public long ArquivoId { get; set; }
        public Guid Codigo { get; set; }
        public string Nome { get; set; }
        public TipoArquivo Tipo { get; set;}
    }
}
