using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class MensagemConsolidarTurmaConselhoClasseAlunoPorAlunoDto
    {
        public MensagemConsolidarTurmaConselhoClasseAlunoPorAlunoDto(string alunoCodigo, long turmaId, IEnumerable<ConsolidacaoConselhoClasseAlunoMigracaoDto> alunoNotas, long? parecerConclusivo)
        {
            this.AlunoCodigo = alunoCodigo;
            this.AlunoNotas = alunoNotas;
            this.TurmaId = turmaId;
            this.ParecerConclusivo = parecerConclusivo;
        }

        public IEnumerable<ConsolidacaoConselhoClasseAlunoMigracaoDto> AlunoNotas { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public long? ParecerConclusivo { get; set; }
    }
}
