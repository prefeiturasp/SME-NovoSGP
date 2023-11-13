using SME.SGP.Dominio;
using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SGP.Dados
{
    public class ConselhoClasseConsolidadoTurmaAlunoMap : DommelEntityMap<ConselhoClasseConsolidadoTurmaAluno>
    {
        public ConselhoClasseConsolidadoTurmaAlunoMap()
        {
            ToTable("consolidado_conselho_classe_aluno_turma");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.DataAtualizacao).ToColumn("dt_atualizacao");
            Map(c => c.Status).ToColumn("status");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.ParecerConclusivoId).ToColumn("parecer_conclusivo_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.AlteradoEm).ToColumn("alterado_em");
            Map(c => c.AlteradoPor).ToColumn("alterado_por");
            Map(c => c.CriadoRF).ToColumn("criado_rf");
            Map(c => c.AlteradoRF).ToColumn("alterado_rf");
            Map(c => c.CriadoEm).ToColumn("criado_em");
            Map(c => c.CriadoPor).ToColumn("criado_por");

        }
    }
}
