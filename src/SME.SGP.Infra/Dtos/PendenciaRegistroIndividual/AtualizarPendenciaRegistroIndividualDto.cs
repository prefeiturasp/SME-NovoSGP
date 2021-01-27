using System;

namespace SME.SGP.Infra
{
    public class AtualizarPendenciaRegistroIndividualDto
    {
        public long TurmaId { get; set; }
        public long CodigoAluno { get; set; }
        public DateTime DataRegistro { get; set; }
    }
}