using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class ConselhoClasseConsolidadoTurmaAlunoNotaMap : DommelEntityMap<ConselhoClasseConsolidadoTurmaAlunoNota>
    {
        public ConselhoClasseConsolidadoTurmaAlunoNotaMap()
        {
            ToTable("consolidado_conselho_classe_aluno_turma_nota");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.ConselhoClasseConsolidadoTurmaAlunoId).ToColumn("consolidado_conselho_classe_aluno_turma_id");
            Map(c => c.Bimestre).ToColumn("bimestre");
            Map(a => a.Nota).ToColumn("nota");
            Map(a => a.ConceitoId).ToColumn("conceito_id");
            Map(a => a.ComponenteCurricularId).ToColumn("componente_curricular_id");
        }
    }
}
