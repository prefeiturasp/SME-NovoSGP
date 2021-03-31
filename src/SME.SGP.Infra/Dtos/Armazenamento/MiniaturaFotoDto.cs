using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class MiniaturaFotoDto
    {
        public Guid Codigo { get; set; }
        public Guid CodigoFotoOriginal { get; set; }
        public TipoArquivo Tipo { get; set; }
        public string TipoConteudo { get; set; }
        public string Nome { get; set; }
        public long MiniaturaId { get; set; }
        public long FotoId { get; set; }
        public long MiniaturaArquivoId { get; set; }
        public long ArquivoId { get; set; }

    }
}
