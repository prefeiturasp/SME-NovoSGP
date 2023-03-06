using System;

namespace SME.SGP.Infra
{
    public class ObterMotivoAusenciaAlunoFrequenciaDiariaPorAulasIdsDto
    {
        public long AnotacaoId { get; set; }
        public long AulaId { get; set; }
        public DateTime DataAula { get; set; }
        public string MotivoAusencia { get; set; }
    }
}
