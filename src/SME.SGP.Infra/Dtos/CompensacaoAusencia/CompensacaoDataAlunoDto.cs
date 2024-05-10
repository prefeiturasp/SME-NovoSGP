using System;

namespace SME.SGP.Infra
{
    public class CompensacaoDataAlunoDto
    {
        public long CompensacaoAusenciaAlunoAulaId { get; set; }
        public long AulaId { get; set; }
        public DateTime DataAula { get; set; }
        public string Descricao { get; set; }
        public long RegistroFrequenciaAlunoId { get; set; }
        public string CodigoAluno { get; set; }
    }
}