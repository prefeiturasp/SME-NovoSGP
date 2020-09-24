using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AnotacaoFrequenciaAlunoDto
    {
        public long Id { get; set; }
        public long MotivoAusenciaId { get; set; }
        public string Anotacao { get; set; }
        public long AulaId { get; set; }
        public string CodigoAluno { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}
