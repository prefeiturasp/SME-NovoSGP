using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class OcorrenciaDto
    {
        public DateTime DataOcorrencia { get; set; }
        public string HoraOcorrencia { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string OcorrenciaTipoId { get; set; }
        public long TurmaId { get; set; }
        public AuditoriaDto Auditoria { get; set; }
        public IEnumerable<OcorrenciaAlunoDto> Alunos { get; set; }

        public OcorrenciaDto()
        {
            Alunos = new List<OcorrenciaAlunoDto>();
        }
    }
}