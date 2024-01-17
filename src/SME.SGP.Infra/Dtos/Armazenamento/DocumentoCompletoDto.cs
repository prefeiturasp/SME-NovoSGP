using System;
using System.Collections.Generic;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class DocumentoCompletoDto
    {
        public DocumentoCompletoDto(){}
        
        public long DocumentoId { get; set; }
        public string Classificacao { get; set; }
        public string TurmaNome { get; set; }
        public int Modalidade { get; set; }
        public string ComponenteCurricularNome { get; set; }
        public Guid CodigoArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public string TipoDocumento { get; set; }
        public string Usuario { get; set; }
        public DateTime Data { get; set; }
        public string NomeDre { get; set; }
        public string AbreviacaoDre { get; set; }
        public string CodigoDre { get; set; }
        public string NomeUe { get; set; }
        public string CodigoUe { get; set; }
        public TipoEscola TipoEscola { get; set; }
    }
}
