using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class MigracaoFrequenciaTurmaAulaDto
    {
        public MigracaoFrequenciaTurmaAulaDto(string turmaCodigo, long aulaId, int quantidadeAula, long registroFrequenciaId, IEnumerable<string> codigosAlunos)
        {
            TurmaCodigo = turmaCodigo;
            AulaId = aulaId;
            QuantidadeAula = quantidadeAula;
            RegistroFrequenciaId = registroFrequenciaId;
            CodigosAlunos = codigosAlunos;
        }

        public string TurmaCodigo { get; set; }
        public long AulaId { get; set; }
        public int QuantidadeAula { get; set; }
        public long RegistroFrequenciaId { get; set; }
        public IEnumerable<string> CodigosAlunos { get; set; }
    }
}
