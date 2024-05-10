using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FrequenciaTurmaEvasaoMap : DommelEntityMap<FrequenciaTurmaEvasao>
    {
        public FrequenciaTurmaEvasaoMap()
        {
            ToTable("frequencia_turma_evasao");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.Mes).ToColumn("mes");
            Map(c => c.QuantidadeAlunosAbaixo50Porcento).ToColumn("quantidade_alunos_abaixo_50_porcento");
            Map(c => c.QuantidadeAlunos0Porcento).ToColumn("quantidade_alunos_0_porcento");
        }
    }
}
