using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class WFAprovacaoParecerConclusivo : EntidadeBase
    {
        public WFAprovacaoParecerConclusivo()
        {}

        public long? WfAprovacaoId { get; set; }
        [Computed]
        public WorkflowAprovacao WfAprovacao { get; set; }
        [Computed]
        public long ConselhoClasseAlunoId { get; set; }
        [Computed]
        public ConselhoClasseAluno ConselhoClasseAluno { get; set; }
        public long UsuarioSolicitanteId { get; set; }

        public long? ConselhoClasseParecerId { get; set; }
        public long? ConselhoClasseParecerAnteriorId { get; set; }
        [Computed]
        public ConselhoClasseParecerConclusivo ConselhoClasseParecer { get; set; }
        public bool Excluido { get; set; }
        public bool ParecerAlteradoManual { get; set; }
    }
}
