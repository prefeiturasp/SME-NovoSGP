using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class DocumentoDto
    {
        public DocumentoDto() { }

        public DocumentoDto(string tipoDocumento, string classificacao, long usuarioId, string usuario, DateTime dataUpload)
        {
            TipoDocumento = tipoDocumento;
            Classificacao = classificacao;
            UsuarioId = usuarioId;
            Usuario = usuario;
            DataUpload = dataUpload;
        }

        public string Usuario { get; set; }
        public DateTime DataUpload { get; set; }
        public string TipoDocumento { get; set; }
        public string Classificacao { get; set; }
        public long UsuarioId { get; set; }
    }
}
