using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ConselhoClasseConsolidadoTurmaAluno : EntidadeBase
    {
        public ConselhoClasseConsolidadoTurmaAluno()
        {
            DataAtualizacao = DateTime.Now;
        }
        public DateTime DataAtualizacao { get; set; }
        public SituacaoConselhoClasse Status { get; set; }
        public string AlunoCodigo { get; set; }
        public long? ParecerConclusivoId { get; set; }
        public long TurmaId { get; set; }
    }
}
