using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class WFAprovacaoParecerConclusivoMap : BaseEntityMap<WFAprovacaoParecerConclusivo>
    {
        public WFAprovacaoParecerConclusivoMap()
        {
            ToTable("wf_aprovacao_parecer_conclusivo");
            Map(nameof(WFAprovacaoParecerConclusivo.WfAprovacaoId), "wf_aprovacao_id");
            Map(nameof(WFAprovacaoParecerConclusivo.ConselhoClasseAlunoId), "conselho_classe_aluno_id");
            Map(nameof(WFAprovacaoParecerConclusivo.UsuarioSolicitanteId), "usuario_solicitante_id");
            Map(nameof(WFAprovacaoParecerConclusivo.ConselhoClasseParecerId), "conselho_classe_parecer_id");
            Map(nameof(WFAprovacaoParecerConclusivo.ConselhoClasseParecerAnteriorId), "conselho_classe_parecer_id_anterior");
            Map(nameof(WFAprovacaoParecerConclusivo.Excluido), "excluido");
            Map(nameof(WFAprovacaoParecerConclusivo.ParecerAlteradoManual), "parecer_alterado_manual");
        }
    }
}