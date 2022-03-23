using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class ConselhoClasseConsolidadoTurmaAlunoNotaMap : BaseMap<ConselhoClasseConsolidadoTurmaAlunoNota>
    {
        public ConselhoClasseConsolidadoTurmaAlunoNotaMap()
        {
            ToTable("consolidado_conselho_classe_aluno_turma_nota");            
            Map(c => c.ConselhoClasseConsolidadoTurmaAlunoId).ToColumn("consolidado_conselho_classe_aluno_turma_id");
            Map(c => c.Bimestre).ToColumn("bimestre");
            Map(a => a.Nota).ToColumn("nota");
            Map(a => a.ConceitoId).ToColumn("conceito_id");
            Map(a => a.ComponenteCurricularId).ToColumn("componente_curricular_id");
        }
    }
}
