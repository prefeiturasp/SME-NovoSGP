using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ObterDocumentoDto : AuditoriaDto
    {
        public ObterDocumentoDto() { }

        public long ClassificacaoId { get; set; }
        public long TipoDocumentoId { get; set; }
        public string UeId { get; set; }
        public string DreId { get; set; }
        public long ArquivoId { get; set; }
        public long AnoLetivo { get; set; }
        public string ProfessorRf { get; set; }
        public string NomeArquivo { get; set; }
        public Guid CodigoArquivo { get; set; }
    }
}
