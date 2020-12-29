using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.Ocorrencias.Detalhes
{
    public class OcorrenciaDto : AuditoriaDto
    {
        public string DataOcorrencia { get; set; }
        public string HoraOcorrencia { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public long OcorrenciaTipoId { get; set; }
        public IEnumerable<OcorrenciaAlunoDto> Alunos { get; set; }

        public OcorrenciaDto()
        {
            Alunos = new List<OcorrenciaAlunoDto>();
        }
    }
}