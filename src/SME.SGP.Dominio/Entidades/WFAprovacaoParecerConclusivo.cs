using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class WFAprovacaoParecerConclusivo : EntidadeBase
    {
        public WFAprovacaoParecerConclusivo()
        {}

        public long? WfAprovacaoId { get; set; }
        public WorkflowAprovacao WfAprovacao { get; set; }
        public long ConselhoClasseAlunoId { get; set; }
        public ConselhoClasseAluno ConselhoClasseAluno { get; set; }
        public long UsuarioSolicitanteId { get; set; }

        public long? ConselhoClasseParecerId { get; set; }
        public long? ConselhoClasseParecerAnteriorId { get; set; }
        public ConselhoClasseParecerConclusivo ConselhoClasseParecer { get; set; }
        public bool Excluido { get; set; }
        public bool ParecerAlteradoManual { get; set; }
    }
}
