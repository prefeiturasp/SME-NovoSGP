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
        public string NomeDre { get; set; }
        public string AbreviacaoDre { get; set; }
        public string CodigoDre { get; set; }
        public string NomeUe { get; set; }
        public string CodigoUe { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public string SiglaNomeUe => $"{TipoEscola.ObterNomeCurto()} {NomeUe}";
    }
}
