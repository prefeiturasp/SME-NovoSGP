using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class ConselhoClasseConsolidadoTurmaAlunoMap : BaseMap<ConselhoClasseConsolidadoTurmaAluno>
    {
        public ConselhoClasseConsolidadoTurmaAlunoMap()
        {
            ToTable("consolidado_conselho_classe_aluno_turma");
            Map(c => c.DataAtualizacao).ToColumn("dt_atualizacao");
            Map(c => c.Status).ToColumn("status");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.ParecerConclusivoId).ToColumn("parecer_conclusivo_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.Bimestre).ToColumn("bimestre");
        }
    }
}
