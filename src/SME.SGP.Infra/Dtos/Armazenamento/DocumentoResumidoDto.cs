using System;
using System.Collections.Generic;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class DocumentoResumidoDto
    {
        public DocumentoResumidoDto(){}
        
        public long DocumentoId { get; set; }
        public string Classificacao { get; set; }
        public string TurmaComponenteCurricular{ get; set; }
        public string TipoDocumento { get; set; }
        public string Usuario { get; set; }
        public DateTime Data { get; set; }
        public List<ArquivoResumidoDto> Arquivos { get; set; }
    }
}
