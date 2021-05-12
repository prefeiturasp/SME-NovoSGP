using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class ConselhoClasseConsolidadoTurmaAlunoMap : BaseMap<ConselhoClasseConsolidadoTurmaAluno>
    {
        public ConselhoClasseConsolidadoTurmaAlunoMap()
        {
            ToTable("consolidado_conselho_classe_aluno_turma");
            Map(a => a.DataAtualizacao).ToColumn("dt_atualizacao");
            Map(a => a.AlunoCodigo).ToColumn("aluno_codigo");
            Map(a => a.ParecerConclusivoId).ToColumn("parecer_conclusivo_id");
            Map(a => a.TurmaId).ToColumn("turma_id");
        }
    }
}
