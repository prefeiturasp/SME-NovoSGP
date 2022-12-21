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
        public string ClassificacaoDescricao { get; set; }
        public long TipoDocumentoId { get; set; }
        public string TipoDocumentoDescricao { get; set; }        
        public string UeId { get; set; }
        public string UeNome { get; set; }
        public string DreId { get; set; }
        public string DreNome { get; set; }
        public long AnoLetivo { get; set; }
        public string ProfessorRf { get; set; }
        public long? TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public string TurmaNome { get; set; }
        public int? Modalidade { get; set; }
        public string ModalidadeNome { get; set; }
        public int? Semestre { get; set; }
        public long? ComponenteCurricularId { get; set; }
        public string ComponenteCurricularDescricao { get; set; }
        public List<ArquivoResumidoDto> Arquivos { get; set; }
    }
}
