using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class TipoDocumentoDto
    {
        public TipoDocumentoDto()
        {
            Classificacoes = new List<ClassificacaoDocumentoDto>();
        }
        public long Id { get; set; }
        public string TipoDocumento { get; set; }
        public List<ClassificacaoDocumentoDto> Classificacoes { get; set; }
    }
}
