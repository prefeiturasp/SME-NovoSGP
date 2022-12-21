using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ObterDocumentoResumidoDto : AuditoriaDto
    {
        public ObterDocumentoResumidoDto()
        {
            Arquivos = new List<ArquivoResumidoDto>();
        }

        public long ClassificacaoId { get; set; }
        public long TipoDocumentoId { get; set; }
        public string UeId { get; set; }
        public string DreId { get; set; }
        public long AnoLetivo { get; set; }
        public string ProfessorRf { get; set; }
        public long? TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public long? ComponenteCurricularId { get; set; }
        public List<ArquivoResumidoDto> Arquivos { get; set; }
    }
}
