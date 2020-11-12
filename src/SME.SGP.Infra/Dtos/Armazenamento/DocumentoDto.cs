using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class DocumentoDto
    {
        public DocumentoDto() { }

        public DocumentoDto(string tipoDocumento, string classificacao, string usuario, DateTime dataUpload)
        {
            TipoDocumento = tipoDocumento;
            Classificacao = classificacao;
            Usuario = usuario;
            DataUpload = dataUpload;
        }

        public long DocumentoId { get; set; }
        public string Usuario { get; set; }
        public string NomeArquivo { get; set; }
        public DateTime DataUpload { get; set; }
        public string TipoDocumento { get; set; }
        public string Classificacao { get; set; }
        public Guid CodigoArquivo { get; set; }
    }
}
