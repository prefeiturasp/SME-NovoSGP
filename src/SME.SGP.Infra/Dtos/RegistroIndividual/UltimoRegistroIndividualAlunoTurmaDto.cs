using System;

namespace SME.SGP.Infra
{
    public class UltimoRegistroIndividualAlunoTurmaDto
    {
        public long TurmaId { get; set; }
        public string CodigoAluno { get; set; }
        public DateTime DataRegistro { get; set; }
    }
}