using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ObterDocumentoCompletoDto : AuditoriaDto
    {
        public ObterDocumentoCompletoDto() { }

        public long ClassificacaoId { get; set; }
        public long TipoDocumentoId { get; set; }
        public string UeId { get; set; }
        public string DreId { get; set; }
        public long AnoLetivo { get; set; }
        public string ProfessorRf { get; set; }
        public string NomeArquivo { get; set; }
        public Guid CodigoArquivo { get; set; }
        public long? TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public long? ComponenteCurricularId { get; set; }
    }
}
