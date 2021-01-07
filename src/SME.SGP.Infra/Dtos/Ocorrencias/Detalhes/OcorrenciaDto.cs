using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class OcorrenciaDto
    {
        public string DataOcorrencia { get; set; }
        public string HoraOcorrencia { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public long OcorrenciaTipoId { get; set; }
        public long TurmaId { get; set; }
        public AuditoriaDto Auditoria { get; set; }
        public IEnumerable<OcorrenciaAlunoDto> Alunos { get; set; }

        public OcorrenciaDto()
        {
            Alunos = new List<OcorrenciaAlunoDto>();
        }
    }
}